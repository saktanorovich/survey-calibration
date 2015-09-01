namespace Core.Mvvm {
    public abstract class WizardBasePage : ViewModel {
        #region Constants

        public static readonly string PrevDefaultText = "Back";
        public static readonly string NextDefaultText = "Next";
        public static readonly string FinishDefaultText = "Finish";
        public static readonly string CancelDefaultText = "Cancel";

        #endregion

        #region Fields

        private bool _isFirstPage;
        private bool _isLastPage;

        private string _prevButtonText;
        private string _nextButtonText;
        private string _finishButtonText;
        private string _cancelButtonText;

        private bool _showPrevButton;
        private bool _showNextButton;
        private bool _showFinishButton;
        private bool _showCancelButton;

        private bool _isCurrentPage;

        #endregion

        #region Properties

        public bool IsFirstPage {
            get { return _isFirstPage; }
            protected set {
                _isFirstPage = value;
                OnPropertyChanged("IsFirstPage");
            }
        }

        public bool IsLastPage {
            get { return _isLastPage; }
            protected set {
                _isLastPage = value;
                OnPropertyChanged("IsLastPage");
            }
        }

        public string PrevButtonText {
            get { return _prevButtonText; }
            protected set {
                _prevButtonText = value;
                OnPropertyChanged("PrevButtonText");
            }
        }

        public string NextButtonText {
            get { return _nextButtonText; }
            protected set {
                _nextButtonText = value;
                OnPropertyChanged("NextButtonText");
            }
        }

        public string FinishButtonText {
            get { return _finishButtonText; }
            protected set {
                _finishButtonText = value;
                OnPropertyChanged("FinishButtonText");
            }
        }

        public string CancelButtonText {
            get { return _cancelButtonText; }
            protected set {
                _cancelButtonText = value;
                OnPropertyChanged("CancelButtonText");
            }
        }

        public bool ShowPrevButton {
            get { return _showPrevButton; }
            protected set {
                _showPrevButton = value;
                OnPropertyChanged("ShowPrevButton");
            }
        }

        public bool ShowNextButton {
            get { return _showNextButton; }
            protected set {
                _showNextButton = value;
                OnPropertyChanged("ShowNextButton");
            }
        }

        public bool ShowFinishButton {
            get { return _showFinishButton; }
            protected set {
                _showFinishButton = value;
                OnPropertyChanged("ShowFinishButton");
            }
        }

        public bool ShowCancelButton {
            get { return _showCancelButton; }
            protected set {
                _showCancelButton = value;
                OnPropertyChanged("ShowCancelButton");
            }
        }

        public bool IsCurrentPage {
            get { return _isCurrentPage; }
            set {
                if (_isCurrentPage == value) {
                    return;
                }
                _isCurrentPage = value;
                OnIsCurrentPageChanged();
                OnPropertyChanged("IsCurrentPage");
            }
        }

        #endregion

        #region Ctors

        protected WizardBasePage() {
            PrevButtonText = PrevDefaultText;
            NextButtonText = NextDefaultText;
            FinishButtonText = FinishDefaultText;
            CancelButtonText = CancelDefaultText;
        }

        #endregion

        #region Property Changed Handlers

        protected virtual void OnIsCurrentPageChanged() {
            if (_isCurrentPage) {
                OnActivated();
            }
        }

        #endregion

        #region Public Methods

        public virtual void Cancel() {
        }

        public virtual bool CanGoPrevPage() {
            return !IsFirstPage;
        }

        public virtual bool CanGoNextPage() {
            return !IsLastPage;
        }

        public virtual bool CanFinish() {
            return IsLastPage;
        }

        public virtual bool CanCancel() {
            return true;
        }

        #endregion

        #region Protected Methods

        protected virtual void OnActivated() { }

        #endregion
    }
}
