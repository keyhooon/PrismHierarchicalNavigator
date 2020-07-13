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
            set { SetProperty(ref _activeView, value, ()=> { if (value != null) ContentRegion.Activate(value); }); }
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

        private DelegateCommand<object> _closeCommand;
        public DelegateCommand<object> CloseCommand =>
                    _closeCommand ??= new DelegateCommand<object>(o =>
                    {
                        ContentRegion.Remove(o);
                        if (ContentRegion.Views.Any())
                            ContentRegion.Activate(ContentRegion.Views.FirstOrDefault());
                    }, o => ContentRegion != null).ObservesProperty(() => ContentRegion);
        private DelegateCommand _closeAllCommand;
        public DelegateCommand CloseAllCommand =>
                    _closeAllCommand ??= new DelegateCommand(() =>
                    {
                        ContentRegion.RemoveAll();
                    }, () => ContentRegion != null).ObservesProperty(() => ContentRegion);
        private DelegateCommand<object> _closeAllButThisCommand;
        public DelegateCommand<object> CloseAllButThisCommand =>
                    _closeAllButThisCommand ??= new DelegateCommand<object>(o =>
                    {
                        foreach (var view in ContentRegion.Views)
                        {
                            if (o == view)
                                continue;
                            ContentRegion.Remove(view);
                        }
                    }, o => ContentRegion != null).ObservesProperty(() => ContentRegion);
    }
}
