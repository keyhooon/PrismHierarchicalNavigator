using System.Collections.Specialized;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace CompositeContentNavigatorServiceModule.ViewModels
{
    public class ActiveViewCollectionViewModel : BindableBase
    {
        public ActiveViewCollectionViewModel(IRegionManager regionManager)
        {
            ContentRegion = regionManager.Regions.FirstOrDefault(region => region.Name == "Content");
            regionManager.Regions.CollectionChanged += (sender, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (IRegion argsNewItem in args.NewItems)
                            if (argsNewItem.Name == "Content")
                                ContentRegion = argsNewItem;
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (IRegion argsNewItem in args.OldItems)
                            if (argsNewItem.Name == "Content")
                                ContentRegion = null;
                        break;
                }
            };
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
            set => SetProperty(ref _contentRegion, value);
        }

        private DelegateCommand<object> _navigateCommand;
        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public DelegateCommand<object> NavigateCommand
        {
            get
            {
                if (_navigateCommand == null)
                {
                    _navigateCommand = new DelegateCommand<object>(o => { if (o != null) ContentRegion.Activate(o); }, o => ContentRegion != null).ObservesProperty(() => ContentRegion);
                }

                return _navigateCommand;
            }
        }
        private DelegateCommand<object> _deleteCommand;


        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public DelegateCommand<object> DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new DelegateCommand<object>(o =>
                    {
                        ContentRegion.Remove(o);
                        ContentRegion.NavigationService.Journal.GoBack();
                    }, o => ContentRegion != null).ObservesProperty(() => ContentRegion);
                }
                return _deleteCommand;
            }
        }

    }
}
