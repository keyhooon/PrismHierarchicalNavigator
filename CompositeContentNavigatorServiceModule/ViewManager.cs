using MaterialDesignThemes.Wpf;
using System.Windows;

namespace CompositeContentNavigator
{
    public class ViewManager : DependencyObject
    {
        public static readonly DependencyProperty HeaderDisplayProperty = DependencyProperty.RegisterAttached(
            "HeaderDisplay", typeof(string), typeof(ViewManager), new PropertyMetadata(null));

        public static void SetHeaderDisplay(DependencyObject element, string value)
        {
            element.SetValue(HeaderDisplayProperty, value);
        }

        public static string GetHeaderDisplay(DependencyObject element)
        {
            return (string)element.GetValue(HeaderDisplayProperty);
        }

        public static readonly DependencyProperty HeaderChipIconProperty = DependencyProperty.RegisterAttached(
            "HeaderChipIcon", typeof(DependencyObject), typeof(ViewManager), new PropertyMetadata());

        public static void SetHeaderChipIcon(DependencyObject element, DependencyObject value)
        {
            element.SetValue(HeaderChipIconProperty, value);
        }

        public static DependencyObject GetHeaderChipIcon(DependencyObject element)
        {
            return (DependencyObject)element.GetValue(HeaderChipIconProperty);
        }

        public static readonly DependencyProperty HeaderPackIconProperty = DependencyProperty.RegisterAttached(
            "HeaderPackIcon", typeof(PackIconKind), typeof(ViewManager), new PropertyMetadata(PackIconKind.Monitor));

        public static void SetHeaderPackIcon(DependencyObject element, PackIconKind value)
        {
            element.SetValue(HeaderPackIconProperty, value);
        }

        public static PackIconKind GetHeaderPackIcon(DependencyObject element)
        {
            return (PackIconKind)element.GetValue(HeaderPackIconProperty);
        }
    }
}
