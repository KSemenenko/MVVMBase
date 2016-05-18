# MVVMBase

## NuGet
https://www.nuget.org/packages/KSemenenko.MVVMBase/

### How it use
code sample:



```
public class ModelClass : BaseViewModel
{
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
