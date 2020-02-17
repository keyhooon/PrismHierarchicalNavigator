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
    public class HierarchicalContentNavigatorModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion("ContentMap", typeof(ContentNavigatorView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ContentNavigatorView>(typeof(ContentNavigatorView).FullName);
        }
    }
}