

using System;
using System.Windows.Shapes;

namespace CompositeContentNavigatorServiceModule.Services.MapItems.Data
{
    public class HasImageSvgPathMapItem : MapItemDecorator
    {
        public Path ImageSvgPath { get; }

        internal HasImageSvgPathMapItem(MapItem mapItem) : this(mapItem, null)
        {

        }

        public HasImageSvgPathMapItem(MapItem mapItem,Path imageSvgPath) : base(mapItem)
        {
            ImageSvgPath = imageSvgPath;
        }

    }
    public static class HasImageSvgPathMapItemHelper
    {
        public static MapItemBuilder WithImageSvgPath(this MapItemBuilder mapItemBuilder, Path imageSvgPath)
        {
            if (mapItemBuilder.SetupActions.Keys.Contains(1))
                throw new NotSupportedException("Multi Image for Map Item not Support");
            mapItemBuilder.SetupActions.Add(1, item => new HasImageSvgPathMapItem(item, imageSvgPath));
            return mapItemBuilder;
        }

        public static Path GetImageSvgPath(this MapItem mapItem)
        {
            return (Path)mapItem.FindDecoratedProperty<HasImageSvgPathMapItem>(item => item.ImageSvgPath);
        }
    }
}
