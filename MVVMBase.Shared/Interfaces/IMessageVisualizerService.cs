using System.Threading.Tasks;

namespace MVVMBase.Interfaces
{
    /// <summary>
    ///     Interface to display UI "MessageBox" style prompts.
    /// </summary>
    public interface IMessageVisualizerService
    {
        /// <summary>
        ///     Show a message on the UI
        /// </summary>
        /// <returns>Async result (true/false)</returns>
        /// <param name="title">Title</param>
        /// <param name="message">Message</param>
        /// <param name="ok">Text for OK button</param>
        /// <param name="cancel">Optional text for Cancel button</param>
        Task<bool> ShowMessage(string title, string message, string ok, string cancel = null);
    }
}