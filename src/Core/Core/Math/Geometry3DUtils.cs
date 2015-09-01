using System.Windows.Media.Media3D;

namespace Core.Math {
    public static class Geometry3DUtils {
        #region Public Methods

        public static double EuclidianDistance(Point3D a, Point3D b) {
            return System.Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y) + (a.Z - b.Z) * (a.Z - b.Z));
        }

        #endregion

        #region 3D Rotation Stuff

        public static double[,] Get3DxRotationMatrix(double cosα, double sinα) {
            return new[,] {
                {1,   0  ,   0  },
                {0, +cosα, +sinα},
                {0, -sinα, +cosα},
            };
        }

        public static double[,] Get3DyRotationMatrix(double cosα, double sinα) {
            return new[,] {
                {+cosα, 0, -sinα},
                {  0  , 1,   0  },
                {+sinα, 0, +cosα},
            };
        }

        public static double[,] Get3DzRotationMatrix(double cosα, double sinα) {
            return new[,] {
                {+cosα, +sinα, 0},
                {-sinα, +cosα, 0},
                {  0  ,   0 ,  1},
            };
        }

        #endregion
    }
}
