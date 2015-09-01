using System.Windows.Media.Media3D;
using Accord.Math;
using Accord.Math.Decompositions;

namespace Core.Math.Figures.Forms {
    /**
     * Defines an ellipsoid in the form
     *     Ax^2 + By^2 + Cz^2 + 2Dxy + 2Exz + 2Fyz + 2Gx + 2Hy + 2Iz + J = 0
     * where D^2 < AB, E^2 < AC, F^2 < BC, A,B,C > 0.
     */
    public sealed class EllipsoidQuadraticForm {
        #region Fields

        private readonly double[] _coefficients;

        #endregion

        #region Internal Properties

        internal double A { get { return _coefficients[0]; } }
        internal double B { get { return _coefficients[1]; } }
        internal double C { get { return _coefficients[2]; } }
        internal double D { get { return _coefficients[3]; } }
        internal double E { get { return _coefficients[4]; } }
        internal double F { get { return _coefficients[5]; } }
        internal double G { get { return _coefficients[6]; } }
        internal double H { get { return _coefficients[7]; } }
        internal double I { get { return _coefficients[8]; } }
        internal double J { get { return _coefficients[9]; } }

        internal EllipsoidParametricForm EllipsoidParametricForm { get; private set; }
        internal double[,] AffineTransform { get; private set; }
        internal double[,] QuadraticFrom { get; private set; }
        internal double[] Offset { get; private set; }

        #endregion

        #region Ctors

        public EllipsoidQuadraticForm(double A, double B, double C, double D, double E, double F, double G, double H, double I, double J) {
            var doubleUtils = new DoubleUtils(1e-12);
            if (doubleUtils.Sign(A) < 0) {
                A = -A;
                B = -B;
                C = -C;
                D = -D;
                E = -E;
                F = -F;
                G = -G;
                H = -H;
                I = -I;
                J = -J;
            }
            Requires.Positive(doubleUtils.Sign(A), "A");
            Requires.Positive(doubleUtils.Sign(B), "B");
            Requires.Positive(doubleUtils.Sign(C), "C");
            Requires.Positive(doubleUtils.Sign(A * B - D * D), "A * B - D * D");
            Requires.Positive(doubleUtils.Sign(A * C - E * E), "A * C - E * E");
            Requires.Positive(doubleUtils.Sign(B * C - F * F), "B * C - F * F");
            _coefficients = new[] { A, B, C, D, E, F, G, H, I, J };
            QuadraticFrom = new[,] {
                { A, D, E },
                { D, B, F },
                { E, F, C }
            };
            Offset = new[] { G, H, I, };

            var b = QuadraticFrom.Inverse().Multiply(Offset.Multiply(-1));
            var eigenDecomposition = new EigenvalueDecomposition(QuadraticFrom);
            var axisNumerator = b.ElementwiseMultiply(QuadraticFrom.Multiply(b)).Sum() - J;
            var xradius = System.Math.Sqrt(axisNumerator / eigenDecomposition.RealEigenvalues[0]);
            var yradius = System.Math.Sqrt(axisNumerator / eigenDecomposition.RealEigenvalues[1]);
            var zradius = System.Math.Sqrt(axisNumerator / eigenDecomposition.RealEigenvalues[2]);
            var rotation = eigenDecomposition.Eigenvectors;
            EllipsoidParametricForm = new EllipsoidParametricForm(xradius, yradius, zradius, new Point3D(b[0], b[1], b[2]), rotation);

            var sqrt = rotation.Multiply(eigenDecomposition.DiagonalMatrix.Sqrt().Multiply(rotation.Inverse()));
            AffineTransform = sqrt.Multiply(1.0 / System.Math.Sqrt(axisNumerator));
        }

        #endregion

        #region Internal Methods

        internal double Evaluate(Point3D point) {
            var x = point.X;
            var y = point.Y;
            var z = point.Z;
            var components = new[] {
                x * x, y * y, z * z, 2 * x * y, 2 * x * z, 2 * y * z, 2 * x, 2 * y, 2 * z, +1
            };
            var result = 0.0;
            for (var i = 0; i < 10; ++i) {
                result += _coefficients[i] * components[i];
            }
            return result;
        }

        #endregion
    }
}
