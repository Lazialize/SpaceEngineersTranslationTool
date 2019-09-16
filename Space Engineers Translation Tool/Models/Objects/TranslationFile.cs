using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Engineers_Translation_Tool.Models.Objects
{
    public class TranslationFile : INotifyPropertyChanged
    {
        public TranslationFile(string name, string url, string path)
        {
            Name = name;
            Url = url;
            Path = path;
            UpdateLastModifyTime();
        }

        public string Name { get; }
        public string Url { get; }
        public string Path { get; }

        public string LastModifyTime { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateLastModifyTime()
        {
            string filePath = @"TranslationFiles\{0}.csv";
            LastModifyTime =
                File.Exists(string.Format(filePath, Name)) ?
                    File.GetLastWriteTime(string.Format(filePath, Name)).ToString()
                    :
                    "ダウンロードされていません。";
            RaisePropertyChanged("LastModifyTime");
        }

        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }
}
