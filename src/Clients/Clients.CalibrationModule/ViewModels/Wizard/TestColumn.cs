using Core.Mvvm;

namespace Clients.CalibrationModule.ViewModels.Wizard {
    public sealed class TestColumn : ViewModel {
        private bool _isSelected;

        public bool IsEnabled { get; private set; }

        public bool IsSelected {
            get { return _isSelected; }
            set {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public TestColumn(string displayName, bool isSelected, bool isEnabled) {
            DisplayName = displayName;
            IsSelected = isSelected;
            IsEnabled = isEnabled;
        }
    }
}
