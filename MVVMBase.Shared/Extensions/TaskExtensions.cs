using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MVVMBase.Extensions
{
    /// <summary>
    ///     Extensions for the System.Threading.Tasks.Task type.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        ///     This method can be used to ignore the result of a Task without
        ///     losing the ability to throw the exception if the task fails.
        /// </summary>
        /// <example>
        ///     <code>
        ///     Task.Run(() => ...).IgnoreResult();
        /// </code>
        /// </example>
        /// <param name="task">Task to ignore</param>
        /// <param name="faultHandler">Optional handler for the exception; if null then method throws on UI thread.</param>
        /// <param name="member">Caller name</param>
        /// <param name="lineNumber">Line number.</param>
        public static void IgnoreResult(this Task task, Action<Exception> faultHandler = null, [CallerMemberName] string member = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            task.ContinueWith(tr =>
            {
                Debug.WriteLine("Encountered {0} at {1}, line #{2}", task.Exception.GetType(), member, lineNumber);
                Debug.WriteLine(task.Exception.Flatten());

                if(faultHandler != null)
                {
                    faultHandler.Invoke(task.Exception);
                }
                else
                {
                    Debug.WriteLine("WARNING: exception {0} was ignored!", task.Exception.GetType());
                }
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}