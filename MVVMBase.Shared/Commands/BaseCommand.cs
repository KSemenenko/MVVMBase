using System;
using System.Windows.Input;

namespace MVVMBase
{
    /// <summary>
    ///     Provides a simple ICommand implementation.
    /// </summary>
    public class BaseCommand : ICommand
    {
        private readonly Func<bool> canExecute;
        private readonly Action command;

        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="command">The execute action.</param>
        /// <param name="canExecute">The can execute predicate.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BaseCommand(Action command, Func<bool> canExecute = null)
        {
            if(command == null)
            {
                throw new ArgumentNullException("command");
            }
            this.canExecute = canExecute;
            this.command = command;
        }

        /// <summary>
        ///     Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be
        ///     set to null.
        /// </param>
        public void Execute(object parameter)
        {
            command();
        }

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be
        ///     set to null.
        /// </param>
        /// <returns>
        ///     True if this command can be executed, otherwise - false.
        /// </returns>
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