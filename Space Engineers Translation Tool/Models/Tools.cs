using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using CsvHelper;
using Microsoft.Win32;
using Prism.Mvvm;
using Space_Engineers_Translation_Tool.Models.Objects;
using Space_Engineers_Translation_Tool.Models.Utils;

namespace Space_Engineers_Translation_Tool.Models
{
    public class Tools : BindableBase
    {
        public Tools()
        {
            Initialize();
        }

        public async void Download()
        {
            StatusMessage = "ダウンロードを開始します";
            ProgressMaxValue = TranslationFiles.Count();
            ProgressCurrentValue = 0;
            IsProgress = true;
            try
            {
                if (!Directory.Exists(DownloadDirectoryName)) Directory.CreateDirectory(DownloadDirectoryName);

                foreach (var translationFile in TranslationFiles)
                {
                    StatusMessage = $"{translationFile.Name}をダウンロード中...";
                    var uri = new Uri(translationFile.Url);
                    await Downloader.DownloadAsync(uri,
                        $@"{DownloadDirectoryName}\{translationFile.Name}.csv");
                    translationFile.UpdateLastModifyTime();
                    ProgressCurrentValue += 1;
                }

                StatusMessage = "全てのダウンロードが完了しました";
            }
            finally
            {
                IsProgress = false;
            }
        }

        public async void ApplyTranslation(bool toEnglish=false)
        {
            IsProgress = true;

            // 最終結果用の変数
            var succeedItems = 0;
            var errorItems = 0;
            var notFoundFiles = 0;

            // ファイルの存在確認
            if (!CheckFileExistence())
            {
                MessageBox.Show("ファイルが存在しません。\nダウンロードを行ってください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var translationFile in TranslationFiles)
            {
                TranslationApplier applier;
                try
                {
                    applier = new TranslationApplier(SpaceEngineersDirectory + translationFile.Path);
                }
                catch (FileNotFoundException)
                {
                    notFoundFiles += 1;
                    continue;
                }

                var reader =
                    new CsvReader(new StreamReader($@"{DownloadDirectoryName}\{translationFile.Name}.csv"));
                reader.Configuration.HasHeaderRecord = false;
                reader.Configuration.RegisterClassMap<TranslationSourceMap>();

                reader.Parser.Read();
                reader.Parser.Read();

                var records = reader.GetRecords<TranslationSource>().ToList();

                ProgressCurrentValue = 0;
                ProgressMaxValue = records.Count();

                foreach (var record in records)
                {
                    if (!toEnglish)
                    {
                        if (record.Key.Contains("DisplayName_Item_"))
                        {
                            if (!IsItemTranslationEnabled) continue;
                        }

                        if (record.Key.Contains("DisplayName_Block_"))
                        {
                            if (!IsBlockTranslationEnabled) continue;
                        }
                    }

                    try
                    {
                        await Task.Run(() => applier.Replace(record.Key, toEnglish ? record.EnglishValue : record.Value));
                        StatusMessage = $"{record.Key}を置換";
                        succeedItems += 1;
                    }
                    catch (ArgumentException)
                    {
                        StatusMessage = $"{record.Key}が見つかりませんでした";
                        errorItems += 1;
                    }

                    ProgressCurrentValue += 1;
                }

                reader.Dispose();
                applier.Save();
            }

            StatusMessage = $"正常終了: {succeedItems} | 失敗: {errorItems} | 適用不可だったファイル: {notFoundFiles}";

            IsProgress = false;
        }

        public void ApplyFonts()
        {
            IsProgress = true;
            CopyDirectory("Fonts", $@"{SpaceEngineersDirectory}\Content\Fonts");
            StatusMessage = "Fontを適用しました";
            IsProgress = false;
        }

        public async void ExportXml()
        {
            IsProgress = true;

            if (!CheckFileExistence())
            {
                MessageBox.Show("ファイルが存在しません。\nダウンロードを行ってください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var translationFile in TranslationFiles)
            {
                var xml = new XElement("root");
                var reader =
                    new CsvReader(new StreamReader($@"{DownloadDirectoryName}\{translationFile.Name}.csv"));
                reader.Configuration.HasHeaderRecord = false;
                reader.Configuration.RegisterClassMap<TranslationSourceMap>();

                reader.Parser.Read();
                reader.Parser.Read();

                var records = reader.GetRecords<TranslationSource>().ToList();

                ProgressCurrentValue = 0;
                ProgressMaxValue = records.Count();

                foreach (var record in records)
                {
                    await Task.Run(() =>
                    {
                        var data = new XElement("data",
                            new XAttribute("name", record.Key),
                            new XElement("value", record.EnglishValue));
                        data.SetAttributeValue(XNamespace.Xml + "space", "preserve");
                        xml.Add(data);

                        // 処理が早すぎて画面の更新が発生しない。
                        Thread.Sleep(1);
                    });
                    StatusMessage = $"{record.Key}を追加";
                    ProgressCurrentValue += 1;

                }

                var xdoc = new XDocument(xml);

                if (!Directory.Exists(@"Export\Xml"))
                {
                    Directory.CreateDirectory(@"Export\Xml");
                }

                xdoc.Save(new StreamWriter($@"Export\Xml\{translationFile.Name}.xml"));
                StatusMessage = "Xmlのエクスポート完了";
                reader.Dispose();
            }
            IsProgress = false;
        }

        public void ExportCsv()
        {

        }

        public void ExportDiffXml()
        {

        }

        public void BrowseFolder()
        {
            var fbd = new FolderBrowserDialog
            {
                Description = "Space Engineersのディレクトリを指定してください。"
            };

            if (fbd.ShowDialog() == DialogResult.OK) SpaceEngineersDirectory = fbd.SelectedPath;
        }

        private void Initialize()
        {
            TranslationFiles = Converter.Convert(ListFileLoader.LoadAll());
            ProgressMaxValue = 100;
            ProgressCurrentValue = 0;

            if (string.IsNullOrEmpty(SpaceEngineersDirectory)) GetInstalledLocation();

            IsBlockTranslationEnabled = Config.ConfigData.IsBlockTranslationEnabled;
            IsItemTranslationEnabled = Config.ConfigData.IsItemTranslationEnabled;
            SpaceEngineersDirectory = Config.ConfigData.SpaceEngineersDirectory;
        }

        private bool CheckFileExistence()
        {
            foreach (var translationFile in TranslationFiles)
                if (!File.Exists(string.Format($@"{DownloadDirectoryName}\{translationFile.Name}.csv")))
                    return false;

            return true;
        }

        private void GetInstalledLocation()
        {
            var registryKey = Registry.LocalMachine.OpenSubKey(RegistryKey);

            if (registryKey != null)
                SpaceEngineersDirectory = registryKey.GetValue(RegistryName).ToString();
            else
                BrowseFolder();
        }

        private void CopyDirectory(string srcDirectoryPath, string destDirectoryPath)
        {
            DirectoryInfo dir = new DirectoryInfo(srcDirectoryPath);

            // ソースディレクトリが存在しない場合は例外を投げる。
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException();
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // 宛先のディレクトリが存在しない場合は作成する。
            if (!Directory.Exists(destDirectoryPath))
            {
                Directory.CreateDirectory(destDirectoryPath);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirectoryPath, file.Name);
                file.CopyTo(tempPath, true);
            }

            foreach (DirectoryInfo subDir in dirs)
            {
                string tempPath = Path.Combine(destDirectoryPath, subDir.Name);
                CopyDirectory(subDir.FullName, tempPath);
            }
        }

        #region プライベート変数

        private const string RegistryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 244850";
        private const string RegistryName = "InstallLocation";
        private const string DownloadDirectoryName = "TranslationFiles";

        private List<TranslationFile> _translationFiles;
        private int _progressMaxValue;
        private int _progressCurrentValue;
        private bool _isItemTranslationEnabled;
        private bool _isBlockTranslationEnabled;
        private bool _isProgress;
        private string _spaceEngineersDirectory;
        private string _statusMessage;

        #endregion

        #region プライベート変数参照用プロパティ

        public List<TranslationFile> TranslationFiles
        {
            get => _translationFiles;
            set => SetProperty(ref _translationFiles, value);
        }

        public int ProgressMaxValue
        {
            get => _progressMaxValue;
            set => SetProperty(ref _progressMaxValue, value);
        }

        public int ProgressCurrentValue
        {
            get => _progressCurrentValue;
            set => SetProperty(ref _progressCurrentValue, value);
        }

        public bool IsItemTranslationEnabled
        {
            get => _isItemTranslationEnabled;
            set
            {
                SetProperty(ref _isItemTranslationEnabled, value);
                Config.ConfigData.IsItemTranslationEnabled = value;
                Config.Save();
            }
        }

        public bool IsBlockTranslationEnabled
        {
            get => _isBlockTranslationEnabled;
            set
            {
                SetProperty(ref _isBlockTranslationEnabled, value);
                Config.ConfigData.IsBlockTranslationEnabled = value;
                Config.Save();
            }
        }

        public bool IsProgress
        {
            get => _isProgress;
            set => SetProperty(ref _isProgress, value);
        }

        public string SpaceEngineersDirectory
        {
            get => _spaceEngineersDirectory;
            set
            {
                SetProperty(ref _spaceEngineersDirectory, value);
                Config.ConfigData.SpaceEngineersDirectory = value;
                Config.Save();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        #endregion
    }
}