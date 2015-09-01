using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Clients.CalibrationModule.Controls {
    public class PointVisual3D : ModelVisual3D {
        public PointVisual3D(Point3D point, double radius)
            : this(point, radius, PointVisual3DMaterial.BlackMaterial) {
        }

        public PointVisual3D(Point3D point, double radius, Material material) {
            var meshBuilder = new MeshBuilder();
            meshBuilder.AddSphere(point, radius);
            Visual3DModel = new GeometryModel3D {
                Geometry = meshBuilder.ToMesh(),
                Material = material,
            };
        }
    }
}
