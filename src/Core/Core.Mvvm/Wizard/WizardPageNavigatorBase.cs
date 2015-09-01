using System;

namespace Core.Mvvm {
    public class WizardPageNavigatorBase : IWizardPageNavigator {
        public void NavigatePrevPage(WizardBase wizard) {
            if (wizard.CurrentPage.IsFirstPage) {
                throw new InvalidOperationException();
            }
            var currentPageIndex = wizard.Pages.IndexOf(wizard.CurrentPage);
            wizard.CurrentPage = wizard.Pages[currentPageIndex - 1];
        }

        public void NavigateNextPage(WizardBase wizard) {
            if (wizard.CurrentPage.IsLastPage) {
                throw new InvalidOperationException();
            }
            var currentPageIndex = wizard.Pages.IndexOf(wizard.CurrentPage);
            wizard.CurrentPage = wizard.Pages[currentPageIndex + 1];
        }
    }
}
