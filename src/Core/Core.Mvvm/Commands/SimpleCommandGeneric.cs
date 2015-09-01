using System;
using System.Windows.Input;

namespace Core.Mvvm.Commands {
    public class SimpleCommand<T> : ICommand {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public SimpleCommand(Action<T> execute)
            : this(execute, null) {
        }

        public SimpleCommand(Action<T> execute, Predicate<T> canExecute) {
            if (execute == null) {
                throw new ArgumentNullException("execute");
            }
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged {
            add {
                if (_canExecute != null) {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove {
                if (_canExecute != null) {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public void Execute(object parameter) {
            _execute((T)parameter);
        }
    }
}