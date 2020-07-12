using CompositeContentNavigator.Infrastructure;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace CompositeContentNavigator.Views
{
    /// <summary>
    /// Interaction logic for ActiveViewCollectionView
    /// </summary>
    public partial class ActiveViewCollectionView 
    {
        public ActiveViewCollectionView()
        {
            InitializeComponent();
        }

        private void Chip_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dependencyObject = (DependencyObject)e.OriginalSource;
            var listViewItem = dependencyObject.FindAncestor<ListViewItem>();
            var chip = dependencyObject.FindAncestor<Chip>();

            if (listViewItem != null)
            {
                if (chip != null)
                {
                    ListView.SelectedItem = listViewItem.DataContext;
                    listViewItem.SetValue(ListBoxItem.IsSelectedProperty, true);
                }
                else e.Handled = true; //Handled means will not pass to ListView for selection.
            }

        }
    }
}
