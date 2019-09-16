using Prism.Ioc;
using Prism.Regions;
using System.Windows;
using Unity;

namespace Space_Engineers_Translation_Tool.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        [Dependency]
        public IContainerExtension ContainerExtension { get; set; }

        [Dependency]
        public IRegionManager RegionManager { get; set; }
        public MainView()
        {
            InitializeComponent();
        }

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            RegionManager.AddToRegion("StatusBarRegion", ContainerExtension.Resolve<StatusBarView>());
            RegionManager.AddToRegion("FileListRegion", ContainerExtension.Resolve<FileListView>());
            RegionManager.AddToRegion("ToolTabRegion", ContainerExtension.Resolve<ToolTabView>());
            RegionManager.AddToRegion("SettingsTabRegion", ContainerExtension.Resolve<SettingsTabView>());
            RegionManager.AddToRegion("OthersTabRegion", ContainerExtension.Resolve<OthersTabView>());
        }
    }
}
