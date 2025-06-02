using System;
using System.Windows.Input;

namespace UPMurtazinIS121.Validations
{
    public class RelayCommand(Action<object> execute) : ICommand
    {
        private readonly Action<object> _execute = execute ?? throw new ArgumentNullException(nameof(execute));

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => _execute(parameter);
    }
}