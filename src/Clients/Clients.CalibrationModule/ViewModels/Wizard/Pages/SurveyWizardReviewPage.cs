using System;
using System.Diagnostics;
using Core.Calibration;

namespace Clients.CalibrationModule.ViewModels.Wizard {
    public sealed class SurveyWizardReviewPage : SurveyWizardPage {
        #region Fields

        private SurveyCalibration _surveyCalibration;
        private Stopwatch _stopwatch;

        #endregion

        #region Ctors

        #region Properties

        public SurveyCalibration SurveyCalibration {
            get { return _surveyCalibration; }
            private set {
                _surveyCalibration = value;
                OnPropertyChanged("SurveyCalibration");
            }
        }

        #endregion

        public SurveyWizardReviewPage(SurveyWizard surveyWizard)
            : base(surveyWizard)
        {
            ShowPrevButton = true;
            ShowNextButton = true;
            ShowCancelButton = true;
            NextButtonText = "Test";
        }

        #endregion

        #region Public Methods

        protected override void ActivateImpl() {
            var surveyCalibration = SurveyWizard.Calibrate();
            if (surveyCalibration != null) {
                if (_stopwatch != null) {
                    _stopwatch.Stop();
                    _stopwatch = null;
                }
                _stopwatch = Stopwatch.StartNew();
                Dispatcher.Invoke(() => {
                    SurveyCalibration = surveyCalibration;
                    FirstStatusMessage = ToString("Elapsed", surveyCalibration.ElapsedTime);
                });
                return;
            }
            SurveyWizard.PrevPageCommand.Execute(null);
        }

        protected override void LoadedImpl() {
            if (_stopwatch != null) {
                _stopwatch.Stop();
                var elapsed = _stopwatch.Elapsed;
                SecondStatusMessage = ToString("Rendering", elapsed);
            }
        }

        #endregion

        #region Private Methods

        private static string ToString(string name, TimeSpan timeSpan) {
            return string.Format("{0} Time: {1:F4} {2}", name, GetValue(timeSpan), GetSuffix(timeSpan));
        }

        private static string GetSuffix(TimeSpan timeSpan) {
            if (timeSpan.TotalMilliseconds > 1000.0) {
                if (timeSpan.TotalSeconds > 60.0) {
                    return "m";
                }
                return "s";
            }
            return "ms";
        }

        private static double GetValue(TimeSpan timeSpan) {
            if (timeSpan.TotalMilliseconds > 1000.0) {
                if (timeSpan.TotalSeconds > 60.0) {
                    return timeSpan.TotalMinutes;
                }
                return timeSpan.TotalSeconds;
            }
            return timeSpan.TotalMilliseconds;
        }

        #endregion
    }
}
