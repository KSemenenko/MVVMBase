namespace MVVMBase.Collections
{
    /// <summary>
    ///     The notify concurrent queue changed action.
    /// </summary>
    public enum NotifyConcurrentQueueChangedAction
    {
        /// <summary>
        ///     The enqueue
        /// </summary>
        Enqueue,

        /// <summary>
        ///     The de-queue
        /// </summary>
        Dequeue,

        /// <summary>
        ///     The peek
        /// </summary>
        Peek,

        /// <summary>
        ///     The empty
        /// </summary>
        Empty
    }
}