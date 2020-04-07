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
            regionManager.Regions.CollectionChanged += (sender, args) =>ContentRegion = regionManager.Regions.FirstOrDefault(region => region.Name == _config.ContentRegionName);
        }

        private void ActiveViews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _activeView = ContentRegion.ActiveViews.FirstOrDefault();
            RaisePropertyChanged(nameof(ActiveView));
        }

        private object _activeView;
        public object ActiveView
        {
            get { return _activeView; }
            set { SetProperty(ref _activeView, value, ()=>ContentRegion.Activate(value)); }
        }

        private IViewsCollection _views;
        public IViewsCollection Views
        {
            get { return _views; }
            set { SetProperty(ref _views, value); }
        }
                     
        private IRegion _contentRegion = null;
        public IRegion ContentRegion
        {
            get => _contentRegion;
            private set
            {
                if (_contentRegion == value)
                    return;
                if (_contentRegion != null)
                    _contentRegion.ActiveViews.CollectionChanged -= ActiveViews_CollectionChanged;
                _contentRegion = value;
                if (ContentRegion != null)
                    ContentRegion.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;
                Views = ContentRegion?.Views;
            }
        }

        private DelegateCommand<object> _deleteCommand;
        public DelegateCommand<object> DeleteCommand =>
                    _deleteCommand ??= new DelegateCommand<object>(o =>
                    {
 //                       ContentRegion.NavigationService.Journal.GoBack();
                        ContentRegion.Remove(o);

                    }, o => ContentRegion != null).ObservesProperty(() => ContentRegion);
    }
}
