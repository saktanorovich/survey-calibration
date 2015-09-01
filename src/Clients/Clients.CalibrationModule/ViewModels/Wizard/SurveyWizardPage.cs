using Core;
using Core.Mvvm;

namespace Clients.CalibrationModule.ViewModels.Wizard {
    public abstract class SurveyWizardPage : WizardBasePage, ISurveyWizardPage {
        #region Fields

        protected readonly SurveyWizard SurveyWizard;
        private string _firstStatusMessage;
        private string _secondStatusMessage;

        #endregion

        #region Properties

        public string FirstStatusMessage {
            get { return _firstStatusMessage; }
            protected set {
                _firstStatusMessage = value;
                OnPropertyChanged("FirstStatusMessage");
            }
        }

        public string SecondStatusMessage {
            get { return _secondStatusMessage; }
            protected set {
                _secondStatusMessage = value;
                OnPropertyChanged("SecondStatusMessage");
            }
        }

        #endregion

        #region Ctors

        protected SurveyWizardPage(SurveyWizard surveyWizard) {
            Requires.NotNull(surveyWizard, "surveyWizard");
            SurveyWizard = surveyWizard;
            DisplayName = "Survey Calibration Wizard";
        }

        #endregion

        #region Public Methods

        public void Activate() {
            ActivateImpl();
        }

        public void Deactivate() {
            DeactivateImpl();
        }

        public void Loaded() {
            LoadedImpl();
        }

        #endregion

        #region Protected Methods

        protected override void OnActivated() {
            Activate();
            base.OnActivated();
        }

        protected override void OnIsCurrentPageChanged() {
            if (!IsCurrentPage) {
                Deactivate();
            }
            base.OnIsCurrentPageChanged();
        }

        protected virtual void ActivateImpl() { }
        protected virtual void DeactivateImpl() { }
        protected virtual void LoadedImpl() { }

        #endregion
    }
}
