using System.Globalization;
using System.Windows.Data;

namespace ZTimePlanner.PoC.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    public class PlannerCellWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value?.ToString(), out var width))
            {
                if (double.TryParse(parameter?.ToString(), culture.NumberFormat, out var param) && param > 0)
                    width = width * param;
                return width;
            }
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
