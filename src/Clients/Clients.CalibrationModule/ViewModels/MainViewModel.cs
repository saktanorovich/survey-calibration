using System.Windows;
using System.Windows.Input;
using Clients.CalibrationModule.ViewModels.Wizard;
using Clients.CalibrationModule.Windows.Wizard;
using Core;
using Core.Calibration;
using Core.Mvvm;
using Core.Mvvm.Commands;

namespace Clients.CalibrationModule.ViewModels {
    public class MainViewModel : ClosableViewModel {
        private ICommand _createCommmand;

        public ICommand CreateCommand {
            get {
                return _createCommmand ?? (_createCommmand = new SimpleCommand(OnCreateCommand));
            }
        }

        private static void OnCreateCommand() {
            var surveyPointProvider = new SurveyPointProviderRandom();
            ShowWindow(new WizardWindow {
                DataContext = new SurveyWizard(surveyPointProvider),
                Owner = Application.Current.MainWindow
            });
        }

        private static void ShowWindow(Window window) {
            var dataContext = window.DataContext as WizardBase;
            var lifetime = new Lifetime(() => {
                if (dataContext != null) {
                    dataContext.Dispose();
                }
                if (window.Owner != null) {
                    window.Owner.Focus();
                }
                window.Close();
            });
            window.Closed += (s, e) => lifetime.Dispose();
            if (dataContext != null) {
                dataContext.RequestCancel += (s, e) => lifetime.Dispose();
                dataContext.RequestFinish += (s, e) => lifetime.Dispose();
            }
            window.Show();
        }
    }
}
