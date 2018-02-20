﻿using System;
using System.Collections.Concurrent;

namespace MVVMBase.Collections
{
    /// <summary>
    ///     The observable concurrent queue.
    /// </summary>
    /// <typeparam name="T">
    ///     The content type
    /// </typeparam>
    public sealed class ObservableConcurrentQueue<T> : ConcurrentQueue<T>
    {
        #region Public Events

        /// <summary>
        ///     Occurs when concurrent queue elements [changed].
        /// </summary>
        public event EventHandler<NotifyConcurrentQueueChangedEventArgs<T>> ContentChanged;

        #endregion

        #region Methods

        /// <summary>
        ///     Raises the <see cref="E:Changed" /> event.
        /// </summary>
        /// <param name="args">
        ///     The <see cref="NotifyConcurrentQueueChangedEventArgs{T}" /> instance containing the event data.
        /// </param>
        private void OnContentChanged(NotifyConcurrentQueueChangedEventArgs<T> args)
        {
            ContentChanged?.Invoke(this, args);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds an object to the end of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.
        /// </summary>
        /// <param name="item">
        ///     The object to add to the end of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />
        ///     . The value can be a null reference (Nothing in Visual Basic) for reference types.
        /// </param>
        public new void Enqueue(T item)
        {
            base.Enqueue(item);

            // Raise event added event
            OnContentChanged(new NotifyConcurrentQueueChangedEventArgs<T>(NotifyConcurrentQueueChangedAction.Enqueue, item));
        }

        /// <summary>
        ///     Attempts to remove and return the object at the beginning of the
        ///     <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.
        /// </summary>
        /// <param name="result">
        ///     When this method returns, if the operation was successful, <paramref name="result" /> contains the
        ///     object removed. If no object was available to be removed, the value is unspecified.
        /// </param>
        /// <returns>
        ///     true if an element was removed and returned from the beginning of the
        ///     <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> successfully; otherwise, false.
        /// </returns>
        public new bool TryDequeue(out T result)
        {
            if(!base.TryDequeue(out result))
            {
                return false;
            }

            // Raise item dequeued event
            OnContentChanged(new NotifyConcurrentQueueChangedEventArgs<T>(NotifyConcurrentQueueChangedAction.Dequeue, result));

            if(IsEmpty)
            {
                // Raise Queue empty event
                OnContentChanged(new NotifyConcurrentQueueChangedEventArgs<T>(NotifyConcurrentQueueChangedAction.Empty));
            }

            return true;
        }

        /// <summary>
        ///     Attempts to return an object from the beginning of the
        ///     <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> without removing it.
        /// </summary>
        /// <param name="result">
        ///     When this method returns, <paramref name="result" /> contains an object from the beginning of the
        ///     <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> or an unspecified value if the operation failed.
        /// </param>
        /// <returns>
        ///     true if and object was returned successfully; otherwise, false.
        /// </returns>
        public new bool TryPeek(out T result)
        {
            bool retValue = base.TryPeek(out result);
            if(retValue)
            {
                // Raise item dequeued event
                OnContentChanged(new NotifyConcurrentQueueChangedEventArgs<T>(NotifyConcurrentQueueChangedAction.Peek, result));
            }

            return retValue;
        }

        #endregion
    }
}