using System.Windows;
using System.Windows.Media;

namespace Clients.CalibrationModule.Controls {
    public static class ControlsExtension {
        public static T Search<T>(this DependencyObject obj) where T : DependencyObject {
            var result = obj as T;
            if (result != null) {
                return result;
            }
            var childrenCount = VisualTreeHelper.GetChildrenCount(obj);
            for (var i = 0; i < childrenCount; ++i) {
                var child = Search<T>(VisualTreeHelper.GetChild(obj, i));
                if (child != null) {
                    return child;
                }
            }
            return null;
        }
    }
}
