using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ZTimePlanner.PoC.PlannerElements
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ZTimePlanner.PoC.PlannerElements"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ZTimePlanner.PoC.PlannerElements;assembly=ZTimePlanner.PoC.PlannerElements"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:PlannerItemControl/>
    ///
    /// </summary>
    public class PlannerItemControl : Border
    {
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(object), typeof(PlannerItemControl), new PropertyMetadata(null, ItemModified));

        private static void ItemModified(DependencyObject selfItem, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (((PlannerItemControl)selfItem).ChildControl != null && eventArgs.NewValue != null)
                ((PlannerItemControl)selfItem).ChildControl.SetValue(FrameworkElement.DataContextProperty, ((PlannerItemControl)selfItem).Item);
        }

        public object Item
        {
            get { return GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ChildControlProperty =
            DependencyProperty.Register("Template", typeof(FrameworkElement), typeof(PlannerItemControl), new PropertyMetadata(null, ChildCreated));

        private static void ChildCreated(DependencyObject selfItem, DependencyPropertyChangedEventArgs eventArgs)
        {
            //if (((PlannerItemControl)selfItem).ChildControl != null)
            //    ((PlannerItemControl)selfItem).ChildControl.SetValue(FrameworkElement.DataContextProperty, ((PlannerItemControl)selfItem).Item);

            //((PlannerItemControl)selfItem).Child = ((PlannerItemControl)selfItem).ChildControl;
        }

        public FrameworkElement ChildControl
        {
            get { return (FrameworkElement)GetValue(ChildControlProperty); }
            set { SetValue(ChildControlProperty, value); }
        }

        private PlannerItemControl()
        {
            //this.Loaded += PlannerItemControl_Loaded;
            this.Child = this.ChildControl;
        }

        public static PlannerItemControl Create(object item, FrameworkElement childControl, Color? color = null)
        {
            var control = new PlannerItemControl()
            {
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(2),
                Background = color.HasValue ? new SolidColorBrush(color.Value) : Brushes.LightCyan,
                BorderBrush = color.HasValue ? new SolidColorBrush(Color.Multiply(color.Value, 0.8f)) : Brushes.DarkCyan,
                Item = item,
                ChildControl = childControl
            };

            control.ChildControl.SetValue(FrameworkElement.DataContextProperty, item);
            control.Child = control.ChildControl;

            return control;
        }

        private void PlannerItemControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
