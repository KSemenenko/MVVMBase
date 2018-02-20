using Foundation;

namespace MVVMBase
{
    /// <summary>
    ///     Base View Model
    /// </summary>
    public partial class NotifyObject
    {
        partial void SetUiThread()
        {
            _runOnUiThread = (action) => new NSObject().InvokeOnMainThread(action); 
        }
    }
}