using System.Globalization;
using System.Windows.Data;
using System.Windows.Shapes;

namespace ZTimePlanner.PoC.Converters
{
    //[ValueConversion(typeof(object), typeof(double))]
    public class PlannerCellLeftMarginConverter : IValueConverter
    {
        private const double DefaultMargin = 2;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Rectangle && int.TryParse(parameter?.ToString(), out var param))
            {
                var margin = (value as Rectangle).Margin;
                margin.Left = ((value as Rectangle).ActualWidth * param) + DefaultMargin;
                return margin;
            }
            else
                return DefaultMargin;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
