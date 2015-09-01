using System;
using System.Threading;
using System.Windows.Input;
using Core.Mvvm.Commands;

namespace Core.Mvvm {
    public abstract class ClosableViewModel : ViewModel {
        private ICommand _closeCommand;

        public ICommand CloseCommand {
            get { return _closeCommand ?? (_closeCommand = new SimpleCommand(OnClose, CanClose)); }
        }

        public event EventHandler RequestClose;

        protected virtual void OnClose() {
            OnRequestClose();
        }

        protected virtual bool CanClose() {
            return true;
        }

        protected virtual void OnRequestClose() {
            var handler = Interlocked.CompareExchange(ref RequestClose, null, null);
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
