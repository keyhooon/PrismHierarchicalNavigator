using CompositeContentNavigator.Services.MapItems.Data;
using System;
using System.Collections.Generic;

namespace CompositeContentNavigator.Services.MapItems
{
    public class MapItemBuilder
    {
        protected MapItemBuilder(SortedList<int, Func<MapItem, MapItem>> setupActions)
        {
            SetupActions = setupActions;
        }

        public static MapItemBuilder CreateDefaultBuilder(string displayName)
        {
            var setupActions = new SortedList<int, Func<MapItem, MapItem>>();
            var builder = new MapItemBuilder(setupActions);
            setupActions.Add(0, item => new MapItem(displayName));
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
