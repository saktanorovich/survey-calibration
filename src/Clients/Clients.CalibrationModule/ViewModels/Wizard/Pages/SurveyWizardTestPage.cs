using System;
using System.Collections.ObjectModel;
using Core.Calibration;

namespace Clients.CalibrationModule.ViewModels.Wizard {
    public sealed class SurveyWizardTestPage : SurveyWizardPage {
        #region Fields

        private int _totalPointsCount;

        #endregion

        #region Properties

        public ObservableCollection<SurveyPointPair> Source { get; private set; }
        public TestColumnCollection Columns { get; private set; }

        #endregion

        #region Ctors

        public SurveyWizardTestPage(SurveyWizard surveyWizard)
            : base(surveyWizard)
        {
            IsLastPage = true;
            ShowPrevButton = true;
            ShowFinishButton = true;
            ShowCancelButton = true;
            FinishButtonText = "Apply";
            Source = new ObservableCollection<SurveyPointPair>();
            Columns = new TestColumnCollection();
        }

        #endregion

        #region Public Methods

        protected override void ActivateImpl() {
            _totalPointsCount = 0;
            Source.Clear();
            SurveyWizard.SurveyPointReceived -= OnSurveyWizardSurveyPointReceived;
            SurveyWizard.SurveyPointReceived += OnSurveyWizardSurveyPointReceived;
            SurveyWizard.StartReceivingPoints(false, false);
        }

        protected override void DeactivateImpl() {
            SurveyWizard.StopReceivingPoints();
            SurveyWizard.SurveyPointReceived -= OnSurveyWizardSurveyPointReceived;
            Source.Clear();
        }

        #endregion

        #region Protected Methods

        protected override void OnDispose() {
            SurveyWizard.StopReceivingPoints();
            if (Source != null) {
                Source.Clear();
                Source = null;
            }
            base.OnDispose();
        }

        #endregion

        #region Private Methods

        private void OnSurveyWizardSurveyPointReceived(object sender, EventArgs e) {
            ++_totalPointsCount;
            FirstStatusMessage = "Total Points Count: " + _totalPointsCount;
            var source = SurveyWizard.LastSurveyPoint;
            var output = SurveyWizard.Calibrate().Transformer.Transform(source);
            Source.Add(new SurveyPointPair(_totalPointsCount, source, output));
        }

        #endregion
    }
}
