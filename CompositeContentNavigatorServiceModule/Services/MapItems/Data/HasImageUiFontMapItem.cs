using System;
using System.Windows.Media;

namespace CompositeContentNavigatorServiceModule.Services.MapItems.Data
{
    public class HasImageUiFontMapItem : MapItemDecorator
    {
        public char Character{ get; }

        public FontFamily Font { get; }

        internal HasImageUiFontMapItem(MapItem mapItem) : this(mapItem, char.MinValue, null)
        {

        }

        public HasImageUiFontMapItem(MapItem mapItem, char character, FontFamily font) : base(mapItem)
        {
            Character = character;
            Font = font;

        }

    }
    public static class HasImageUiFontMapItemHelper
    {
        public static MapItemBuilder WithImageUiFontPath(this MapItemBuilder mapItemBuilder, char character, FontFamily font)
        {
            if (mapItemBuilder.SetupActions.Keys.Contains(1))
                throw new NotSupportedException("Multi Image for Map Item not Support");
            mapItemBuilder.SetupActions.Add(1, item => new HasImageUiFontMapItem(item, character, font));
            return mapItemBuilder;
        }

        public static char GetImageUiFontChar(this MapItem mapItem)
        {
            return (char)mapItem.FindDecoratedProperty<HasImageUiFontMapItem>(item => item.Character);
        }
        public static FontFamily GetImageUiFontFamily(this MapItem mapItem)
        {
            return (FontFamily)mapItem.FindDecoratedProperty<HasImageUiFontMapItem>(item => item.Font);
        }
    }
}