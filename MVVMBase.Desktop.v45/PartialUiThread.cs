using System;
using System.Windows;

namespace MVVMBase
{
    /// <summary>
    ///     Base View Model
    /// </summary>
    public partial class NotifyObject 
    {
        partial void SetUiThread()
        {
            var dispather = Application.Current?.Dispatcher;
            if (dispather != null)
            {
                _runOnUiThread = (action) => { dispather.Invoke(action); };
            }
        }
    }
}