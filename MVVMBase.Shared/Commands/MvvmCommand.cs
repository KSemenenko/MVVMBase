using System;
using System.Threading.Tasks;

namespace MVVMBase.Commands
{
    /// <summary>
    ///     Provides a simple ICommand implementation.
    /// </summary>
    public class MvvmCommand : BaseMvvmCommand
    {
        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        public MvvmCommand(Action<object> execute) : base(execute)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        public MvvmCommand(Func<object, Task> execute) : base(execute)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        /// <param name="canExecute">The can execute predicate.</param>
        public MvvmCommand(Action<object> execute, Predicate<object> canExecute) : base(execute, canExecute)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        /// <param name="canExecute">The can execute predicate.</param>
        public MvvmCommand(Func<object, Task> execute, Predicate<object> canExecute) : base(execute, canExecute)
        {
        }

        #region Run

        /// <summary>
        ///     Calls a command checking CanExecute method
        /// </summary>
        public void Run()
        {
            if(CanExecute(null))
            {
                Execute(null);
            }
        }

        /// <summary>
        ///     Calls a command checking CanExecute method
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be
        ///     set to null.
        /// </param>
        public void Run(object parameter)
        {
            if(CanExecute(null))
            {
                Execute(parameter);
            }
        }

        /// <summary>
        ///     Calls a command checking CanExecute method
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be
        ///     set to null.
        /// </param>
        /// <param name="executeParameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be
        ///     set to null.
        /// </param>
        public void Run(object parameter, object executeParameter)
        {
            if(CanExecute(executeParameter))
            {
                Execute(parameter);
            }
        }

        #endregion

        #region RunAsync

        /// <summary>
        ///     Calls a command checking CanExecute method
        /// </summary>
        public Task RunAsync()
        {
            return Task.Run(() =>
            {
                if(CanExecute(null))
                {
                    Execute(null);
                }
            });
        }

        /// <summary>
        ///     Calls a command checking CanExecute method
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be
        ///     set to null.
        /// </param>
        public Task RunAsync(object parameter)
        {
            if(IsAsync)
            {
                if (CanExecute(null))
                {
                    return ExecuteAsync(parameter);
                }
            }

            return Task.Run(() =>
            {
                if(CanExecute(null))
                {
                    Execute(parameter);
                }
            });
        }

        /// <summary>
        ///     Calls a command checking CanExecute method
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be
        ///     set to null.
        /// </param>
        /// <param name="executeParameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be
        ///     set to null.
        /// </param>
        public Task RunAsync(object parameter, object executeParameter)
        {
            if (IsAsync)
            {
                if (CanExecute(executeParameter))
                {
                    return ExecuteAsync(parameter);
                }
            }

            return Task.Run(() =>
            {
                if(CanExecute(executeParameter))
                {
                    Execute(parameter);
                }
            });
        }

        #endregion
    }
}