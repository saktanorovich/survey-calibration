using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Media3D;
using Core.Calibration;
using Core.Math.Figures;
using HelixToolkit.Wpf;

namespace Clients.CalibrationModule.Controls {
    public partial class SurveySpaceReview {
        #region Fields

        public static readonly DependencyProperty DescriptorProperty;

        private Visual3D _ellipsoidVisual;
        private Visual3D _pointsVisual;
        private double _maxCoord;

        #endregion

        #region Properties

        public SurveyDescriptor Descriptor {
            get { return (SurveyDescriptor)GetValue(DescriptorProperty); }
            set { SetValue(DescriptorProperty, value); }
        }

        #endregion

        #region Ctors

        static SurveySpaceReview() {
            DescriptorProperty = DependencyProperty.Register("Descriptor",
                typeof (SurveyDescriptor), typeof (SurveySpaceReview),
                    new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnDescriptorPropertyChanged));
        }

        public SurveySpaceReview() {
            InitializeComponent();
        }

        #endregion

        #region DescriptorProperty Stuff

        private static void OnDescriptorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var control = (sender as SurveySpaceReview);
            if (control != null) {
                control.OnDescriptorPropertyChanged(e);
            }
        }

        private void OnDescriptorPropertyChanged(DependencyPropertyChangedEventArgs e) {
            var oldDescriptor = e.OldValue as SurveyDescriptor;
            var newDescriptor = e.NewValue as SurveyDescriptor;
            if (oldDescriptor != null) {
                RemoveEllipsoid();
            }
            if (newDescriptor != null) {
                var ellipsoid = newDescriptor.FittingEllipsoid;
                _maxCoord = double.MinValue;
                _maxCoord = Math.Max(_maxCoord, Math.Abs(ellipsoid.Xmax));
                _maxCoord = Math.Max(_maxCoord, Math.Abs(ellipsoid.Ymax));
                _maxCoord = Math.Max(_maxCoord, Math.Abs(ellipsoid.Zmax));
                _maxCoord = Math.Max(_maxCoord, GetMaxCoord(Source));
                AddEllipsoid(ellipsoid);
                AddPoints(Source);
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnSourcePropertyChanged(DependencyPropertyChangedEventArgs e) {
            var newSet = e.NewValue as IList<SurveyPoint>;
            if (newSet != null) {
                _maxCoord = GetMaxCoord(newSet);
                if (Descriptor != null) {
                    var ellipsoid = Descriptor.FittingEllipsoid;
                    _maxCoord = Math.Max(_maxCoord, Math.Abs(ellipsoid.Xmax));
                    _maxCoord = Math.Max(_maxCoord, Math.Abs(ellipsoid.Ymax));
                    _maxCoord = Math.Max(_maxCoord, Math.Abs(ellipsoid.Zmax));
                    RemoveEllipsoid();
                    AddEllipsoid(ellipsoid);
                }
                AddPoints(newSet);
            }
        }

        protected override HelixViewport3D GetViewPortImpl() {
            return ViewPort;
        }

        #endregion

        #region Private Methods

        private void AddPoints(IEnumerable<SurveyPoint> surveyPoints) {
            ViewPort.Children.Remove(_pointsVisual);
            var meshBuilder = new MeshBuilder();
            foreach (var current in surveyPoints) {
                var point = new Point3D {
                    X = current.CalibrationPoint.X * ScaleFactor / _maxCoord,
                    Y = current.CalibrationPoint.Y * ScaleFactor / _maxCoord,
                    Z = current.CalibrationPoint.Z * ScaleFactor / _maxCoord
                };
                meshBuilder.AddSphere(point, PointRadius);
            }
            var modelVisual = new ModelVisual3D {
                Content = new GeometryModel3D {
                    Geometry = meshBuilder.ToMesh(),
                    Material = PointVisual3DMaterial.BlackMaterial
                }
            };
            ViewPort.Children.Add(modelVisual);
            _pointsVisual = modelVisual;
        }

        private void AddEllipsoid(Ellipsoid ellipsoid) {
            if (ellipsoid != null) {
                var ellipsoidVisual = new EllipsoidVisual3D(ellipsoid, PointRadius, ScaleFactor / _maxCoord);
                ViewPort.Children.Add(ellipsoidVisual);
                _ellipsoidVisual = ellipsoidVisual;
            }
        }

        private void RemoveEllipsoid() {
            if (_ellipsoidVisual != null) {
                ViewPort.Children.Remove(_ellipsoidVisual);
            }
        }

        private static double GetMaxCoord(IEnumerable<SurveyPoint> points) {
            var result = double.MinValue;
            if (points != null) {
                foreach (var point in points) {
                    var calibrationPoint = point.CalibrationPoint;
                    result = Math.Max(result, Math.Abs(calibrationPoint.X));
                    result = Math.Max(result, Math.Abs(calibrationPoint.Y));
                    result = Math.Max(result, Math.Abs(calibrationPoint.Z));
                }
            }
            return result;
        }

        #endregion
    }
}
