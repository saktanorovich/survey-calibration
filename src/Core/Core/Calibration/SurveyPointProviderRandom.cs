using System;
using Accord.Math;
using Core.Math;

namespace Core.Calibration {
    public class SurveyPointProviderRandom : SurveyPointProviderBase {
        #region Fields

        private readonly Random _rand;
        private readonly int _xradius;
        private readonly int _yradius;
        private readonly int _zradius;
        private readonly int _xcenter;
        private readonly int _ycenter;
        private readonly int _zcenter;
        private readonly double _xcos, _xsin;
        private readonly double _ycos, _ysin;
        private readonly double _zcos, _zsin;

        #endregion

        #region Ctors

        public SurveyPointProviderRandom() {
            _rand = new Random(Environment.TickCount);
            _xradius = System.Math.Max(50, _rand.Next(500));
            _yradius = System.Math.Max(50, _rand.Next(500));
            _zradius = System.Math.Max(50, _rand.Next(500));
            _xcenter = _rand.Next(50);
            _ycenter = _rand.Next(50);
            _zcenter = _rand.Next(50);
            var xangel = _rand.Next(50) * System.Math.PI / (_rand.Next(100) + 0.01);
            var yangel = _rand.Next(50) * System.Math.PI / (_rand.Next(100) + 0.01);
            var zangel = _rand.Next(50) * System.Math.PI / (_rand.Next(100) + 0.01);
            _xcos = System.Math.Cos(xangel);
            _xsin = System.Math.Sin(xangel);
            _ycos = System.Math.Cos(yangel);
            _ysin = System.Math.Sin(yangel);
            _zcos = System.Math.Cos(zangel);
            _zsin = System.Math.Sin(zangel);
        }

        #endregion

        #region Protected Methods

        protected override bool HasSurveyPointImpl() {
            return true;
        }

        protected override SurveyPoint GetSurveyPointImpl()
        {
            var r3Dx = Geometry3DUtils.Get3DxRotationMatrix(_xcos, _xsin);
            var r3Dy = Geometry3DUtils.Get3DyRotationMatrix(_ycos, _ysin);
            var r3Dz = Geometry3DUtils.Get3DzRotationMatrix(_zcos, _zsin);
            var rotation = r3Dx.Multiply(r3Dy.Multiply(r3Dz));

            var tet = (1 * System.Math.PI / 100) * _rand.Next(100 + 1);
            var phi = (2 * System.Math.PI / 100) * _rand.Next(100 + 1);
            var pt = rotation.Multiply(new[] {
                _xradius * System.Math.Sin(tet) * System.Math.Cos(phi) + _xcenter,
                _yradius * System.Math.Sin(tet) * System.Math.Sin(phi) + _ycenter,
                _zradius * System.Math.Cos(tet) + _zcenter
            });
            return new SurveyPoint(0, 0, 1, pt[0], pt[1], pt[2]);
        }

        #endregion
    }
}
