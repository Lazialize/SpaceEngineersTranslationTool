using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Prism.Events;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Space_Engineers_Translation_Tool.Models;
using Unity;

namespace Space_Engineers_Translation_Tool.ViewModels
{
    public class ToolTabViewModel : BindableBase
    {
        public Tools Tools { get; private set; }

        public string ButtonDownload { get; }
        public string ButtonApplyTranslation { get; }
        public string ButtonApplyFont { get; }
        public string ButtonRevertToEnglish { get; }

        public ReactiveProperty<bool> IsProgress { get; }

        public ReactiveCommand DownloadCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ApplyTranslationCommand { get; }  = new ReactiveCommand();
        public ReactiveCommand ApplyFontsCommand { get; } = new ReactiveCommand();
        public ReactiveCommand RevertToEnglishCommand { get; } = new ReactiveCommand();


        public ToolTabViewModel(Tools tools)
        {
            Tools = tools;
            ButtonDownload = "翻訳ファイルをダウンロード";
            ButtonApplyTranslation = "翻訳を適用";
            ButtonApplyFont = "フォントを適用";
            ButtonRevertToEnglish = "英語に戻す";
            IsProgress = Tools.ToReactivePropertyAsSynchronized(x => x.IsProgress, convert: x => x.Inverse(), convertBack: x => x.Inverse());

            DownloadCommand.Subscribe(_ => Tools.Download());
            ApplyTranslationCommand.Subscribe(_ => Tools.ApplyTranslation());
            ApplyFontsCommand.Subscribe(_ => Tools.ApplyFonts());
            RevertToEnglishCommand.Subscribe(_ => Tools.ApplyTranslation(true));
        }
    }
}
