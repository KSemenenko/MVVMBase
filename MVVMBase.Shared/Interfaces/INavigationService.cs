using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVVMBase.Interfaces
{
    /// <summary>
    /// Interface to manage navigation in the application.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Key comparer for navigation keys. Change this if you
        /// use a complex key which doesn't support direct equality.
        /// </summary>
        IEqualityComparer<object> KeyComparer { get; set; }

        /// <summary>
        /// Event raised when NavigateAsync is used.
        /// </summary>
        event EventHandler Navigated;

        /// <summary>
        /// Event raised when a GoBackAsync operation occurs.
        /// </summary>
        event EventHandler NavigatedBack;

        /// <summary>
        /// Pops all pages off the stack up to the first one.
        /// </summary>
        Task PopToRootAsync();

        /// <summary>
        /// Navigate to a page using the known key.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="key">Navigation key.</param>
        /// <param name="state">State (often View model)</param>
        Task NavigateAsync(object key, object state = null);

        /// <summary>
        /// Returns true/false whether we can go backwards on the Nav Stack.
        /// </summary>
        /// <value><c>true</c> if can go back; otherwise, <c>false</c>.</value>
        bool CanGoBack { get; }

        /// <summary>
        /// Pops the last page off the stack and navigates to it.
        /// </summary>
        /// <returns>Async response</returns>
        Task GoBackAsync();

        /// <summary>
        /// Push a page onto the modal stack.
        /// </summary>
        /// <returns>Async response</returns>
        /// <param name="key">Navigation key.</param>
        /// <param name="state">State (often ViewModel)</param>
        Task PushModalAsync(object key, object state = null);

        /// <summary>
        /// Pops the last page off the modal stack
        /// </summary>
        /// <returns>Async response</returns>
        Task PopModalAsync();

        /// <summary>
        /// Register an action to take when a specific navigation is requested.
        /// </summary>
        /// <param name="key">Navigation Key</param>
        /// <param name="action">Action to perform</param>
        void RegisterAction(object key, Action action);

        /// <summary>
        /// Register an action to take when a specific navigation is requested.
        /// </summary>
        /// <param name="key">Navigation Key</param>
        /// <param name="action">Action to perform</param>
        void RegisterAction(object key, Action<object> action);

        /// <summary>
        /// Unregister a specific key
        /// </summary>
        /// <param name="key">Navigation Key</param>
        void Unregister(object key);
    }
}