using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CompositeContentNavigator.Services.MapItems;
using CompositeContentNavigator.Services.MapItems.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;


namespace CompositeContentNavigator.Services
{
    public class CompositeMapNavigatorService : BindableBase
    {
        public string ContentRegionName => _options.Value.ContentRegionName;
        public string ToolbarRegionName => _options.Value.ToolbarRegionName;

        private readonly IRegionManager _regionManager;
        private readonly IContainerRegistry _container;
        private readonly IOptions<ContentNavigatorOptions> _options;
        private MapItem _selectedItem;

        private readonly Dictionary<string, MapItem> _itemsTagDictionary;
        public ReadOnlyObservableCollection<MapItem> RootItemList { get; }
        private readonly Dictionary<string, MapItem> _itemsViewDictionary;
        private readonly ObservableCollection<MapItem> _rootItemList;

        public event EventHandler ActiveViewOnContentRegionChanged;


        public object CurrentView { get; set; }
        public IRegion ContentRegion { get; }
        public IRegion ToolBarRegion { get; }

        public CompositeMapNavigatorService(IRegionManager regionManager, IContainerExtension container, IOptions<ContentNavigatorOptions> options)
        {

            _regionManager = regionManager;
            _container = container;
            _options = options;
       
 
            _rootItemList = new ObservableCollection<MapItem>();
            RootItemList = new ReadOnlyObservableCollection<MapItem>(_rootItemList);
            _itemsViewDictionary = new Dictionary<string, MapItem>();
            _itemsTagDictionary = new Dictionary<string, MapItem>();
            ContentRegion = _regionManager.Regions.Where((region, i) => region.Name == ContentRegionName).FirstOrDefault();
            ToolBarRegion = _regionManager.Regions.Where((region, i) => region.Name == ToolbarRegionName).FirstOrDefault();
            if (ContentRegion != null) ContentRegion.ActiveViews.CollectionChanged += ActiveViewsOnCollectionChanged;
            if (options.Value.HasRoot)
                RegisterItem("Root",MapItemBuilder.CreateDefaultBuilder(options.Value.RootDisplay).WithImagePackIcon(options.Value.RootPackIcon) );
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
            ActiveViewOnContentRegionChanged?.Invoke(this, null);
        }

        private void OnDeactiveView(object newDeactiveView)
        {
            if (!_itemsViewDictionary.TryGetValue(newDeactiveView.GetType().FullName, out var mapItem)) 
                throw new KeyNotFoundException();

            var toolBars = mapItem.GetToolBars();
            if (toolBars != null)
                foreach (var toolbarView in toolBars.Join(_regionManager.Regions[ToolbarRegionName].Views, type => type.FullName, o => o.GetType().FullName, (type, o) => o))
                    _regionManager.Regions[ToolbarRegionName].Remove(toolbarView);
            


            var allExtraViews = mapItem.GetExtraViews();
            if (allExtraViews != null)
                foreach (var (regionName, extraViews) in allExtraViews)
                    foreach (var extraView in extraViews.Join(_regionManager.Regions[regionName].Views, type => type.FullName, o => o.GetType().FullName, (type, o) => o))
                        _regionManager.Regions[regionName].Remove(extraView);

        }
        private void OnActiveView(object newActiveView)
        {
            if (!_itemsViewDictionary.TryGetValue(newActiveView.GetType().FullName!, out var mapItem)) return;



            var toolBars = mapItem.GetToolBars();
            if (toolBars != null)
                foreach (var toolbar in toolBars)
                {
                    if (!_container.IsRegistered<object>(toolbar.FullName))
                        _container.RegisterSingleton(typeof(object), toolbar, toolbar.FullName);
                    ToolBarRegion.RequestNavigate(toolbar.FullName);
                }

            var allExtraViews = mapItem.GetExtraViews();
            if (allExtraViews != null)
                foreach (var (regionName, extraViews) in allExtraViews)
                    foreach (var extraView in extraViews)
                    {
                        if (!_container.IsRegistered<object>(extraView.FullName))
                            _container.RegisterSingleton(typeof(object), extraView, extraView.FullName);
                        _regionManager.Regions[regionName].RequestNavigate(extraView.FullName);
                    }

        }

        public MapItem RegisterItem(string name, MapItemBuilder builder, string parentName = "")
        {
            if (_itemsTagDictionary.ContainsKey(name)) throw new ArgumentException();
            ObservableCollection<MapItem> observableCollection;
            if (parentName != string.Empty)
            {
                if (!_itemsTagDictionary.TryGetValue(parentName, out _selectedItem))
                    throw new ArgumentException("ParentNotFound",nameof(parentName));
                if (!(_selectedItem is CompositeMapItem item))
                {
                    // decorate MapItem with CompositeMapItem 
                    var parentItem = new CompositeMapItem(_itemsTagDictionary[parentName]);
                    // change old MapItem with new CompositeMapItem in TreeList
                    _itemsTagDictionary[parentName] = parentItem;
                    if (_rootItemList.Contains(_selectedItem))
                    {
                        _rootItemList.Remove(_selectedItem);
                        _rootItemList.Add(parentItem);
                    }

                    item = parentItem;
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
            if (!_container.IsRegistered<object>(viewType.FullName))
                _container.RegisterSingleton(typeof(object), viewType, viewType.FullName);
            _regionManager.Regions[ContentRegionName].RequestNavigate(viewType.FullName, item.GetNavigationParameters());
        }
    }

}
