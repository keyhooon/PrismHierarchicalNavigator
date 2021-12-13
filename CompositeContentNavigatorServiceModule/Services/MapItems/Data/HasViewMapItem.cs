using System;

namespace CompositeContentNavigator.Services.MapItems.Data
{
    public class HasViewMapItem : MapItemDecorator
    {
        public Type ViewType { get; }
        public HasViewMapItem(MapItem mapItem, Type viewType) : base(mapItem)
        {
            ViewType = viewType;
        }

    }
    public static class HasViewMapItemHelper
    {
        public static MapItemBuilder WithView(this MapItemBuilder mapItemBuilder, Type viewType)
        {
            mapItemBuilder.SetupActions.Add(20, item => new HasViewMapItem(item, viewType));
            return mapItemBuilder;
        }
        public static Type GetViewType(this MapItem mapItem)
        {
            return (Type)mapItem.FindDecoratedProperty<HasViewMapItem>(item => item.ViewType);
        }
    }
}
