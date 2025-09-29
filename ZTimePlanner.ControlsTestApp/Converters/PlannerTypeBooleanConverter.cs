using System.Globalization;
using System.Windows.Data;
using ZTimePlanner.Controls.Models;

namespace ZTimePlanner.ControlsTestApp.Converters
{
    [ValueConversion(typeof(PlannerTypes), typeof(bool))]
    public class PlannerTypeBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PlannerTypes && !String.IsNullOrEmpty(value.ToString()) && parameter is string && !String.IsNullOrEmpty(parameter.ToString()))
                return value.ToString() == parameter.ToString();

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (value is bool) && (bool)value ? parameter : null;
        }
    }
}
