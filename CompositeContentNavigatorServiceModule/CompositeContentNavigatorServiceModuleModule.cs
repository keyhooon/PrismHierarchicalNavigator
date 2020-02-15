using CompositeContentNavigatorServiceModule.Services;
using CompositeContentNavigatorServiceModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace CompositeContentNavigatorServiceModule
{
    public class CompositeContentNavigatorServiceModuleModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .RegisterSingleton<CompositeMapNavigatorService>()
                ;
        }
    }
}