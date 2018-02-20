# MVVMBase Mini Framework
This is a small library that uses pattern MVVM for a handy use in WPF and Xamarin forms. 
 
## Available at NuGet. 
https://www.nuget.org/packages/ksemenenko.MVVMBase/

### Build status
[![Build status](https://ci.appveyor.com/api/projects/status/5lckjbd45no96f4c?svg=true)](https://ci.appveyor.com/project/KSemenenko/mvvmbase)
[![Build status](https://ci.appveyor.com/api/projects/status/5lckjbd45no96f4c/branch/master?svg=true)](https://ci.appveyor.com/project/KSemenenko/mvvmbase/branch/master)


## Example use:
```cs
public class ModelClass : NotifyObject
{

    public ModelClass()
    {
        // bind to property
        BindToPropertyChange(nameof(MyProperty), nameof(MyCommand));
        BindToPropertyChange(() => MyCommand, nameof(MyPropertyByName));

        //or
        Bind(nameof(MyProperty)).To(nameof(MyCommand));
        Bind(() => MyCommand).To(()=> MyPropertyByName);

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

    public MvvmCommand MyCommand
    {
        get
        {
            return new MvvmCommand(executedParam =>
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
|Windows Phone 8.1|Partial|8.1+|
|Windows Store|Partial|8.1+|
|Windows 10 UWP|Yes|10+|
