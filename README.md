# MVVMBase

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
