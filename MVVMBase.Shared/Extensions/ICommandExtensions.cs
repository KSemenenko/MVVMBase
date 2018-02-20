using System.Threading.Tasks;
using System.Windows.Input;
using MVVMBase.Interfaces;

namespace MVVMBase.Extensions
{
    public static class ICommandExtensions
    {
        /// <summary>
        ///     Calls a command checking CanExecute method
        /// </summary>
        public static void Run(this ICommand command)
        {
            if(command.CanExecute(null))
            {
                command.Execute(null);
            }
        }

        /// <summary>
        ///     Calls a command checking CanExecute method
        /// </summary>
        /// <param name="command">
        ///     ICommand
        /// </param>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be
        ///     set to null.
        /// </param>
        public static void Run(this ICommand command, object parameter)
        {
            if(command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        /// <summary>
        ///     Calls a command checking CanExecute method
        /// </summary>
        public static Task RunAsync(this ICommand command)
        {
            if(command is IAsyncDelegateCommand asyncCommand)
            {
                return asyncCommand.ExecuteAsync(null);
            }
            return Task.Run(() => { command.Run(); });
        }

        /// <summary>
        ///     Calls a command checking CanExecute method
        /// </summary>
        /// <param name="command">
        ///     ICommand
        /// </param>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be
        ///     set to null.
        /// </param>
        public static Task RunAsync(this ICommand command, object parameter)
        {
            if (command is IAsyncDelegateCommand asyncCommand)
            {
                return asyncCommand.ExecuteAsync(parameter);
            }
            return Task.Run(() => { command.Run(parameter); });
        }

    }
}