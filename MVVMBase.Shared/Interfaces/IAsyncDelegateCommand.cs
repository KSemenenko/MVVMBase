using System.Threading.Tasks;

namespace MVVMBase.Interfaces
{
    /// <summary>
    ///     Extension of ICommand which exposes a raise execute handler and async support.
    /// </summary>
    public interface IAsyncDelegateCommand : IDelegateCommand
    {
        /// <summary>
        ///     Executes the command and returns the async Task.
        /// </summary>
        /// <returns>async result</returns>
        /// <param name="parameter">Parameter.</param>
        Task ExecuteAsync(object parameter);
    }

    /// <summary>
    ///     Extension of ICommand which exposes a raise execute handler.
    /// </summary>
    public interface IAsyncDelegateCommand<T> : IAsyncDelegateCommand
    {
        /// <summary>
        ///     Executes the command and returns the async Task.
        /// </summary>
        /// <returns>async result</returns>
        /// <param name="parameter">Parameter.</param>
        Task ExecuteAsync(T parameter);
    }
}