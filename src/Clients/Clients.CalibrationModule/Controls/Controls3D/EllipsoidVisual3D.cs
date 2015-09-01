using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Core;
using Core.Math.Figures;
using HelixToolkit.Wpf;

namespace Clients.CalibrationModule.Controls {
    public class EllipsoidVisual3D : ModelVisual3D {
        #region Const

        private const int DencityFactor = 2;
        private const int GraticuleSpacingθ = 12 * DencityFactor;
        private const int GraticuleSpacingφ = 24 * DencityFactor;
        private const double Dθ = 1 * Math.PI / GraticuleSpacingθ;
        private const double Dφ = 2 * Math.PI / GraticuleSpacingφ;

        #endregion

        #region Ctors

        internal EllipsoidVisual3D(Ellipsoid ellipsoid, double pointRadius, double scaleFactor) {
            Requires.NotNull(ellipsoid, "ellipsoid");
            /* Generates ellipsoid using spherical coordinates (r, θ, φ) where 0 ≤ θ ≤ π, 0 ≤ φ ≤ 2π. */
            var points = new Point3D[GraticuleSpacingθ + 1, GraticuleSpacingφ + 1];
            for (var it = 0; it <= GraticuleSpacingθ; ++it) {
                for (var ip = 0; ip <= GraticuleSpacingφ; ++ip) {
                    points[it, ip] = ellipsoid.GetSurfacePoint(it * Dθ, ip * Dφ);
                    points[it, ip].X *= scaleFactor;
                    points[it, ip].Y *= scaleFactor;
                    points[it, ip].Z *= scaleFactor;
                }
            }
            BuildLatitudeLines(points, GraticuleSpacingθ + 1, GraticuleSpacingφ + 1);
            BuildLongitudeLines(points, GraticuleSpacingθ + 1, GraticuleSpacingφ + 1);
            BuildPoint(ellipsoid.Center, pointRadius, scaleFactor);
            foreach (var pole in ellipsoid.Poles) {
                BuildPoint(pole, pointRadius, scaleFactor);
            }
        }

        #endregion

        #region Private Methods

        /* Latitude Line - θ is const. */
        private void BuildLatitudeLines(Point3D[,] points, int rows, int cols) {
            for (var θ = 0; θ < rows; θ += DencityFactor) {
                var lines = MakeLinesVisual3D();
                for (var φ = 0; φ < cols; ++φ) {
                    lines.Points.Add(points[θ, (φ + 0) % cols]);
                    lines.Points.Add(points[θ, (φ + 1) % cols]);
                }
                Children.Add(lines);
            }
        }

        /* Longitude Line - φ is const. */
        private void BuildLongitudeLines(Point3D[,] points, int rows, int cols) {
            for (var φ = 0; φ < cols; φ += DencityFactor) {
                var lines = MakeLinesVisual3D();
                for (var θ = 0; θ < rows; ++θ) {
                    lines.Points.Add(points[(θ + 0) % rows, φ]);
                    lines.Points.Add(points[(θ + 1) % rows, φ]);
                }
                Children.Add(lines);
            }
        }

        private void BuildPoint(Point3D center, double radius, double scaleFactor) {
            Children.Add(new SphereVisual3D {
                Radius = radius,
                Material = PointVisual3DMaterial.RedMaterial,
                Center = new Point3D(
                    center.X * scaleFactor,
                    center.Y * scaleFactor,
                    center.Z * scaleFactor)
            });
        }

        private static LinesVisual3D MakeLinesVisual3D() {
            return new LinesVisual3D {
                Color = Colors.Gray,
                Thickness = 0.5
            };
        }

        #endregion
    }
}
