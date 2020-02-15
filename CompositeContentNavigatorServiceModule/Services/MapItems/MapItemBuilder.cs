
using CompositeContentNavigatorServiceModule.Services.MapItems.Data;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CompositeContentNavigatorServiceModule.Services.MapItems
{
    public class MapItemBuilder
    {
        protected MapItemBuilder(SortedList<int, Func<MapItem, MapItem>> setupActions)
        {
            this.SetupActions = setupActions;
        }

        public static MapItemBuilder CreateDefaultBuilder(string displayName)
        {
            var setupActions = new SortedList<int, Func<MapItem, MapItem>>();
            var builder = new MapItemBuilder(setupActions);
            setupActions.Add(0, item => new MapItem(displayName));
            return builder;
        }

        public static MapItemBuilder CreateViewBuilder<T>(T d) where T : DependencyObject
        {
            var setupActions = new SortedList<int, Func<MapItem, MapItem>>();
            var builder = new MapItemBuilder(setupActions);
            setupActions.Add(2, item => new HasViewMapItem(new MapItem(ViewManager.GetViewName(d) ?? typeof(T).Name), typeof(T)));
            var viewImage = ViewManager.GetViewImage(d);
            if (viewImage != null)
                setupActions.Add(1, item => new HasImageMapItem(item, viewImage));
            return builder;
        }

        public MapItem Build()
        {
            MapItem mapItem = null;
            foreach (var action in SetupActions.Values)
            {
                mapItem = action(mapItem);
            }
            return mapItem;
        }
        public SortedList<int, Func<MapItem, MapItem>> SetupActions { get; }
    }


}
