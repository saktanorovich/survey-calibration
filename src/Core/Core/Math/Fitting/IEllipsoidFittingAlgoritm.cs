using System.Windows.Media.Media3D;
using Core.Math.Figures;

namespace Core.Math.Fitting {
    public interface IEllipsoidFittingAlgoritm
    {
        Ellipsoid Fit(Point3D[] points);
    }
}
