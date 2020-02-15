

using System.Windows.Shapes;

namespace CompositeContentNavigatorServiceModule.Services.MapItems.Data
{
    public class HasImageMapItem : MapItemDecorator
    {
        public Path Image { get; }

        internal HasImageMapItem(MapItem mapItem) : this(mapItem, null)
        {

        }

        public HasImageMapItem(MapItem mapItem,Path image) : base(mapItem)
        {
            Image = image;
        }

    }
    public static class HasImageMapItemHelper
    {
        public static MapItemBuilder WithImage(this MapItemBuilder mapItemBuilder, Path image)
        {
            mapItemBuilder.SetupActions.Add(1, item => new HasImageMapItem(item, image));
            return mapItemBuilder;
        }

        public static Path GetImage(this MapItem mapItem)
        {
            return (Path)mapItem.FindDecoratedProperty<HasImageMapItem>(item => item.Image);
        }
    }
}
