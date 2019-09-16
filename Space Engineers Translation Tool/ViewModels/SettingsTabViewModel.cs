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
    public class SettingsTabViewModel : BindableBase
    {
        public Tools Tools { get; private set; }

        public string CheckBoxTranslateItem { get; }
        public string CheckBoxTranslateBlock { get; }
        public string ButtonBrowse { get; }
        public string TextBlockSpaceEngineersDirectory { get; }

        public ReactiveProperty<bool> IsItemTranslationEnabled { get; set; }
        public ReactiveProperty<bool> IsBlockTranslationEnabled { get; set; }
        public ReactiveProperty<string> SpaceEngineersDirectory { get; set; }

        public ReactiveCommand BrowseFolderCommand { get; } = new ReactiveCommand();
        public SettingsTabViewModel(Tools tools)
        {
            Tools = tools;

            CheckBoxTranslateItem = "アイテム名を翻訳";
            CheckBoxTranslateBlock = "ブロック名を翻訳";
            ButtonBrowse = "参照";
            TextBlockSpaceEngineersDirectory = "Space Engineersのディレクトリ";

            IsItemTranslationEnabled = Tools.ToReactivePropertyAsSynchronized(x => x.IsItemTranslationEnabled);
            IsBlockTranslationEnabled = Tools.ToReactivePropertyAsSynchronized(x => x.IsBlockTranslationEnabled);
            SpaceEngineersDirectory = Tools.ToReactivePropertyAsSynchronized(x => x.SpaceEngineersDirectory);

            BrowseFolderCommand.Subscribe(_ => Tools.BrowseFolder());
        }
    }
}
