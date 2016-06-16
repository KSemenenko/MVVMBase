using System;
using System.Windows.Input;

namespace MVVMBase
{
    internal class BaseCommand : ICommand
    {
        private readonly Func<bool> canExecute;
        private readonly Action command;

        public BaseCommand(Action command, Func<bool> canExecute = null)
        {
            if(command == null)
            {
                throw new ArgumentNullException("command");
            }
            this.canExecute = canExecute;
            this.command = command;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            command();
        }

        public bool CanExecute(object parameter)
        {
            if(canExecute == null)
            {
                return true;
            }
            return canExecute();
        }
    }
}