using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Space_Engineers_Translation_Tool.ViewModels
{
    public class OthersTabViewModel : BindableBase
    {
        public string GroupBoxOtherTools { get; }
        public string Copyright { get; }
        public string NotImplemented { get; }
        public OthersTabViewModel()
        {
            GroupBoxOtherTools = "その他のツール";
            Copyright = "Copyright (c) 2019 Lazialize All Rights Reserved.";
            NotImplemented = "未実装";
        }
    }
}
