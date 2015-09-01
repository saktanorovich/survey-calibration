using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Accord.Math;

namespace Core.Calibration {
    public sealed class SurveyPointTransformer {
        public Vector3D HardIronEffect { get; private set; }
        public double[,] SoftIronEffect { get; private set; }

        public SurveyPointTransformer(Vector3D hardIronEffect, double[,] softIronEffect) {
            HardIronEffect = hardIronEffect;
            SoftIronEffect = softIronEffect;
        }

        public SurveyPoint[] Transform(SurveyPoint[] surveyPoints) {
            var transformed = new SurveyPoint[surveyPoints.Length];
            Parallel.For(0, surveyPoints.Length, i => {
                transformed[i] = Transform(surveyPoints[i]);
            });
            return transformed;
        }

        public SurveyPoint Transform(SurveyPoint surveyPoint) {
            var pt = new[] {
                surveyPoint.CalibrationPoint.X,
                surveyPoint.CalibrationPoint.Y,
                surveyPoint.CalibrationPoint.Z,
            }.Subtract(new[] {
                HardIronEffect.X,
                HardIronEffect.Y,
                HardIronEffect.Z,
            });
            pt = SoftIronEffect.Multiply(pt);
            return new SurveyPoint(surveyPoint.Gx, surveyPoint.Gy, surveyPoint.Gz, pt[0], pt[1], pt[2]);
        }
    }
}
