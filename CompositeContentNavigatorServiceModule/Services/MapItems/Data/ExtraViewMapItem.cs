﻿using System;
using System.Collections.Generic;

namespace CompositeContentNavigatorServiceModule.Services.MapItems.Data
{
    class ExtraViewMapItem : MapItemDecorator
    {
        public Dictionary<string, ICollection<Type>> ViewTypesKeyedByRegion;

        public ExtraViewMapItem(MapItem mapItem, Dictionary<string, ICollection<Type>> viewTypesKeyedByRegion) : base(mapItem)
        {
            ViewTypesKeyedByRegion = viewTypesKeyedByRegion;
        }
    }
    public static class ExtraViewMapItemHelper
    {
        public static MapItemBuilder WithExtraView(this MapItemBuilder mapItemBuilder, Dictionary<string, ICollection<Type>> viewTypesKeyedByRegion)
        {
            mapItemBuilder.SetupActions.Add(4, item => new ExtraViewMapItem(item, viewTypesKeyedByRegion));
            return mapItemBuilder;
        }
        public static Dictionary<string, ICollection<Type>> GetExtraViews(this MapItem mapItem)
        {
            return (Dictionary<string, ICollection<Type>>)mapItem.FindDecoratedProperty<ExtraViewMapItem>(item => item.ViewTypesKeyedByRegion);
        }
    }
}
