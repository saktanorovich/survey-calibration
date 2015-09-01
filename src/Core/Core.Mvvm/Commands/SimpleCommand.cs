using System;
using System.Windows.Input;

namespace Core.Mvvm.Commands {
    public class SimpleCommand : ICommand {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public SimpleCommand(Action execute)
            : this(execute, null) {
        }

        public SimpleCommand(Action execute, Func<bool> canExecute) {
            if (execute == null) {
                throw new ArgumentNullException("execute");
            }
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            return _canExecute == null || _canExecute();
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
            _execute();
        }
    }
}
