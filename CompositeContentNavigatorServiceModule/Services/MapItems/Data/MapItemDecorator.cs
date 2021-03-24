namespace CompositeContentNavigator.Services.MapItems.Data
{
    public class MapItemDecorator : MapItem
    {
        public MapItem MapItem { get; }

        public MapItemDecorator(string display) : base(display)
        {
            MapItem = this;
        }

        public MapItemDecorator(MapItem mapItem) : base(mapItem)
        {
            MapItem = mapItem;
        }
    }
}
