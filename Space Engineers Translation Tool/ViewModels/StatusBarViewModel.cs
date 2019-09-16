using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Space_Engineers_Translation_Tool.Models;

namespace Space_Engineers_Translation_Tool.ViewModels
{
    public class StatusBarViewModel : BindableBase
    {
        public Tools Tools { get; set; }
        public ReactiveProperty<string> StatusMessage { get; set; }
        public ReactiveProperty<int> ProgressBarMaxValue { get; set; }
        public ReactiveProperty<int> ProgressBarCurrentValue { get; set; }
        public StatusBarViewModel(Tools tools)
        {
            Tools = tools;
            StatusMessage = Tools.ToReactivePropertyAsSynchronized(x => x.StatusMessage);
            ProgressBarMaxValue = Tools.ToReactivePropertyAsSynchronized(x => x.ProgressMaxValue);
            ProgressBarCurrentValue = Tools.ToReactivePropertyAsSynchronized(x => x.ProgressCurrentValue);
        }
    }
}
