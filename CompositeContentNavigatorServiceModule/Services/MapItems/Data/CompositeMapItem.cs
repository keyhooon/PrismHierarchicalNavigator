
using CompositeContentNavigator.Services.MapItems;
using System.Collections.ObjectModel;

namespace CompositeContentNavigator.Services.MapItems.Data
{
    public class CompositeMapItem : MapItemDecorator
    {

        public ObservableCollection<MapItem> ChildList { get; }

        /// <inheritdoc />
        internal CompositeMapItem(MapItem mapItem) : this(mapItem, new ObservableCollection<MapItem>())
        {

        }

        public CompositeMapItem(MapItem mapItem, Collection<MapItem> childList) : base(mapItem)
        {
            ChildList = new ObservableCollection<MapItem>(childList);
        }
    }

    public static class CompositeMapItemHelper
    {
        public static MapItemBuilder WithChild(this MapItemBuilder mapItemBuilder, Collection<MapItem> childItems)
        {
            mapItemBuilder.SetupActions.Add(3, item => new CompositeMapItem(item, childItems));
            return mapItemBuilder;
        }

        public static ObservableCollection<MapItem> GetChildList(this MapItem mapItem)
        {
            return (ObservableCollection<MapItem>)mapItem.FindDecoratedProperty<CompositeMapItem>(item => item.ChildList);
        }
    }

}
