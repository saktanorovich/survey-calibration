using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Core.Mvvm.Commands;

namespace Core.Mvvm {
    public abstract class WizardBase : ViewModel {
        #region Fields

        private IList<WizardBasePage> _pages;
        private WizardBasePage _currentPage;
        private IWizardPageNavigator _pageNavigator;

        private ICommand _prevPageCommand;
        private ICommand _nextPageCommand;
        private ICommand _finishCommand;
        private ICommand _cancelCommand;

        #endregion

        #region Events

        public event EventHandler RequestFinish;
        public event EventHandler RequestCancel;

        #endregion

        #region Properties

        public IList<WizardBasePage> Pages {
            get {
                if (_pages == null) {
                    _pages = new ReadOnlyCollection<WizardBasePage>(GetPagesImpl());
                    if (_pages.Count > 0) {
                        CurrentPage = _pages.First();
                    }
                }
                return _pages;
            }
        }

        public WizardBasePage CurrentPage {
            get {
                if (_currentPage == null) {
                    _pages = new ReadOnlyCollection<WizardBasePage>(GetPagesImpl());
                    if (_pages.Count > 0) {
                        CurrentPage = _pages.First();
                    }
                }
                return _currentPage;
            }
            set {
                OnCurrentPageChanging();
                _currentPage = value;
                OnCurrentPageChanged();
                OnPropertyChanged("CurrentPage");
            }
        }

        public IWizardPageNavigator PageNavigator {
            get {
                return _pageNavigator ?? (_pageNavigator = GetPageNavigatorImpl());
            }
        }

        public ICommand PrevPageCommand { get { return _prevPageCommand ?? (_prevPageCommand = new SimpleCommand(OnPrevPage, CanPrevPage)); } }
        public ICommand NextPageCommand { get { return _nextPageCommand ?? (_nextPageCommand = new SimpleCommand(OnNextPage, CanNextPage)); } }
        public ICommand FinishCommand   { get { return _finishCommand   ?? (_finishCommand   = new SimpleCommand(OnFinish, CanFinish)); } }
        public ICommand CancelCommand   { get { return _cancelCommand   ?? (_cancelCommand   = new SimpleCommand(OnCancel, CanCancel)); } }

        #endregion

        #region Property Changed Handlers

        protected virtual void OnCurrentPageChanging() {
            if (_currentPage != null) {
                _currentPage.IsCurrentPage = false;
            }
        }

        protected virtual void OnCurrentPageChanged() {
            if (_currentPage != null) {
                _currentPage.IsCurrentPage = true;
            }
        }

        #endregion

        #region Command Handlers

        protected virtual void OnPrevPage() {
            PageNavigator.NavigatePrevPage(this);
        }

        protected virtual bool CanPrevPage() {
            if (CurrentPage != null) {
                return CurrentPage.ShowPrevButton && CurrentPage.CanGoPrevPage();
            }
            return false;
        }

        protected virtual void OnNextPage() {
            PageNavigator.NavigateNextPage(this);
        }

        protected virtual bool CanNextPage() {
            if (CurrentPage != null) {
                return CurrentPage.ShowNextButton && CurrentPage.CanGoNextPage();
            }
            return false;
        }

        protected virtual void OnFinish() {
            OnRequestFinish();
        }

        protected virtual bool CanFinish() {
            if (CurrentPage != null) {
                return CurrentPage.ShowFinishButton && CurrentPage.CanFinish();
            }
            return false;
        }

        protected virtual void OnCancel() {
            if (CurrentPage != null) {
                CurrentPage.Cancel();
            }
            OnRequestCancel();
        }

        protected virtual bool CanCancel() {
            if (CurrentPage != null) {
                return CurrentPage.ShowCancelButton && CurrentPage.CanCancel();
            }
            return false;
        }

        #endregion

        #region Protected Methods

        protected abstract IList<WizardBasePage> GetPagesImpl();

        protected virtual IWizardPageNavigator GetPageNavigatorImpl() {
            return new WizardPageNavigatorBase();
        }

        protected virtual void OnRequestFinish() {
            var handler = Interlocked.CompareExchange(ref RequestFinish, null, null);
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnRequestCancel() {
            var handler = Interlocked.CompareExchange(ref RequestCancel, null, null);
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
