using MaterialDesignThemes.Wpf;
using System;

namespace CompositeContentNavigatorServiceModule.Services.MapItems.Data
{
    public class hasImagePackIconMapItem : MapItemDecorator
    {
        public PackIconKind Kind { get; }

        internal hasImagePackIconMapItem(MapItem mapItem) : this(mapItem, PackIconKind.Abc)
        {
            
        }

        public hasImagePackIconMapItem(MapItem mapItem, PackIconKind kind) : base(mapItem)
        {
            Kind = kind;
        }

    }
    public static class hasImagePackIconMapItemHelper
    {
        public static MapItemBuilder WithImagePackIcon(this MapItemBuilder mapItemBuilder, PackIconKind kind)
        {
            if (mapItemBuilder.SetupActions.Keys.Contains(1))
                throw new NotSupportedException("Multi Image for Map Item not Support");
            mapItemBuilder.SetupActions.Add(1, item => new hasImagePackIconMapItem(item, kind));
            return mapItemBuilder;
        }

        public static PackIconKind GetImagePackIcon(this MapItem mapItem)
        {
            return (PackIconKind)mapItem.FindDecoratedProperty<hasImagePackIconMapItem>(item => item.Kind);
        }
    }
}
