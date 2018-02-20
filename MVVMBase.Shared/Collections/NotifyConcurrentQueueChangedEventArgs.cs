using System;

namespace MVVMBase.Collections
{
    /// <summary>
    ///     The notify concurrent queue changed event args.
    /// </summary>
    /// <typeparam name="T">
    ///     The item type
    /// </typeparam>
    public class NotifyConcurrentQueueChangedEventArgs<T> : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotifyConcurrentQueueChangedEventArgs{T}" /> class.
        /// </summary>
        /// <param name="action">
        ///     The action.
        /// </param>
        /// <param name="changedItem">
        ///     The changed item.
        /// </param>
        public NotifyConcurrentQueueChangedEventArgs(NotifyConcurrentQueueChangedAction action, T changedItem)
        {
            Action = action;
            ChangedItem = changedItem;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotifyConcurrentQueueChangedEventArgs{T}" /> class.
        /// </summary>
        /// <param name="action">
        ///     The action.
        /// </param>
        public NotifyConcurrentQueueChangedEventArgs(NotifyConcurrentQueueChangedAction action)
        {
            Action = action;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the action.
        /// </summary>
        /// <value>
        ///     The action.
        /// </value>
        public NotifyConcurrentQueueChangedAction Action { get; }

        /// <summary>
        ///     Gets the changed item.
        /// </summary>
        /// <value>
        ///     The changed item.
        /// </value>
        public T ChangedItem { get; }

        #endregion
    }
}