using System;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Accord.Math;
using Core.Math.Figures;
using Core.Math.Figures.Forms;

namespace Core.Math.Fitting {
    public sealed class AlgebraicEllipsoidFittingAlgorithm : IEllipsoidFittingAlgoritm {
        #region Fit Methods

        public Ellipsoid Fit(Point3D[] points) {
            Requires.NotNull(points, "points");
            if (points.Length < 9) {
                return null;
            }
            try {
                return FitImpl(points);
            }
            catch (Exception) {
                return null;
            }
        }

        private static Ellipsoid FitImpl(Point3D[] points) {
            var design = GetDesignMatrix(points);
            var conic = GetInverseScatterMatrix(design).Multiply(GetSummedDesign(design));

            var A = conic[0];
            var B = conic[1];
            var C = conic[2];
            var D = conic[3];
            var E = conic[4];
            var F = conic[5];
            var G = conic[6];
            var H = conic[7];
            var I = conic[8];
            var J = -1;

            return new Ellipsoid(new EllipsoidQuadraticForm(A, B, C, D, E, F, G, H, I, J));
        }

        #endregion

        #region Private Methods

        private static double[] GetSummedDesign(double[,] design) {
            var identity = new double[design.GetLength(0)];
            Parallel.For(0, identity.Length, i => {
                identity[i] = 1.0;
            });
            return design.Transpose().Multiply(identity);
        }

        private static double[,] GetDesignMatrix(Point3D[] points) {
            var result = new double[points.Length, 9];
            Parallel.For(0, points.Length, i => {
                var x = points[i].X;
                var y = points[i].Y;
                var z = points[i].Z;
                result[i, 0] = x * x;
                result[i, 1] = y * y;
                result[i, 2] = z * z;
                result[i, 3] = 2 * x * y;
                result[i, 4] = 2 * x * z;
                result[i, 5] = 2 * y * z;
                result[i, 6] = 2 * x;
                result[i, 7] = 2 * y;
                result[i, 8] = 2 * z;
            });
            return result;
        }

        private static double[,] GetInverseScatterMatrix(double[,] design) {
            return ((design.Transpose()).Multiply(design)).Inverse();
        }

        #endregion
    }
}
