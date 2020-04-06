using CompositeContentNavigator.Services;
using CompositeContentNavigator.Views;
using Microsoft.Extensions.Configuration;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace CompositeContentNavigator
{
    public class ContentNavigatorModule : IModule
    {
        private ModuleConfig _config;

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var section = containerProvider.Resolve<IConfigurationRoot>().GetSection(ModuleConfig.SectionName);
            if (section.Exists())
                _config = ConfigurationBinder.Get<ModuleConfig>(section);
            else
                _config = new ModuleConfig();
            
            var regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(_config.HeaderRegionName, typeof(ActiveViewCollectionView));
            regionManager.RegisterViewWithRegion(_config.ContentMapRegionName, typeof(ContentNavigatorView));
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .RegisterSingleton<CompositeMapNavigatorService>()
                ;
            containerRegistry.RegisterForNavigation<ActiveViewCollectionView>(typeof(ActiveViewCollectionView).FullName);
            containerRegistry.RegisterForNavigation<ContentNavigatorView>(typeof(ContentNavigatorView).FullName);
        }
    }
}