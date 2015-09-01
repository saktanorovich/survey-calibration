namespace Core.Mvvm {
    public interface IWizardPageNavigator
    {
        void NavigatePrevPage(WizardBase wizard);
        void NavigateNextPage(WizardBase wizard);
    }
}
