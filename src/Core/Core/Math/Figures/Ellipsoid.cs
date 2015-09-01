using System.Windows.Media.Media3D;
using Core.Math.Figures.Forms;

namespace Core.Math.Figures {
    public class Ellipsoid {
        #region Fields

        private readonly EllipsoidParametricForm _parametricForm;
        private readonly EllipsoidQuadraticForm _quadraticForm;

        #endregion

        #region Properties

        public readonly double XRadius;
        public readonly double YRadius;
        public readonly double ZRadius;

        public readonly double Xmax;
        public readonly double Ymax;
        public readonly double Zmax;

        public readonly double[,] InverseTransform;
        public readonly Point3D[] Poles;

        public Point3D Center { get { return _parametricForm.Center; } }

        public double MinRadius {
            get {
                var result = double.MaxValue;
                result = System.Math.Min(result, _parametricForm.XRadius);
                result = System.Math.Min(result, _parametricForm.YRadius);
                result = System.Math.Min(result, _parametricForm.ZRadius);
                return result;
            }
        }

        public double MaxRadius {
            get {
                var result = double.MinValue;
                result = System.Math.Max(result, _parametricForm.XRadius);
                result = System.Math.Max(result, _parametricForm.YRadius);
                result = System.Math.Max(result, _parametricForm.ZRadius);
                return result;
            }
        }

        #endregion

        #region Ctors

        public Ellipsoid(EllipsoidQuadraticForm ellipsoidQuadraticForm) {
            Requires.NotNull(ellipsoidQuadraticForm, "ellipsoidQuadraticForm");
            _quadraticForm = ellipsoidQuadraticForm;
            _parametricForm = ellipsoidQuadraticForm.EllipsoidParametricForm;
            XRadius = _parametricForm.XRadius;
            YRadius = _parametricForm.YRadius;
            ZRadius = _parametricForm.ZRadius;
            Xmax = _parametricForm.GetXmax();
            Ymax = _parametricForm.GetYmax();
            Zmax = _parametricForm.GetZmax();
            InverseTransform = _quadraticForm.AffineTransform;
            Poles = _parametricForm.GetPoles();
        }

        #endregion

        #region Public Methods

        public Point3D GetSurfacePoint(double θ, double φ) {
            return _parametricForm.GetSurfacePoint(θ, φ);
        }

        public bool Contains(Point3D point, double eps) {
            return new DoubleUtils(eps).Sign(_quadraticForm.Evaluate(point)) == 0;
        }

        #endregion
    }
}
