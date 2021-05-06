using CompositeContentNavigator.Services.MapItems;
using MaterialDesignThemes.Wpf;
using System;

namespace CompositeContentNavigator.Services.MapItems.Data
{
    public class HasImagePackIconMapItem : MapItemDecorator
    {
        public PackIconKind Kind { get; }

        internal HasImagePackIconMapItem(MapItem mapItem) : this(mapItem, PackIconKind.Abc)
        {
            
        }

        public HasImagePackIconMapItem(MapItem mapItem, PackIconKind kind) : base(mapItem)
        {
            Kind = kind;
        }

    }
    public static class HasImagePackIconMapItemHelper
    {
        public static MapItemBuilder WithImagePackIcon(this MapItemBuilder mapItemBuilder, PackIconKind kind)
        {
            if (mapItemBuilder.SetupActions.Keys.Contains(1))
                throw new NotSupportedException("Multiple Image for Map Item not Support");
            mapItemBuilder.SetupActions.Add(1, item => new HasImagePackIconMapItem(item, kind));
            return mapItemBuilder;
        }

        public static PackIconKind GetImagePackIcon(this MapItem mapItem)
        {
            return (PackIconKind)mapItem.FindDecoratedProperty<HasImagePackIconMapItem>(item => item.Kind);
        }
    }
}
