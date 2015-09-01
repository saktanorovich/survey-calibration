using System;

namespace Core.Math {
    public sealed class DoubleUtils {
        #region Fields

        private readonly double _eps;
        private readonly int _roundingDigits;

        #endregion

        #region Ctors

        public DoubleUtils(double eps)
            : this(eps, 8) {
        }

        public DoubleUtils(double eps, int roundingDigits) {
            _eps = eps;
            _roundingDigits = roundingDigits;
        }

        #endregion

        #region Public Methods

        public int Sign(double x) {
            if (x + _eps < 0) {
                return -1;
            }
            if (x - _eps > 0) {
                return +1;
            }
            return 0;
        }

        public double Abs(double x) {
            if (Sign(x) < 0) {
                return -x;
            }
            return +x;
        }

        public double Max(double a, double b) {
            if (Sign(a - b) > 0) {
                return a;
            }
            return b;
        }

        public double Min(double a, double b) {
            if (Sign(a - b) < 0) {
                return a;
            }
            return b;
        }

        public bool Less(double a, double b) {
            return Sign(a - b) < 0;
        }

        public bool More(double a, double b) {
            return Sign(a - b) > 0;
        }

        public bool Equals(double a, double b) {
            return Sign(a - b) == 0;
        }

        public bool NotEquals(double a, double b) {
            return Sign(a - b) != 0;
        }

        public double Round(double x) {
            return System.Math.Round(x, _roundingDigits, MidpointRounding.AwayFromZero);
        }

        #endregion
    }
}
