using System;
using System.Collections.Generic;
using Core.Calibration;

namespace Clients.CalibrationModule.ViewModels.Wizard {
    public sealed class SurveyWizardAcquirePage : SurveyWizardPage {
        #region Properties

        public IList<SurveyPoint> SurveySource {
            get {
                return SurveyWizard.SurveyPoints;
            }
        }

        #endregion

        #region Ctors

        public SurveyWizardAcquirePage(SurveyWizard surveyWizard)
            :base(surveyWizard)
        {
            IsFirstPage = true;
            ShowNextButton = true;
            ShowCancelButton = true;
            NextButtonText = "Create";
            SecondStatusMessage = string.Empty;
        }

        #endregion

        #region Public Methods

        protected override void ActivateImpl() {
            SurveyWizard.SurveyPointReceived -= OnSurveyPointReceived;
            SurveyWizard.SurveyPointReceived += OnSurveyPointReceived;
            SurveyWizard.StartReceivingPoints(true, true);
        }

        protected override void DeactivateImpl() {
            SurveyWizard.StopReceivingPoints();
            SurveyWizard.SurveyPointReceived -= OnSurveyPointReceived;
        }

        #endregion

        #region Protected Methods

        protected override void OnDispose() {
            SurveyWizard.StopReceivingPoints();
            base.OnDispose();
        }

        #endregion

        #region Private Methods

        private void OnSurveyPointReceived(object sender, EventArgs e) {
            FirstStatusMessage = "Total Points Count: " + SurveyWizard.TotalPointsCount;
        }

        #endregion
    }
}
