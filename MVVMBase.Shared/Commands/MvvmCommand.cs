using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMBase.Commands
{
    /// <summary>
    ///     Provides a simple ICommand implementation.
    /// </summary>
    public class MvvmCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        private readonly Func<T, Task> _executeTask;

        protected bool IsAsync;

        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        /*public MvvmCommand(Func<Task> execute, ICanShowError errorHandler=null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
            _executeTask = f =>
            {
                try
                {
                    Task result = execute();
                    return result;
                }
                catch (PumaApiException ex)
                {
                    errorHandler?.ShowError(ex.Message);
                    return Task.CompletedTask;
                }
            };
            IsAsync = true;
        }*/
        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        /// <param name="canExecute">The can execute predicate.</param>
        public MvvmCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        /// <param name="canExecute">The can execute predicate.</param>
        public MvvmCommand(Func<T, Task> execute, Predicate<T> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _executeTask = execute;
            _canExecute = canExecute;
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
        public bool CanExecute(T parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be
        ///     set to null.
        /// </param>
        public async void Execute(T parameter)
        {
            _execute?.Invoke(parameter);
            if (_executeTask != null)
            {
                await _executeTask(parameter);
            }
        }

        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be
        ///     set to null.
        /// </param>
        protected async Task ExecuteAsync(T parameter)
        {
            if (_executeTask != null)
            {
                await _executeTask(parameter);
            }
        }


    }

    /// <summary>
    ///     Provides a simple ICommand implementation.
    /// </summary>
    public class MvvmCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        private readonly Func<object, Task> _executeTask;

        protected bool IsAsync;

        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        /*public MvvmCommand(Func<Task> execute, ICanShowError errorHandler=null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
            _executeTask = f =>
            {
                try
                {
                    Task result = execute();
                    return result;
                }
                catch (PumaApiException ex)
                {
                    errorHandler?.ShowError(ex.Message);
                    return Task.CompletedTask;
                }
            };
            IsAsync = true;
        }*/
        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        /// <param name="canExecute">The can execute predicate.</param>
        public MvvmCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            if(execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        /// <param name="canExecute">The can execute predicate.</param>
        public MvvmCommand(Func<object, Task> execute, Predicate<object> canExecute = null)
        {
            if(execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _executeTask = execute;
            _canExecute = canExecute;
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
            if(_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
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
            _execute?.Invoke(parameter);
            if(_executeTask != null)
            {
                await _executeTask(parameter);
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
            if(_executeTask != null)
            {
                await _executeTask(parameter);
            }
        }

       
    }
}