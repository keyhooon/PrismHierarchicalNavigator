using System.Collections.Specialized;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace CompositeContentNavigator.ViewModels
{
    public class ActiveViewCollectionViewModel : BindableBase
    {
        private readonly ModuleConfig _config;
        public ActiveViewCollectionViewModel(IRegionManager regionManager, IConfigurationRoot configurationRoot)
        {
            var section = configurationRoot.GetSection(ModuleConfig.SectionName);
            if (section.Exists())
                _config = ConfigurationBinder.Get<ModuleConfig>(section);
            else
                _config = new ModuleConfig();

            ContentRegion = regionManager.Regions.FirstOrDefault(region => region.Name == _config.ContentRegionName);
            if (ContentRegion != null)
                ContentRegion.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;
            regionManager.Regions.CollectionChanged += (sender, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (IRegion argsNewItem in args.NewItems)
                            if (argsNewItem.Name == _config.ContentRegionName)
                            {
                                ContentRegion = argsNewItem;
                                ContentRegion.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;
                            }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (IRegion argsNewItem in args.OldItems)
                            if (argsNewItem.Name == _config.ContentRegionName)
                            {
                                ContentRegion.ActiveViews.CollectionChanged -= ActiveViews_CollectionChanged;
                                ContentRegion = null;
                            }
                        break;
                }
            };
        }

        private void ActiveViews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
 //           ActiveView = ContentRegion.ActiveViews.FirstOrDefault();
        }

        private object _activeView;
        public object ActiveView
        {
            get { return _activeView; }
            set { SetProperty(ref _activeView, value); }
        }



        /// <summary>
        /// The <see cref="ContentRegion" /> property's name.
        /// </summary>
        private IRegion _contentRegion = null;

        /// <summary>
        /// Sets and gets the ContentRegion property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IRegion ContentRegion
        {
            get => _contentRegion;
            private set => SetProperty(ref _contentRegion, value);
        }

        private DelegateCommand<object> _navigateCommand;
        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public DelegateCommand<object> NavigateCommand =>
                    _navigateCommand ??= new DelegateCommand<object>(o => { if (o != null) ContentRegion.Activate(o); }, o => ContentRegion != null).ObservesProperty(() => ContentRegion);



        private DelegateCommand<object> _deleteCommand;
        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public DelegateCommand<object> DeleteCommand =>
                    _deleteCommand ??= new DelegateCommand<object>(o =>
                    {
//                        ContentRegion.NavigationService.Journal.
                        ContentRegion.Remove(o);

                    }, o => ContentRegion != null).ObservesProperty(() => ContentRegion);
    }
}
