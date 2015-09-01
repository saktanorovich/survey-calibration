using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Core.Math.Fitting;

namespace Core.Calibration {
    public class SurveyCalibrationBuilder {
        #region Public Methods

        public SurveyCalibration Build(SurveyPoint[] surveyPoints) {
            Requires.NotNull(surveyPoints, "surveyPoints");
            return BuildImpl(surveyPoints);
        }

        #endregion

        #region Protected Methods

        protected virtual SurveyCalibration BuildImpl(SurveyPoint[] surveyPoints) {
            TimeSpan elapsedTime;
            var result = Measure(() => {
                var source = surveyPoints;
                var sourceDescriptor = GetDescriptor(source);
                if (sourceDescriptor != null) {
                    var transformer = GetTransformer(sourceDescriptor);
                    if (transformer != null) {
                        var output = transformer.Transform(source);
                        var outputDescriptor = GetDescriptor(output);
                        if (outputDescriptor != null) {
                            return new SurveyCalibration {
                                Source = source,
                                Output = output,
                                SourceDescriptor = sourceDescriptor,
                                OutputDescriptor = outputDescriptor,
                                Transformer = transformer
                            };
                        }
                    }
                }
                return null;
            }, out elapsedTime);
            if (result != null) {
                result.ElapsedTime = elapsedTime;
            }
            return result;
        }

        #endregion

        #region Private Methods

        private static SurveyPointTransformer GetTransformer(SurveyDescriptor descriptor) {
            var ellipsoid = descriptor.FittingEllipsoid;
            var hardIron = new Vector3D(
                ellipsoid.Center.X,
                ellipsoid.Center.Y,
                ellipsoid.Center.Z);
            var softIron = descriptor.FittingEllipsoid.InverseTransform;
            return new SurveyPointTransformer(hardIron, softIron);
        }

        private static SurveyDescriptor GetDescriptor(SurveyPoint[] surveyPoints) {
            var fittingAlgorithm = new AlgebraicEllipsoidFittingAlgorithm();
            var pointsToFit = new Point3D[surveyPoints.Length];
            Parallel.For(0, surveyPoints.Length, i => {
                pointsToFit[i] = surveyPoints[i].CalibrationPoint;
            });
            var fittingEllipsoid = fittingAlgorithm.Fit(pointsToFit);
            if (fittingEllipsoid == null) {
                return null;
            }
            var result = new SurveyDescriptor {
                Radius = new Point3D(
                    fittingEllipsoid.XRadius,
                    fittingEllipsoid.YRadius,
                    fittingEllipsoid.ZRadius),
                Center = fittingEllipsoid.Center,
                Count = surveyPoints.Length,
                Sphericity = fittingEllipsoid.MinRadius / fittingEllipsoid.MaxRadius,
                FittingEllipsoid = fittingEllipsoid
            };
            var totalOnTheSurface = 0;
            for (var i = 0; i < pointsToFit.Length; ++i) {
                if (fittingEllipsoid.Contains(pointsToFit[i], SurveyDoubleUtils.Tolerance)) {
                    ++totalOnTheSurface;
                }
            }
            result.QualityOfFit = 1.0 * totalOnTheSurface / surveyPoints.Length;
            return result;
        }

        private static T Measure<T>(Func<T> func, out TimeSpan time) {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = func();
            stopwatch.Stop();
            time = stopwatch.Elapsed;
            return result;
        }

        #endregion
    }
}
