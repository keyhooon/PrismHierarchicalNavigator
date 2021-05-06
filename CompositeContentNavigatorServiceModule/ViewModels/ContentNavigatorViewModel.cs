using CompositeContentNavigator.Services;
using CompositeContentNavigator.Services.MapItems.Data;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace CompositeContentNavigator.ViewModels
{
    public class ContentNavigatorViewModel : BindableBase
    {
        private readonly CompositeMapNavigatorService _compositeMapService;



        public ReadOnlyObservableCollection<MapItem> RootItems => _compositeMapService.RootItemList;

        public ContentNavigatorViewModel(CompositeMapNavigatorService compositeMapNavigatorService)
        {

            _compositeMapService = compositeMapNavigatorService;
        }


        private DelegateCommand<MapItem> _navigateCommand;


        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public DelegateCommand<MapItem> NavigateCommand => _navigateCommand ??= new DelegateCommand<MapItem>(_compositeMapService.RequestNavigate);
    }
}
