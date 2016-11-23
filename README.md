# MVVMBase Mini Framework
This is a small library that uses pattern MVVM for a handy use in WPF and Xamarin forms. 
 
## Available at NuGet. 
https://www.nuget.org/packages/ksemenenko.MVVMBase/

## Example use:
```cs
public class ModelClass : BaseViewModel
{

    public ModelClass()
    {
        // bind to property
        BindToPropertyChange(nameof(MyProperty), nameof(MyCommand));
        BindToPropertyChange(() => MyCommand, nameof(MyPropertyByName));
    }

    private string myProperty;
    public string MyProperty
    {
        get { return myProperty; }
        set
        {
            myProperty = value;
            OnPropertyChanged(() => MyProperty);
        }
    }
    
    private string myPropertyByName;
    public string MyPropertyByName
    {
        get { return myPropertyByName; }
        set
        {
            myPropertyByName = value;
            OnPropertyChanged(nameof(MyPropertyByName));
        }
    }
    
    private string myPropertyAutoChange;
    public string MyPropertyAutoChange
    {
        get { return myPropertyAutoChange; }
        set
        {
            SetProperty(ref myPropertyAutoChange, value);
        }
    }
    
    private string myPropertyByMemberName;
    public string MyPropertyByMemberName
    {
        get { return myPropertyByMemberName; }
        set
        {
            myPropertyByMemberName = value;
            OnPropertyChanged();
        }
    }

    public ICommand MyCommand
    {
        get
        {
            return new DelegateCommand(executedParam =>
            {
                // Do something
            },
            canExecutedParam => { return true; });
        }
    }
}
```

|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|.NET|Yes|3.5+|
|Xamarin.iOS|Yes|iOS 6+|
|Xamarin.iOS Unified|Yes|iOS 6+|
|Xamarin.Android|Yes|API 10+|
|Windows Phone 8|Partial|8.0+|
|Windows Phone 8.1|Yes|8.1+|
|Windows Store|Yes|8.1+|
|Windows 10 UWP|Yes|10+|
