using System;

namespace Clients.CalibrationModule.ViewModels.Wizard {
    public interface ISurveyWizardPage : IDisposable
    {
        string FirstStatusMessage { get; }
        string SecondStatusMessage { get; }

        void Activate();
        void Deactivate();
        void Loaded();
    }
}
