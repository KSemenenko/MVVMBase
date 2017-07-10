using System;
using Android.App;
using Android.OS;

namespace MVVMBase
{
    /// <summary>
    ///     Base View Model
    /// </summary>
    public partial class BaseViewModel 
    {
        partial void SetUiThread()
        {
            runOnUiThread = (action) => new Handler(Application.Context.MainLooper).Post(action);
        }
    }
}