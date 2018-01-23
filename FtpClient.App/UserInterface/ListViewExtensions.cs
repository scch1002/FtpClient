using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FtpClient.UserInterface
{
    public class ListViewExtensions
    {
        private static SelectedItemsBinder GetSelectedValueBinder(DependencyObject obj)
        {
            return (SelectedItemsBinder)obj.GetValue(SelectedValueBinderProperty);
        }

        private static void SetSelectedValueBinder(DependencyObject obj, SelectedItemsBinder items)
        {
            obj.SetValue(SelectedValueBinderProperty, items);
        }

        private static readonly DependencyProperty SelectedValueBinderProperty = DependencyProperty.RegisterAttached("SelectedValueBinder", typeof(SelectedItemsBinder), typeof(ListViewExtensions), new PropertyMetadata(null));


        public static readonly DependencyProperty SelectedValuesProperty = DependencyProperty.RegisterAttached("SelectedValues", typeof(IList), typeof(ListViewExtensions),
            new PropertyMetadata(null, OnSelectedValuesChanged));


        private static void OnSelectedValuesChanged(DependencyObject o, DependencyPropertyChangedEventArgs value)
        {
            var oldBinder = GetSelectedValueBinder(o);
            if (oldBinder != null)
                oldBinder.UnBind();

            SetSelectedValueBinder(o, new SelectedItemsBinder((ListView)o, (IList)value.NewValue));
            GetSelectedValueBinder(o).Bind();
        }

        public static void SetSelectedValues(Selector elementName, IEnumerable value)
        {
            elementName.SetValue(SelectedValuesProperty, value);
        }

        public static IEnumerable GetSelectedValues(Selector elementName)
        {
            return (IEnumerable)elementName.GetValue(SelectedValuesProperty);
        }
    }
}
