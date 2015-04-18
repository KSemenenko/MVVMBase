using System;
using System.Windows.Input;

namespace MVVMBase
{
    internal class BaseCommand : ICommand
    {
        private readonly Action _command;
        private readonly Func<bool> _canExecute;

        public BaseCommand(Action command, Func<bool> canExecute = null)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            this._canExecute = canExecute;
            this._command = command;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this._command();
        }

        public bool CanExecute(object parameter)
        {
            if (this._canExecute == null)
            {
                return true;
            }
            return this._canExecute();
        }
    }
}