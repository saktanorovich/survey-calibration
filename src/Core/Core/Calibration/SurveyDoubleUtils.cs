using Core.Math;

namespace Core.Calibration {
    public static class SurveyDoubleUtils {
        public static readonly double Tolerance = 1e-4;
        public static readonly double Eps = 1e-4;
        public static readonly int RoundingDigits = 4;

        private static readonly DoubleUtils DoubleUtils = new DoubleUtils(Eps, RoundingDigits);

        public static int Sign(double x) {
            return DoubleUtils.Sign(x);
        }
    }
}
