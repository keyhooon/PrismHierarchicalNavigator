using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace CompositeContentNavigator.Infrastructure
{
    public static class DependencyObjectExtension
    {
        public static T FindAncestor<T>(this DependencyObject obj) where T : DependencyObject
        {
            while (obj != null)
            {
                var o = obj as T;
                if (o != null)
                    return o;
                obj = VisualTreeHelper.GetParent(obj);
            }
            return default;
        }
    }
}
