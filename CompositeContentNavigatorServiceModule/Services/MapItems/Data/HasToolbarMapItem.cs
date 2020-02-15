using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CompositeContentNavigatorServiceModule.Services.MapItems.Data
{
    public class HasToolbarMapItem : MapItemDecorator
    {
        public ObservableCollection<Type> Toolbars { get; private set; }

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
        public static MapItemBuilder WithToolbars(this MapItemBuilder mapItemBuilder, IEnumerable<Type> Toolbars)
        {
            mapItemBuilder.SetupActions.Add(5, item => new HasToolbarMapItem(item, Toolbars));
            return mapItemBuilder;
        }

        public static ObservableCollection<Type> GetToolbars(this MapItem mapItem)
        {
            return (ObservableCollection<Type>)mapItem.FindDecoratedProperty<HasToolbarMapItem>(item => item.Toolbars);
        }
    }
}
