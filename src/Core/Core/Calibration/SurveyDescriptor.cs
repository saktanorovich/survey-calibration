using System.Windows.Media.Media3D;
using Core.Math.Figures;

namespace Core.Calibration {
    public sealed class SurveyDescriptor {
        public double QualityOfFit { get; internal set; }
        public Ellipsoid FittingEllipsoid { get; internal set; }
        public Point3D Radius { get; internal set; }
        public Point3D Center { get; internal set; }
        public int Count { get; internal set; }
        public double Sphericity { get; internal set; }

        internal SurveyDescriptor() {
            QualityOfFit = double.NaN;
        }
    }
}
