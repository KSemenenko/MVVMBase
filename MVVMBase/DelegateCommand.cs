using System;
using System.Linq;
using System.Windows.Input;

namespace MVVMBase
{
    /// <summary>
    /// Provides a simple ICommand implementation.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> execute;

        private readonly Predicate<object> canExecute;

        /// <summary>
        /// Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        public DelegateCommand(Action<object> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        /// <param name="canExecute">The can execute predicate.</param>
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// True if this command can be executed, otherwise - false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null)
            {
                return true;
            }
            return this.canExecute(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}