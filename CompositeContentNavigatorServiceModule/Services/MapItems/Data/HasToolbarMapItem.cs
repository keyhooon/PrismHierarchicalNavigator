using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CompositeContentNavigator.Services.MapItems.Data
{
    public class HasToolbarMapItem : MapItemDecorator
    {
        public ObservableCollection<Type> Toolbars { get; }

        /// <inheritdoc />
        internal HasToolbarMapItem(MapItem mapItem) : this(mapItem, new List<Type>())
        {

        }

        public HasToolbarMapItem(MapItem mapItem, IEnumerable<Type> toolbarList) : base(mapItem)
        {
            Toolbars = new ObservableCollection<Type>(toolbarList);
        }
    }


    public static class HasToolbarMapItemHelper
    {
        public static MapItemBuilder WithToolBars(this MapItemBuilder mapItemBuilder, IEnumerable<Type> ToolBars)
        {
            mapItemBuilder.SetupActions.Add(5, item => new HasToolbarMapItem(item, ToolBars));
            return mapItemBuilder;
        }

        public static ObservableCollection<Type> GetToolBars(this MapItem mapItem)
        {
            return (ObservableCollection<Type>)mapItem.FindDecoratedProperty<HasToolbarMapItem>(item => item.Toolbars);
        }
    }
}
