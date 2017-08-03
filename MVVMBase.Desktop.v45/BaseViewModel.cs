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
            var dispather = Application.Current?.Dispatcher;
            if(dispather != null)
            {
                runOnUiThread = (action) => { dispather?.Invoke(action); };
            }
        }
    }
}