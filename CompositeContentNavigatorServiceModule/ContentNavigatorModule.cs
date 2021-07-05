using System;
using Prism.Ioc;

using System.Windows;
using System.Windows.Controls;

using Prism.Regions;
using System.IO;
using Microsoft.Extensions.Configuration;
using Prism.Modularity;
using CompositeContentNavigator.Infrastructure;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CompositeContentNavigator.Services;
using CompositeContentNavigator.Views;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prism.Microsoft.DependencyInjection;

namespace CompositeContentNavigator
{
    public class ContentNavigatorModule : IModule
    {
        private ContentNavigatorOptions _config;

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            var option = containerProvider.Resolve<IOptions<ContentNavigatorOptions>>();

            regionManager.RegisterViewWithRegion(option.Value.HeaderRegionName, typeof(ActiveViewCollectionView));
            regionManager.RegisterViewWithRegion(option.Value.ContentMapRegionName, typeof(ContentNavigatorView));
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppConfig.json", false, true)
                .Build();

            containerRegistry
                .RegisterSingleton<CompositeMapNavigatorService>()
                .RegisterServices(s =>
                {
                    s.Configure<ContentNavigatorOptions>(configurationRoot.GetSection(nameof(ContentNavigatorOptions)));
                });

            containerRegistry.RegisterForNavigation<ActiveViewCollectionView>(typeof(ActiveViewCollectionView).FullName);
            containerRegistry.RegisterForNavigation<ContentNavigatorView>(typeof(ContentNavigatorView).FullName);

        }
    }
}