using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Clients.CalibrationModule.Converters {
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : BaseConverter, IValueConverter {
        public BoolToVisibilityConverter() {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null) {
                if (targetType == typeof(Visibility)) {
                    if (value is bool) {
                        if ((bool)value) {
                            return Visibility.Visible;
                        }
                        return Visibility.Hidden;
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
