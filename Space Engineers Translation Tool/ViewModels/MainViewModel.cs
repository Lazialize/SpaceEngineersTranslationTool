using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Reactive.Bindings;

namespace Space_Engineers_Translation_Tool.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public string Title { get; }
        public string GroupBoxHeader { get; }
        public string TabHeaderOthers { get; }
        public string TabHeaderSettings { get; }
        public string TabHeaderTools { get; }
        public MainViewModel()
        {
            Title = "Space Engineers 翻訳ツール";
            GroupBoxHeader = "翻訳ファイル";
            TabHeaderOthers = "その他";
            TabHeaderSettings = "設定";
            TabHeaderTools = "ツール";
        }
    }
}
