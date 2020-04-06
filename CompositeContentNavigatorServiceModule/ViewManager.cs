using MaterialDesignThemes.Wpf;
using System.Windows;

namespace CompositeContentNavigator
{
    public class ViewManager : DependencyObject
    {
        public static readonly DependencyProperty HeaderDisplayProperty = DependencyProperty.RegisterAttached(
            "HeaderDisplay", typeof(string), typeof(ViewManager), new PropertyMetadata(default(string)));

        public static void SetHeaderDisplay(DependencyObject element, string value)
        {
            element.SetValue(HeaderDisplayProperty, value);
        }

        public static string GetHeaderDisplay(DependencyObject element)
        {
            return (string)element.GetValue(HeaderDisplayProperty);
        }

        public static readonly DependencyProperty HeaderChipIconProperty = DependencyProperty.RegisterAttached(
            "HeaderChipIcon", typeof(ContentElement), typeof(ViewManager), new PropertyMetadata());

        public static void SetHeaderChipIcon(DependencyObject element, ContentElement value)
        {
            element.SetValue(HeaderChipIconProperty, value);
        }

        public static ContentElement GetHeaderChipIcon(DependencyObject element)
        {
            return (ContentElement)element.GetValue(HeaderChipIconProperty);
        }

        public static readonly DependencyProperty HeaderPackIconProperty = DependencyProperty.RegisterAttached(
            "HeaderPackIcon", typeof(PackIconKind), typeof(ViewManager), new PropertyMetadata(PackIconKind.About));

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
