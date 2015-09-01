using System.Windows;
using Clients.CalibrationModule.ViewModels.Wizard;

namespace Clients.CalibrationModule.Views.Wizard {
    public partial class SurveyWizardReviewPageView {
        public SurveyWizardReviewPageView() {
            InitializeComponent();
        }

        private void OnUserControlLoaded(object sender, RoutedEventArgs e) {
            var dataContext = DataContext as ISurveyWizardPage;
            if (dataContext != null) {
                dataContext.Loaded();
            }
        }
    }
}
