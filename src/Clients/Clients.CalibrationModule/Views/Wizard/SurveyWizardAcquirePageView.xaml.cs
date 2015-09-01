using System.Windows;
using System.Windows.Controls;
using Clients.CalibrationModule.Controls;

namespace Clients.CalibrationModule.Views.Wizard {
    public partial class SurveyWizardAcquirePageView {
        public SurveyWizardAcquirePageView() {
            InitializeComponent();
        }

        private void OnUserControlLoaded(object sender, RoutedEventArgs r) {
            var scrollViewer = PointsDataGrid.Search<ScrollViewer>();
            if (scrollViewer != null) {
                scrollViewer.ScrollChanged += (s, e) => {
                    if (e.ExtentHeightChange > 0.0) {
                        if ((e.VerticalOffset + e.ViewportHeight) + 1.0 > (e.ExtentHeight - e.ExtentHeightChange)) {
                            ((ScrollViewer)s).ScrollToEnd();
                        }
                    }
                };
            }
        }
    }
}
