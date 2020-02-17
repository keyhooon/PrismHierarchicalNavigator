using CompositeContentNavigatorServiceModule.Config;
using CompositeContentNavigatorServiceModule.Services;
using CompositeContentNavigatorServiceModule.Services.MapItems;
using CompositeContentNavigatorServiceModule.Views;
using Microsoft.Extensions.Configuration;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace CompositeContentNavigatorServiceModule
{
    public class CompositeContentNavigatorServiceModule : IModule
    {
        private ContentNavigatorConfig _contentNavigatorConfig;

        public void OnInitialized(IContainerProvider containerProvider)
        {

            var compositeMapNavigatorService = containerProvider.Resolve<CompositeMapNavigatorService>();
            var section = containerProvider.Resolve<IConfigurationRoot>().GetSection("ContentNavigatorConfig");
            if (section.Exists())
            {
                var _contentNavigatorConfig = ConfigurationBinder.Get<ContentNavigatorConfig>(section);
                if (_contentNavigatorConfig.HasRoot)
                    compositeMapNavigatorService.RegisterItem("Root",
                                                      MapItemBuilder.CreateDefaultBuilder(_contentNavigatorConfig.RootName)
                                                     );
            }
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion("Header", typeof(ActiveViewCollectionView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .RegisterSingleton<CompositeMapNavigatorService>()
                .RegisterForNavigation<ActiveViewCollectionView>(typeof(ActiveViewCollectionView).FullName)
            ;
        }
    }
}