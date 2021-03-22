using Prism.Ioc;
using Demo.Views;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Prism.Modularity;
using CompositeContentNavigator;
using Prism.Unity;
using CompositeContentNavigator.Services;
using CompositeContentNavigator.Services.MapItems;
using CompositeContentNavigator.Services.MapItems.Data;
using System.Collections.ObjectModel;
using System.IO;
using MaterialDesignThemes.Wpf;
using Prism.Regions;
using System.Windows.Controls;
using CompositeContentNavigator.Infrastructure;

namespace Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppConfig.json", false, true);
            containerRegistry.RegisterInstance(configurationBuilder.Build());
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings?.RegisterMapping(typeof(ToolBarTray), Container.Resolve<ToolBarTrayRegionAdapter>());
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<ContentNavigatorModule>();
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            var compositeMapNavigatorService = Container.Resolve<CompositeMapNavigatorService>();

            compositeMapNavigatorService.RegisterItem("Cardio", MapItemBuilder.CreateDefaultBuilder("Cardio").WithImagePackIcon(PackIconKind.Heart).WithChild(new Collection<MapItem> {
                    compositeMapNavigatorService.RegisterItem("CardioSignal",MapItemBuilder.CreateDefaultBuilder("Signal").WithToolbars(new[]{typeof(Toolbar1) }).WithView(typeof(View1)).WithImagePackIcon(PackIconKind.Signal)),
                    compositeMapNavigatorService.RegisterItem("CardioAnalysis",MapItemBuilder.CreateDefaultBuilder("Analysis").WithView(typeof(View2)).WithImagePackIcon(PackIconKind.Analog))
                }));
        }
    }
}
