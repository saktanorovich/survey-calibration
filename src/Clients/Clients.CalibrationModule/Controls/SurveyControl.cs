using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Core.Calibration;
using HelixToolkit.Wpf;

namespace Clients.CalibrationModule.Controls {
    public class SurveyControl : UserControl {
        #region Fields

        public static readonly DependencyProperty HeaderProperty;
        public static readonly DependencyProperty ScaleFactorProperty;
        public static readonly DependencyProperty PointRadiusProperty;
        public static readonly DependencyProperty SourceProperty;

        private LinesVisual3D _lines;
        private ArrowVisual3D _xaxis;
        private ArrowVisual3D _yaxis;
        private ArrowVisual3D _zaxis;

        #endregion

        #region Properties

        public string Header {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public int ScaleFactor {
            get { return (int)GetValue(ScaleFactorProperty); }
            set { SetValue(NameProperty, value); }
        }

        public double PointRadius {
            get { return (double)GetValue(PointRadiusProperty); }
            set { SetValue(PointRadiusProperty, value); }
        }

        public IList<SurveyPoint> Source {
            get { return (IList<SurveyPoint>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        #endregion

        #region Ctors

        static SurveyControl() {
            HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(SurveyControl));
            ScaleFactorProperty = DependencyProperty.Register("ScaleFactor", typeof(int), typeof(SurveyControl), new PropertyMetadata(10, OnScaleFactorPropertyChanged));
            PointRadiusProperty = DependencyProperty.Register("PointRadius", typeof(double), typeof(SurveyControl), new PropertyMetadata(1.0));
            SourceProperty = DependencyProperty.Register("Source", typeof(IList<SurveyPoint>), typeof(SurveyControl), new PropertyMetadata(null, OnSourcePropertyChanged));
        }

        #endregion

        #region Source Property

        private static void OnSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var control = (sender as SurveyControl);
            if (control != null) {
                control.OnSourcePropertyChanged(e);
            }
        }

        protected virtual void OnSourcePropertyChanged(DependencyPropertyChangedEventArgs e) {
            throw new NotImplementedException();
        }

        #endregion

        #region ScaleFactor Property

        private static void OnScaleFactorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var control = (sender as SurveyControl);
            if (control != null) {
                control.OnScaleFactorPropertyChanged(e);
            }
        }

        protected virtual void OnScaleFactorPropertyChanged(DependencyPropertyChangedEventArgs e) {
            var oldValue = (int)e.OldValue;
            var newValue = (int)e.NewValue;
            if (oldValue != newValue) {
                if (newValue > 0) {
                    if (_lines != null) {
                        RemoveVisualChildren(_lines, _xaxis, _yaxis, _zaxis);
                    }
                    _lines = new LinesVisual3D {
                        Thickness = 0.5,
                        Color = Colors.Black,
                        Points = new Point3DCollection(new[] {
                            new Point3D(-newValue, +newValue, +newValue),
                            new Point3D(+newValue, +newValue, +newValue),
                            new Point3D(-newValue, +newValue, -newValue),
                            new Point3D(+newValue, +newValue, -newValue),
                            new Point3D(-newValue, -newValue, +newValue),
                            new Point3D(+newValue, -newValue, +newValue),
                            new Point3D(-newValue, -newValue, -newValue),
                            new Point3D(+newValue, -newValue, -newValue),

                            new Point3D(+newValue, -newValue, +newValue),
                            new Point3D(+newValue, +newValue, +newValue),
                            new Point3D(+newValue, -newValue, -newValue),
                            new Point3D(+newValue, +newValue, -newValue),
                            new Point3D(-newValue, -newValue, +newValue),
                            new Point3D(-newValue, +newValue, +newValue),
                            new Point3D(-newValue, -newValue, -newValue),
                            new Point3D(-newValue, +newValue, -newValue),

                            new Point3D(+newValue, +newValue, -newValue),
                            new Point3D(+newValue, +newValue, +newValue),
                            new Point3D(+newValue, -newValue, -newValue),
                            new Point3D(+newValue, -newValue, +newValue),
                            new Point3D(-newValue, +newValue, -newValue),
                            new Point3D(-newValue, +newValue, +newValue),
                            new Point3D(-newValue, -newValue, -newValue),
                            new Point3D(-newValue, -newValue, +newValue),
                        })
                    };
                    _xaxis = MakeArrowVisual(newValue, 0, 0, Brushes.Red);
                    _yaxis = MakeArrowVisual(0, newValue, 0, Brushes.Green);
                    _zaxis = MakeArrowVisual(0, 0, newValue, Brushes.Blue);
                    AddVisualChildren(_lines, _xaxis, _yaxis, _zaxis);
                }
            }
        }

        private static ArrowVisual3D MakeArrowVisual(int dx, int dy, int dz, Brush brush) {
            return new ArrowVisual3D {
                Point1 = new Point3D(-dx, -dy, -dz),
                Point2 = new Point3D(+dx, +dy, +dz),
                Fill = brush,
                Diameter = 0.4,
            };
        }

        #endregion

        #region Protected/Private Methods

        protected virtual HelixViewport3D GetViewPortImpl() {
            throw new NotSupportedException();
        }

        private void AddVisualChildren(params Visual3D[] visuals) {
            var viewPort = GetViewPortImpl();
            foreach (var visual in visuals) {
                viewPort.Children.Add(visual);
            }
        }

        private void RemoveVisualChildren(params Visual3D[] visuals) {
            var viewPort = GetViewPortImpl();
            foreach (var visual in visuals) {
                viewPort.Children.Remove(visual);
            }
        }

        #endregion
    }
}
