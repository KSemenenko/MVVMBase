using System;
using System.Windows;

namespace MVVMBase
{
    /// <summary>
    ///     Base View Model
    /// </summary>
    public partial class BaseViewModel
    {
        partial void SetUiThread()
        {
            runOnUiThread = (action) => { Application.Current.Dispatcher.Invoke(action); };
        }
    }
}