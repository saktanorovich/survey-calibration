using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Accord.Math;

namespace Core.Math.Figures.Forms {
    /**
     * Defines an ellipsoid in the form
     *     { x }            { rx sin(θ) cos(φ) }   { a }
     *     { y } = Rx Ry Rz { ry sin(θ) sin(φ) } + { b }
     *     { z }            {    rz cos(θ)     }   { c }
     * where Rx, Ry, Rz - rotation matrices, (a, b, c) - ellipsoid center, 0 ≤ θ ≤ π, 0 ≤ φ < 2π - parameters.
     */
    public sealed class EllipsoidParametricForm {
        #region Fields

        private double _xmax;
        private double _ymax;
        private double _zmax;

        #endregion

        #region Properties

        public readonly double XRadius;
        public readonly double YRadius;
        public readonly double ZRadius;
        public readonly Point3D Center;
        public double[,] Rotation { get; private set; }

        #endregion

        #region Ctors

        public EllipsoidParametricForm(double xradius, double yradius, double zradius)
            : this(xradius, yradius, zradius, new Point3D(0, 0, 0)) {
        }

        public EllipsoidParametricForm(double xradius, double yradius, double zradius, Point3D center)
            : this(xradius, yradius, zradius, center, 0, 0, 0) {
        }

        public EllipsoidParametricForm(double xradius, double yradius, double zradius, Point3D center, double xrotAngel, double yrotAngel, double zrotAngel) {
            XRadius = xradius;
            YRadius = yradius;
            ZRadius = zradius;
            Center = center;
            var x = Geometry3DUtils.Get3DxRotationMatrix(System.Math.Cos(xrotAngel), System.Math.Sin(xrotAngel));
            var y = Geometry3DUtils.Get3DyRotationMatrix(System.Math.Cos(yrotAngel), System.Math.Sin(yrotAngel));
            var z = Geometry3DUtils.Get3DzRotationMatrix(System.Math.Cos(zrotAngel), System.Math.Sin(zrotAngel));
            Rotation = x.Multiply(y).Multiply(z);
            CalculateMaxDistances();
        }

        public EllipsoidParametricForm(double xradius, double yradius, double zradius, Point3D center, double[,] rotation) {
            XRadius = xradius;
            YRadius = yradius;
            ZRadius = zradius;
            Center = center;
            Rotation = rotation;
            CalculateMaxDistances();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Returns cartesian coordinates of the point with spherical coordinates (r*, θ, φ).
        /// </summary>
        /// <param name="θ">The polar angel, 0 ≤ θ ≤ π.</param>
        /// <param name="φ">The azimuth angel, 0 ≤ φ &lt; 2*π.</param>
        /// <returns>Cartesian coordinates of the point.</returns>
        internal Point3D GetSurfacePoint(double θ, double φ) {
            var pt = Rotation.Multiply(new[] {
                XRadius * System.Math.Sin(θ) * System.Math.Cos(φ),
                YRadius * System.Math.Sin(θ) * System.Math.Sin(φ),
                ZRadius * System.Math.Cos(θ),
            });
            return new Point3D(pt[0] + Center.X, pt[1] + Center.Y, pt[2] + Center.Z);
        }

        internal Point3D[] GetPoles() {
            var result = new Point3D[6];
            result[0] = ToPoint3D(Rotation.Multiply(new[] { -XRadius, 0, 0 }));
            result[1] = ToPoint3D(Rotation.Multiply(new[] { +XRadius, 0, 0 }));
            result[2] = ToPoint3D(Rotation.Multiply(new[] { 0, -YRadius, 0 }));
            result[3] = ToPoint3D(Rotation.Multiply(new[] { 0, +YRadius, 0 }));
            result[4] = ToPoint3D(Rotation.Multiply(new[] { 0, 0, -ZRadius }));
            result[5] = ToPoint3D(Rotation.Multiply(new[] { 0, 0, +ZRadius }));
            for (var i = 0; i < 6; ++i) {
                result[i] = new Point3D(result[i].X + Center.X, result[i].Y + Center.Y, result[i].Z + Center.Z);
            }
            return result;
        }

        internal double GetXmax() { return _xmax; }
        internal double GetYmax() { return _ymax; }
        internal double GetZmax() { return _zmax; }

        #endregion

        #region Private Methods

        private void CalculateMaxDistances() {
            const int tdiv = 180;
            const int pdiv = 360;
            const double dθ = 1 * System.Math.PI / tdiv;
            const double dφ = 2 * System.Math.PI / pdiv;
            var doubleUtils = new DoubleUtils(1e-12);
            var xmax = new double[tdiv + 1];
            var ymax = new double[tdiv + 1];
            var zmax = new double[tdiv + 1];
            Parallel.For(0, tdiv + 1, it => {
                for (var ip = 0; ip <= pdiv; ++ip) {
                    var point = GetSurfacePoint(it * dθ, ip * dφ);
                    if (doubleUtils.Less(xmax[it], System.Math.Abs(point.X))) {
                        xmax[it] = System.Math.Abs(point.X);
                    }
                    if (doubleUtils.Less(ymax[it], System.Math.Abs(point.Y))) {
                        ymax[it] = System.Math.Abs(point.Y);
                    }
                    if (doubleUtils.Less(zmax[it], System.Math.Abs(point.Z))) {
                        zmax[it] = System.Math.Abs(point.Z);
                    }
                }
            });
            _xmax = 0;
            _ymax = 0;
            _zmax = 0;
            for (var it = 0; it <= tdiv; ++it) {
                _xmax = System.Math.Max(_xmax, xmax[it]);
                _ymax = System.Math.Max(_ymax, ymax[it]);
                _zmax = System.Math.Max(_zmax, zmax[it]);
            }
        }

        private static Point3D ToPoint3D(double[] coordinates) {
            return new Point3D(coordinates[0], coordinates[1], coordinates[2]);
        }

        #endregion
    }
}
