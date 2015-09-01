using System.Windows.Media.Media3D;
using Core.Math.Figures;

namespace Core.Math.Fitting {
    public sealed class GeometricEllipsoidFittingAlgorithm : IEllipsoidFittingAlgoritm {
        #region Fit Methods

        public Ellipsoid Fit(Point3D[] points) {
            return FitImpl(points);
        }

        private static Ellipsoid FitImpl(Point3D[] points) {
            var algebraicEllipsoidFittingAlgorithm = new AlgebraicEllipsoidFittingAlgorithm();
            var feasibleSolution = algebraicEllipsoidFittingAlgorithm.Fit(points);
            if (feasibleSolution != null) {
                //todo: add Gauss-Newton or Levenberg–Marquardt algorithms for proper fitting.
                var solution = GetNonlinearForm(feasibleSolution);
                for (var iterationsCount = 0; iterationsCount < 10; ++iterationsCount) {

                }
                return feasibleSolution;
            }
            return null;
        }

        #endregion

        #region Private Methods

        private static object GetNonlinearForm(Ellipsoid feasibleSolution) {
            return new object();
        }

        #endregion
    }
}
