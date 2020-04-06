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
            configurationBuilder.AddJsonFile("AppConfig.json", true, true);
            containerRegistry.RegisterInstance(configurationBuilder.Build());


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

            compositeMapNavigatorService.RegisterItem("Cardio", MapItemBuilder.CreateDefaultBuilder("Cardio").WithChild(new Collection<MapItem> {
                    compositeMapNavigatorService.RegisterItem("CardioSignal",MapItemBuilder.CreateDefaultBuilder("Signal").WithView(typeof(View1))),
                    compositeMapNavigatorService.RegisterItem("CardioAnalysis",MapItemBuilder.CreateDefaultBuilder("Analysis").WithView(typeof(View2))),
                }));
        }
    }
}
