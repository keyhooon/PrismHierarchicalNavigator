using System.Collections.Specialized;
using System.Windows.Controls;
using Prism.Regions;

namespace CompositeContentNavigator.Infrastructure
{
    public class ToolBarTrayRegionAdapter : RegionAdapterBase<ToolBarTray>
    {
        public ToolBarTrayRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }

        protected override void Adapt(IRegion region, ToolBarTray regionTarget)
        {
            region.Views.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (ToolBar toolBar in e.NewItems)
                    {
                        regionTarget.ToolBars.Add(toolBar);
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (ToolBar toolBar in e.OldItems)
                    {
                        regionTarget.ToolBars.Remove(toolBar);
                    }
                }
            };
        }
    }
}