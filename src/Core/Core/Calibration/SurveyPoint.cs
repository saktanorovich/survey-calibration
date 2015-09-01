using System;
using System.Windows.Media.Media3D;

namespace Core.Calibration {
    public sealed class SurveyPoint : IEquatable<SurveyPoint> {
        #region Fields

        private static readonly string Format = string.Format("F{0}", SurveyDoubleUtils.RoundingDigits);

        #endregion

        #region Properties

        public double Gx { get; private set; }
        public double Gy { get; private set; }
        public double Gz { get; private set; }
        public double Mx { get; private set; }
        public double My { get; private set; }
        public double Mz { get; private set; }

        public Point3D CalibrationPoint { get; private set; }
        public double Azimuth { get; private set; }
        public double Inclination { get; private set; }
        public double Roll { get; private set; }
        public int Seqno { get; set; }

        #endregion

        #region Ctors

        public SurveyPoint(double gx, double gy, double gz, double mx, double my, double mz) {
            Gx = gx;
            Gy = gy;
            Gz = gz;
            Mx = mx;
            My = my;
            Mz = mz;
            CalibrationPoint = new Point3D(mx, my, mz);

            Azimuth = System.Math.Atan2(System.Math.Sqrt(Gx * Gx + Gy * Gy + Gz * Gz) * (Mx * Gy - My * Gx), Mz * (Gx * Gx + Gy * Gy) - Gz * (My * Gy + Mx * Gx));
            Inclination = System.Math.Atan2(System.Math.Sqrt(Gx * Gx + Gy * Gy), Gz);
            Roll = System.Math.Atan2(Gy, Gx);

            Azimuth = NormalizeAngle(Azimuth);
            Inclination = NormalizeAngle(Inclination);
            Roll = NormalizeAngle(Roll);
        }

        #endregion

        #region Object Members

        public override int GetHashCode() {
            return Gx.GetHashCode() ^ Gy.GetHashCode() ^ Gz.GetHashCode() ^
                   Mx.GetHashCode() ^ My.GetHashCode() ^ Mz.GetHashCode();
        }

        public override bool Equals(object obj) {
            var other = obj as SurveyPoint;
            if (other != null) {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(SurveyPoint other) {
            if (other != null) {
                return SurveyDoubleUtils.Equals(Mx, other.Mx) &&
                       SurveyDoubleUtils.Equals(My, other.My) &&
                       SurveyDoubleUtils.Equals(Mz, other.Mz);
            }
            return false;
        }

        public override string ToString() {
            return string.Format("G:({0}, {1}, {2}) M:({3}, {4}, {5})",
                Gx.ToString(Format),
                Gy.ToString(Format),
                Gz.ToString(Format),
                Mx.ToString(Format),
                My.ToString(Format),
                Mz.ToString(Format));
        }

        #endregion

        #region Private Methods

        private static double NormalizeAngle(double angel) {
            if (SurveyDoubleUtils.Sign(angel) < 0) {
                angel += 2 * System.Math.PI;
            }
            return angel * 180 / System.Math.PI;
        }

        #endregion
    }
}
