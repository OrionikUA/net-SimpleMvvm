using System;
using System.Windows.Input;

namespace OrionikUA.SimpleMvvm
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute) : this(execute, null) { }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class RelayCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute) : this(execute, null) { }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null) return true;
            var val = parameter;
            if (parameter != null && parameter.GetType() != typeof(T) && parameter is IConvertible)
                val = Convert.ChangeType(parameter, typeof(T), null);

            if (typeof(T).IsValueType && val == null)
                return _canExecute.Invoke(default);
            return _canExecute.Invoke((T)val);
        }

        public void Execute(object parameter)
        {
            var val = parameter;
            if (parameter != null && parameter.GetType() != typeof(T) && parameter is IConvertible)
                val = Convert.ChangeType(parameter, typeof(T), null);

            if (typeof(T).IsValueType && val == null)
                _execute.Invoke(default);
            else
                _execute.Invoke((T)val);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
