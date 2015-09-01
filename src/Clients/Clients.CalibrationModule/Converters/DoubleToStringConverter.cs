using System;
using System.Globalization;
using System.Windows.Data;

namespace Clients.CalibrationModule.Converters {
    [ValueConversion(typeof(double), typeof(string))]
    public class DoubleToStringConverter : BaseConverter, IValueConverter {
        public DoubleToStringConverter() {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null) {
                if (targetType == typeof(string)) {
                    if (value is double) {
                        var d = (double)value;
                        if (!double.IsNaN(d)) {
                            return string.Format("{0:F4}", d);
                        }
                        return "N/A";
                    }
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
}
