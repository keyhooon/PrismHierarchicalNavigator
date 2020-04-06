using CompositeContentNavigator.Services.MapItems;
using Prism.Regions;
using System;

namespace CompositeContentNavigator.Services.MapItems.Data
{
    class HasNavigationParameterMapItem : MapItemDecorator
    {
        public NavigationParameters NavigationParameters { get; }

        internal HasNavigationParameterMapItem(MapItem mapItem) : this(mapItem, null)
        {

        }

        public HasNavigationParameterMapItem(MapItem mapItem, NavigationParameters navigationParameters) : base(mapItem)
        {
            NavigationParameters = navigationParameters;
        }

    }
    public static class HasNavigationParameterMapItemHelper
    {
        public static MapItemBuilder WithNavigationParameters(this MapItemBuilder mapItemBuilder, NavigationParameters navigationParameters)
        {
            if (mapItemBuilder.SetupActions.Keys.Contains(15))
                throw new Exception("This Set more than One time");
            mapItemBuilder.SetupActions.Add(400, item => new HasNavigationParameterMapItem(item, navigationParameters));
            return mapItemBuilder;
        }

        public static NavigationParameters GetNavigationParameters(this MapItem mapItem)
        {
            return (NavigationParameters)mapItem.FindDecoratedProperty<HasNavigationParameterMapItem>(item => item.NavigationParameters);
        }
    }
}
