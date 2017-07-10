using Foundation;

namespace MVVMBase
{
    /// <summary>
    ///     Base View Model
    /// </summary>
    public partial class BaseViewModel
    {
        partial void SetUiThread()
        {
            runOnUiThread = (action) => new NSObject().InvokeOnMainThread(action); 
        }
    }
}