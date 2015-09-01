using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using Core;
using Core.Calibration;
using Core.Math;
using Core.Mvvm;

namespace Clients.CalibrationModule.ViewModels.Wizard {
    public sealed class SurveyWizard : WizardBase {
        #region Fields

        private ISurveyPointProvider _surveyPointProvider;
        private DispatcherTimer _timer;
        private SurveyCalibration _surveyCalibration;
        private bool _storePoints;
        private bool _ignoreNeigbours = true;
        private bool _isDisposed;

        #endregion

        #region Ctors

        public SurveyWizard(ISurveyPointProvider surveyPointProvider) {
            Requires.NotNull(surveyPointProvider, "surveyPointProvider");
            _surveyPointProvider = surveyPointProvider;
            SurveyPoints = new ObservableCollection<SurveyPoint>();
            LastSurveyPoint = new SurveyPoint(
                  double.MaxValue / 2,
                  double.MaxValue / 2,
                  double.MaxValue / 2,
                  double.MaxValue / 2,
                  double.MaxValue / 2,
                  double.MaxValue / 2);
            _timer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _timer.Tick += OnTimerTick;
        }

        #endregion

        #region Events

        public event EventHandler SurveyPointReceived;

        #endregion

        #region Properties

        public ObservableCollection<SurveyPoint> SurveyPoints { get; private set; }
        public SurveyPoint LastSurveyPoint { get; private set; }
        public int TotalPointsCount { get; private set; }

        #endregion

        #region Public Methods

        public void StartReceivingPoints(bool storePoints, bool ignoreNeigbours) {
            _surveyCalibration = null;
            _storePoints = storePoints;
            _ignoreNeigbours = ignoreNeigbours;
            _timer.Start();
        }

        public void StopReceivingPoints() {
            if (_timer != null) {
                _timer.Stop();
            }
        }

        public SurveyCalibration Calibrate() {
            if (ReferenceEquals(_surveyCalibration, null)) {
                var surveyBuilder = new SurveyCalibrationBuilder();
                var result = surveyBuilder.Build(SurveyPoints.ToArray());
                _surveyCalibration = result;
            }
            return _surveyCalibration;
        }

        #endregion

        #region Protected Methods

        protected override IList<WizardBasePage> GetPagesImpl() {
            return new List<WizardBasePage> {
                new SurveyWizardAcquirePage(this),
                new SurveyWizardReviewPage(this),
                new SurveyWizardTestPage(this),
            };
        }

        protected override bool CanFinish() {
            return false;
        }

        protected override void OnDispose() {
            if (_isDisposed) {
                return;
            }
            _isDisposed = true;
            if (_timer != null) {
                _timer.Stop();
                _timer = null;
            }
            if (SurveyPoints != null) {
                SurveyPoints = null;
            }
            foreach (var page in Pages) {
                page.Dispose();
            }
            if (_surveyPointProvider != null) {
                _surveyPointProvider.Dispose();
                _surveyPointProvider = null;
            }
            base.OnDispose();
        }

        #endregion

        #region Private Methods

        private void OnTimerTick(object sender, EventArgs e) {
            if (_surveyPointProvider.HasSurveyPoint()) {
                var surveyPoint = GetSurveyPoint();
                if (surveyPoint == null) {
                    return;
                }
                if (_storePoints) {
                    ++TotalPointsCount;
                    surveyPoint.Seqno = TotalPointsCount;
                    SurveyPoints.Add(surveyPoint);
                }
                OnSurveyPointReceived();
            }
        }

        private void OnSurveyPointReceived() {
            var handler = Interlocked.CompareExchange(ref SurveyPointReceived, null, null);
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }

        private SurveyPoint GetSurveyPoint() {
            var currSurveyPoint = _surveyPointProvider.GetSurveyPoint();
            if (currSurveyPoint == null) {
                return null;
            }
            if (_ignoreNeigbours) {
                var distance = Geometry3DUtils.EuclidianDistance(currSurveyPoint.CalibrationPoint, LastSurveyPoint.CalibrationPoint);
                if (distance < SurveyDoubleUtils.Tolerance) {
                    return null;
                }
            }
            LastSurveyPoint = currSurveyPoint;
            return LastSurveyPoint;
        }

        #endregion
    }
}
