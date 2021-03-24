using System;
using System.ComponentModel.Design;

namespace CompositeContentNavigator.Services.MapItems.Data
{
    public class MapItem
    {
        public MapItem(string display)
        {
            Display = display;
        }

        public MapItem(MapItem mapItem)
        {
            Display = mapItem.Display;
        }

        public string Display { get; }
    }
    public static class ExtraMapItemHelper
    {

        public static object FindDecoratedProperty<T>(this MapItem mapItem, Func<T, object> propertySelector) where T : MapItemDecorator
        {
            while (mapItem is MapItemDecorator item)
            {
                if (item is T extraViewMapItem)
                    return propertySelector(extraViewMapItem);
                mapItem = item.MapItem;
            }
            return null;
        }
    }

}
