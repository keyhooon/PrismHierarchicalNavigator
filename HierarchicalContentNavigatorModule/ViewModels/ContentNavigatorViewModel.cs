using CompositeContentNavigatorServiceModule.Services;
using CompositeContentNavigatorServiceModule.Services.MapItems.Data;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HierarchicalContentNavigatorModule.ViewModels
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
        public DelegateCommand<MapItem> NavigateCommand
        {
            get
            {
                return _navigateCommand
                       ?? (_navigateCommand = new DelegateCommand<MapItem>(
                           (o) =>
                           {
                               _compositeMapService.RequestNavigate(o);
                           }));
            }
        }
    }
}
