using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using Space_Engineers_Translation_Tool.Models;
using Space_Engineers_Translation_Tool.ViewModels;
using Space_Engineers_Translation_Tool.Views;

namespace Space_Engineers_Translation_Tool
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : PrismApplication {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(new Tools());
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            // ViewModelとViewの関連付け
            ViewModelLocationProvider.Register<MainView, MainViewModel>();
            ViewModelLocationProvider.Register<StatusBarView, StatusBarViewModel>();
            ViewModelLocationProvider.Register<FileListView, FileListViewModel>();
            ViewModelLocationProvider.Register<ToolTabView, ToolTabViewModel>();
            ViewModelLocationProvider.Register<SettingsTabView, SettingsTabViewModel>();
            ViewModelLocationProvider.Register<OthersTabView, OthersTabViewModel>();
        }
    }
}
