using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;
using Core.Calibration;
using HelixToolkit.Wpf;

namespace Clients.CalibrationModule.Controls {
    public partial class SurveySpace {
        #region Fields

        private readonly Dictionary<SurveyPoint, PointVisual3D> _points = new Dictionary<SurveyPoint, PointVisual3D>();
        private double _maxCoord = double.MinValue;

        #endregion

        #region Ctors

        public SurveySpace() {
            InitializeComponent();
        }

        #endregion

        #region SourceProperty Stuff

        protected override void OnSourcePropertyChanged(DependencyPropertyChangedEventArgs e) {
            var oldSet = e.OldValue as IList<SurveyPoint>;
            var newSet = e.NewValue as IList<SurveyPoint>;
            if (oldSet != null) {
                var observable = oldSet as ObservableCollection<SurveyPoint>;
                if (observable != null) {
                    observable.CollectionChanged -= OnSourceCollectionChanged;
                }
                Remove(oldSet);
            }
            if (newSet != null) {
                var observable = newSet as ObservableCollection<SurveyPoint>;
                if (observable != null) {
                    observable.CollectionChanged += OnSourceCollectionChanged;
                }
                Add(newSet);
            }
        }

        private void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    Add(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Remove(e.OldItems);
                    break;
            }
        }

        #endregion

        #region Protected/Private Methods

        protected override HelixViewport3D GetViewPortImpl() {
            return ViewPort;
        }

        private void Add(IEnumerable surveyPoints) {
            foreach (SurveyPoint surveyPoint in surveyPoints) {
                if (_points.ContainsKey(surveyPoint)) {
                    return;
                }
                var calibrationPoint = surveyPoint.CalibrationPoint;
                var currMaxCoord = double.MinValue;
                currMaxCoord = Math.Max(currMaxCoord, Math.Abs(calibrationPoint.X));
                currMaxCoord = Math.Max(currMaxCoord, Math.Abs(calibrationPoint.Y));
                currMaxCoord = Math.Max(currMaxCoord, Math.Abs(calibrationPoint.Z));
                if (currMaxCoord > _maxCoord) {
                    _maxCoord = currMaxCoord;
                    foreach (var entry in _points.ToArray()) {
                        ViewPort.Children.Remove(entry.Value);
                        Add(entry.Key);
                    }
                }
                Add(surveyPoint);
            }
        }

        private void Remove(IEnumerable surveyPoints) {
            foreach (SurveyPoint surveyPoint in surveyPoints) {
                PointVisual3D visualPoint;
                if (_points.TryGetValue(surveyPoint, out visualPoint)) {
                    ViewPort.Children.Remove(visualPoint);
                    _points.Remove(surveyPoint);
                }
            }
        }

        private void Add(SurveyPoint surveyPoint) {
            var point = surveyPoint.CalibrationPoint;
            var x = point.X * ScaleFactor / _maxCoord;
            var y = point.Y * ScaleFactor / _maxCoord;
            var z = point.Z * ScaleFactor / _maxCoord;
            var visualPoint = new PointVisual3D(new Point3D(x, y, z), PointRadius);
            ViewPort.Children.Add(visualPoint);
            _points[surveyPoint] = visualPoint;
        }

        #endregion
    }
}
