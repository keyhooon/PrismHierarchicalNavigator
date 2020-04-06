using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CompositeContentNavigatorServiceModule.Config;
using CompositeContentNavigatorServiceModule.Services.MapItems;
using CompositeContentNavigatorServiceModule.Services.MapItems.Data;
using Microsoft.Extensions.Configuration;
using Prism.Ioc;
using Prism.Regions;


namespace CompositeContentNavigatorServiceModule.Services
{
    public class CompositeMapNavigatorService
    {
        public string ContentRegionName => _config.ContentRegionName;
        public string ToolbarRegionName => _config.ToolbarRegionName;

        private readonly IRegionManager _regionManager;
        private readonly IContainerRegistry _containerRegistery;
        private readonly ModuleConfig _config;
        private MapItem _selectedItem;

        private readonly Dictionary<string, MapItem> _itemsTagDictionary;
        public ReadOnlyObservableCollection<MapItem> RootItemList { get; }
        private readonly Dictionary<string, MapItem> _itemsViewDictionary;
        private readonly ObservableCollection<MapItem> _rootItemList;

        public CompositeMapNavigatorService(IRegionManager regionManager, IContainerRegistry icontainerRegistery, IConfigurationRoot configurationRoot)
        {

            _regionManager = regionManager;
            _containerRegistery = icontainerRegistery;
            var section = configurationRoot.GetSection(ModuleConfig.SectionName);
            if (section.Exists())
                _config = ConfigurationBinder.Get<ModuleConfig>(section);
            else
                _config = new ModuleConfig();

 
            _rootItemList = new ObservableCollection<MapItem>();
            RootItemList = new ReadOnlyObservableCollection<MapItem>(_rootItemList);
            _itemsViewDictionary = new Dictionary<string, MapItem>();
            _itemsTagDictionary = new Dictionary<string, MapItem>();
            var contentRegion = _regionManager.Regions.Where((region, i) => region.Name == ContentRegionName).FirstOrDefault();
            if (contentRegion != null) contentRegion.ActiveViews.CollectionChanged += ActiveViewsOnCollectionChanged;
            _regionManager.Regions.CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null)
                    foreach (Region region in args.NewItems)
                        if (region.Name == ContentRegionName)
                            region.ActiveViews.CollectionChanged += ActiveViewsOnCollectionChanged;
                if (args.OldItems != null)
                    foreach (Region region in args.OldItems)
                        if (region.Name == ContentRegionName)
                            region.ActiveViews.CollectionChanged -= ActiveViewsOnCollectionChanged;
            };
            if (_config.HasRoot)
                RegisterItem("Root", MapItemBuilder.CreateDefaultBuilder(_config.RootDisplay));
        }

        private void ActiveViewsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                var newActiveView = e.NewItems[0];
                OnActiveView(newActiveView);
            }

            if (e.OldItems != null)
            {
                var oldActiveView = e.OldItems[0];
                OnDeactiveView(oldActiveView);
            }
        }

        private void OnDeactiveView(object newDeactiveView)
        {
            if (!_itemsViewDictionary.TryGetValue(newDeactiveView.GetType().FullName, out var mapItem)) return;
            var toolbars = mapItem.GetToolbars();
            if (toolbars != null && toolbars.Any())
                foreach (var toolbarView in toolbars.Join(_regionManager.Regions[ToolbarRegionName].Views, type => type.FullName, o => o.GetType().FullName, (type, o) => o))
                    _regionManager.Regions[ToolbarRegionName].Remove(toolbarView);


            var extraViews = mapItem.GetExtraViews();
            if (extraViews != null && extraViews.Any())
                foreach (var extraView in extraViews)
                {
                    _regionManager.Regions[extraView.Key].Remove(extraView.Value.Join(_regionManager.Regions[extraView.Key].Views, type => type.FullName, o => o.GetType().FullName, (type, o) => o));

                }
        }
        private void OnActiveView(object newActiveView)
        {
            if (!_itemsViewDictionary.TryGetValue(newActiveView.GetType().FullName, out var mapItem)) return;
            var toolbars = mapItem.GetToolbars();
            if (toolbars != null && toolbars.Any())
                foreach (var toolbar in toolbars)
                {
                    if (_containerRegistery.IsRegistered<object>(toolbar.FullName))
                        _containerRegistery.RegisterSingleton(typeof(object), toolbar, toolbar.FullName);
                    _regionManager.Regions[ToolbarRegionName].RequestNavigate(toolbar.FullName);
                }

            var allExtraViews = mapItem.GetExtraViews();
            if (allExtraViews != null && allExtraViews.Any())
                foreach (var extraViews in allExtraViews)
                    foreach (var extraView in extraViews.Value)
                    {
                        if (!_containerRegistery.IsRegistered<object>(extraView.FullName))
                            _containerRegistery.RegisterSingleton(typeof(object), extraView, extraView.FullName);
                        _regionManager.Regions[extraViews.Key].RequestNavigate(extraView.FullName);
                    }

        }

        public MapItem RegisterItem(string name, MapItemBuilder builder, string parentName = "")
        {
            if (_itemsTagDictionary.ContainsKey(name)) throw new ArgumentException();
            ObservableCollection<MapItem> observableCollection;
            if (parentName != string.Empty)
            {
                if (!_itemsTagDictionary.TryGetValue(parentName, out _selectedItem))
                {
                    throw new ArgumentException("ParentNotFound",nameof(parentName));
                }
                if (!(_selectedItem is CompositeMapItem item))
                {
                    // decorate MapItem with CompositeMapItem 
                    var compositeMapItem = new CompositeMapItem(_itemsTagDictionary[parentName]);
                    // change old MapItem with new CompositeMapItem in TreeList
                    _itemsTagDictionary[parentName] = compositeMapItem;
                    if (_rootItemList.Contains(_selectedItem))
                    {
                        _rootItemList.Remove(_selectedItem);
                        _rootItemList.Add(compositeMapItem);
                    }

                    item = compositeMapItem;
                }

                observableCollection = item.ChildList;
            }
            else
            {
                observableCollection = _rootItemList;
            }
            var mapItem = builder.Build();
            _itemsTagDictionary.Add(name, mapItem);
            var viewType = mapItem.GetViewType();
            if (viewType != null && viewType.FullName != null)
                _itemsViewDictionary.Add(viewType.FullName, mapItem);
            observableCollection.Add(mapItem);
            if (mapItem is CompositeMapItem compositeItem)
                foreach (var item in compositeItem.ChildList)
                {
                    _rootItemList.Remove(item);
                }
            return mapItem;
        }


        public void RequestNavigate(MapItem item)
        {
            var viewType = item.GetViewType();
            if (viewType == null)
                return;
            if (_regionManager.Regions[ContentRegionName].ActiveViews.FirstOrDefault()?.GetType() == viewType)
                return;
            if (!_containerRegistery.IsRegistered<object>(viewType.FullName))
                _containerRegistery.RegisterSingleton(typeof(object), viewType, viewType.FullName);
            _regionManager.Regions[ContentRegionName].RequestNavigate(viewType.FullName, item.GetNavigationParameters());
        }
    }

}
