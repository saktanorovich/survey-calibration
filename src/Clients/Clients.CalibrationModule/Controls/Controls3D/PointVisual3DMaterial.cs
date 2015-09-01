using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Clients.CalibrationModule.Controls {
    public static class PointVisual3DMaterial {
        public static readonly Material BlackMaterial;
        public static readonly Material RedMaterial;

        static PointVisual3DMaterial() {
            BlackMaterial = MakeMaterial(Colors.Black, 0.8);
            RedMaterial = MakeMaterial(Colors.Red, 1.0);
        }

        private static Material MakeMaterial(Color color, double opacity) {
            var result = MaterialHelper.CreateMaterial(
                new SolidColorBrush(color) {
                    Opacity = opacity
                });
            result.Freeze();
            return result;
        }
    }
}
