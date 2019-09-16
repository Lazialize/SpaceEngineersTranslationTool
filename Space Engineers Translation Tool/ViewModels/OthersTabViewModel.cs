using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Space_Engineers_Translation_Tool.Models;

namespace Space_Engineers_Translation_Tool.ViewModels
{
    public class OthersTabViewModel : BindableBase
    {
        public Tools Tools { get; private set; }
        public string GroupBoxOtherTools { get; }
        public string Copyright { get; }
        public string ExportXml { get; }
        public string ExportCsv { get; }

        public ReactiveProperty<bool> IsProgress { get; }

        public ReactiveCommand ExportXmlCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ExportCsvCommand { get; } = new ReactiveCommand();
        public OthersTabViewModel(Tools tools)
        {
            Tools = tools;
            GroupBoxOtherTools = "その他のツール";
            Copyright = "Copyright (c) 2019 Lazialize All Rights Reserved.";
            ExportXml = "XMLをエクスポート";
            ExportCsv = "CSVをエクスポート";
            IsProgress = Tools.ToReactivePropertyAsSynchronized(x => x.IsProgress, convert: x => x.Inverse(), convertBack: x => x.Inverse());
            ExportXmlCommand.Subscribe(_ => Tools.ExportXml());
            ExportCsvCommand.Subscribe(_ => Tools.ExportCsv());
        }
    }
}
