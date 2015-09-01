using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace Clients.CalibrationModule.Converters {
    [ValueConversion(typeof(Point3D), typeof(string))]
    public class Point3DToStringConverter : BaseConverter, IValueConverter {
        public Point3DToStringConverter() {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null) {
                if (targetType == typeof(string)) {
                    var point = (Point3D)value;
                    return string.Format("({0:F2}, {1:F2}, {2:F2})", point.X, point.Y, point.Z);
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
}
