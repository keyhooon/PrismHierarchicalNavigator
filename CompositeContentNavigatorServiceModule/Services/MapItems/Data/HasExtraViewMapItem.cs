using System;
using System.Collections.Generic;

namespace CompositeContentNavigator.Services.MapItems.Data
{
    class HasExtraViewMapItem : MapItemDecorator
    {
        public Dictionary<string, IEnumerable<Type>> ViewTypesKeyedByRegion;

        public HasExtraViewMapItem(MapItem mapItem, Dictionary<string, IEnumerable<Type>> viewTypesKeyedByRegion) : base(mapItem)
        {
            ViewTypesKeyedByRegion = viewTypesKeyedByRegion;
        }
    }
    public static class ExtraViewMapItemHelper
    {
        public static MapItemBuilder WithExtraView(this MapItemBuilder mapItemBuilder, Dictionary<string, IEnumerable<Type>> viewTypesKeyedByRegion)
        {
            mapItemBuilder.SetupActions.Add(4, item => new HasExtraViewMapItem(item, viewTypesKeyedByRegion));
            return mapItemBuilder;
        }
        public static Dictionary<string, IEnumerable<Type>> GetExtraViews(this MapItem mapItem)
        {
            return (Dictionary<string, IEnumerable<Type>>)mapItem.FindDecoratedProperty<HasExtraViewMapItem>(item => item.ViewTypesKeyedByRegion);
        }
    }
}
