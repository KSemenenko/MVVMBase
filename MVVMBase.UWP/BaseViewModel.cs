using System;
using Windows.UI.Core;

namespace MVVMBase
{
    /// <summary>
    ///     Base View Model
    /// </summary>
    public partial class BaseViewModel 
    {
        partial void SetUiThread()
        {
            runOnUiThread = async (action) => 
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, ()=> action());
            };
        }
    }
}