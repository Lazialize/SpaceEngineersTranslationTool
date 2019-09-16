using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Events;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Space_Engineers_Translation_Tool.Models;
using Space_Engineers_Translation_Tool.Models.Objects;
using Space_Engineers_Translation_Tool.Models.Utils;
using Unity;

namespace Space_Engineers_Translation_Tool.ViewModels
{
    public class FileListViewModel : BindableBase
    {
        [Dependency]
        public Tools Tools { get; set; }
        public string HeaderFileName { get; }
        public string HeaderLastModifyTime { get; }

        public ReactiveProperty<List<TranslationFile>> TranslationFiles { get; private set; }

        public FileListViewModel(Tools tools)
        {
            Tools = tools;
            HeaderFileName = "ファイル名";
            HeaderLastModifyTime = "最終更新日";

            this.TranslationFiles = Tools.ObserveProperty(x => x.TranslationFiles).ToReactiveProperty();
        }
    }
}
