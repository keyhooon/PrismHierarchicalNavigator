using System;
using System.Windows;
using System.Windows.Shapes;

namespace CompositeContentNavigatorServiceModule
{
    public class ViewManager : DependencyObject
    {
        public static readonly DependencyProperty ViewNameProperty = DependencyProperty.RegisterAttached(
            "ViewName", typeof(string), typeof(ViewManager), new PropertyMetadata(default(string)));

        public static void SetViewName(DependencyObject element, string value)
        {
            element.SetValue(ViewNameProperty, value);
        }

        public static string GetViewName(DependencyObject element)
        {
            return (string)element.GetValue(ViewNameProperty);
        }

        public static readonly DependencyProperty ViewImageProperty = DependencyProperty.RegisterAttached(
            "ViewImage", typeof(Path), typeof(ViewManager), new PropertyMetadata(default(Path)));

        public static void SetViewImage(DependencyObject element, Path value)
        {
            element.SetValue(ViewImageProperty, value);
        }

        public static Path GetViewImage(DependencyObject element)
        {
            return (Path)element.GetValue(ViewImageProperty);
        }
    }
}
