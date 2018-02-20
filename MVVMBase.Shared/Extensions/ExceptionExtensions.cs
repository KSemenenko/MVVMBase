using System;
using System.Text;

namespace MVVMBase.Extensions
{
    /// <summary>
    ///     Extensions for the global Exception type
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     Flatten the exception and inner exception data.
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="header">Any string prefix to add</param>
        /// <param name="includeStackTrace">True to include stack trace at end</param>
        /// <returns>String with Message and all InnerException messages appended together</returns>
        public static string Flatten(this Exception ex, string header = "", bool includeStackTrace = false)
        {
            var sb = new StringBuilder(header);

            Exception current;
            var aex = ex as AggregateException;
            if(aex != null)
            {
                sb.AppendLine("Aggregate Exception.");
                aex = aex.Flatten();
                for(var i = 0; i < aex.InnerExceptions.Count; i++)
                {
                    current = aex.InnerExceptions[i];
                    sb.AppendLine(current.Flatten($"{i}: ", includeStackTrace));
                }
            }
            else
            {
                current = ex;
                while(current != null)
                {
                    sb.AppendFormat("{0} : {1}", current.GetType(), current.Message);
                    if(includeStackTrace)
                    {
                        sb.Append(ex.StackTrace);
                    }
                    else
                    {
                        sb.AppendLine();
                    }

                    current = current.InnerException;
                    if(current != null && includeStackTrace)
                    {
                        sb.AppendLine();
                    }
                }
            }

            return sb.ToString();
        }
    }
}