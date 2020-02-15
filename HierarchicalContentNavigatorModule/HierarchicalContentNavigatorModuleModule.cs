using CompositeContentNavigatorServiceModule.Services;
using CompositeContentNavigatorServiceModule.Services.MapItems;
using CompositeContentNavigatorServiceModule.Services.MapItems.Data;
using HierarchicalContentNavigatorModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Windows;
using System.Windows.Shapes;

namespace HierarchicalContentNavigatorModule
{
    public class HierarchicalContentNavigatorModuleModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var myResourceDictionary = new ResourceDictionary
            {
                Source = new Uri("/Modules.ContentNavigator;Component/Assets/IconVectorResource.xaml",
                     UriKind.Relative)
            };
            var compositeMapNavigatorService = containerProvider.Resolve<CompositeMapNavigatorService>();
            compositeMapNavigatorService.RegisterItem("Health",
                                                      MapItemBuilder.CreateDefaultBuilder("My Health")
                                                                    .WithImage((Path)myResourceDictionary["Health"])
                                                     );
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion("ContentMap", typeof(ContentNavigatorView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ContentNavigatorView>(typeof(ContentNavigatorView).FullName);
        }
    }
}