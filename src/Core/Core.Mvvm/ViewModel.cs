using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Windows.Threading;

namespace Core.Mvvm {
    public abstract class ViewModel : DispatcherObject, INotifyPropertyChanged, IDisposable {
        private string _displayName;

        public string DisplayName {
            get { return _displayName; }
            protected set {
                _displayName = value;
                OnPropertyChanged("DisplayName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName) {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null) {
                throw new Exception("Invalid property name : " + propertyName);
            }
        }

        public void Dispose() {
            OnDispose();
        }

        protected virtual void OnPropertyChanged(string propertyName) {
            VerifyPropertyName(propertyName);
            OnPropertyChangedRaiseEvent(propertyName);
        }

        protected virtual void OnPropertyChangedRaiseEvent(string propertyName) {
            var handler = Interlocked.CompareExchange(ref PropertyChanged, null, null);
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void OnDispose() {
        }
    }
}
