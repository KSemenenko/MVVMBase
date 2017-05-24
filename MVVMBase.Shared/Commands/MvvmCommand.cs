using System;
using System.Threading.Tasks;

namespace MVVMBase
{
    /// <summary>
    ///     Provides a simple ICommand implementation.
    /// </summary>
    public class MvvmCommand : DelegateCommand
    {
        /// <summary>
        ///     Initializes a new instance of the  class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        public MvvmCommand(Action<object> execute) : base(execute, null)
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


        #region Run

        /// <summary>
        /// Calls a command checking CanExecute method
        /// </summary>
        public void Run()
        {
            if(CanExecute(null))
            {
                Execute(null);
            }
        }

        /// <summary>
        /// Calls a command checking CanExecute method
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
        /// Calls a command checking CanExecute method
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
        /// Calls a command checking CanExecute method
        /// </summary>
        public Task RunAsync()
        {
            return Task.Run(() =>
            {
                if (CanExecute(null))
                {
                    Execute(null);
                }
            });
        }

        /// <summary>
        /// Calls a command checking CanExecute method
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be
        ///     set to null.
        /// </param>
        public Task RunAsync(object parameter)
        {
            return Task.Run(() =>
            {
                if (CanExecute(null))
                {
                    Execute(parameter);
                }
            });
        }

        /// <summary>
        /// Calls a command checking CanExecute method
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
            return Task.Run(() =>
            {
                if (CanExecute(executeParameter))
                {
                    Execute(parameter);
                }
            });
        }
        #endregion
    }
}