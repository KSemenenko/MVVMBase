using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMBase.Commands
{
    /// <summary>
    ///     Provides a simple ICommand implementation.
    /// </summary>
    public abstract class BaseMvvmCommand : ICommand
    {
        private readonly Predicate<object> canExecute;
        private readonly Action<object> execute;

        private readonly Func<object, Task> executeTask;

        protected bool IsAsync = false;

        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        public BaseMvvmCommand(Action<object> execute) : this(execute, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        public BaseMvvmCommand(Func<object, Task> execute) 
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
            this.executeTask = execute;
            IsAsync = true;
        }

        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        /// <param name="canExecute">The can execute predicate.</param>
        public BaseMvvmCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if(execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        /// <param name="canExecute">The can execute predicate.</param>
        public BaseMvvmCommand(Func<object, Task> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
            this.executeTask = execute;
            this.canExecute = canExecute;
            IsAsync = true;
        }


        /// <summary>
        ///     Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

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
            return canExecute(parameter);
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be
        ///     set to null.
        /// </param>
        public async void Execute(object parameter)
        {
            execute?.Invoke(parameter);
            if(executeTask != null)
            {
                await executeTask(parameter);
            }
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be
        ///     set to null.
        /// </param>
        protected async Task ExecuteAsync(object parameter)
        {
            if (executeTask != null)
            {
                await executeTask(parameter);
            }
        }
    }
}