using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MVVMBase;
using MVVMBase.Commands;

namespace MVVMBaseUnitTest
{
    public class BindClass : BaseViewModel
    {
        private string myCommandProperty;
        private string myProperty;
        private string myPropertyAutoChange;
        private string myPropertyByName;
        private string myPropertyByMemberName;

        public BindClass()
        {
        }

        public string MyCommandProperty
        {
            get { return myCommandProperty; }
            set
            {
                myCommandProperty = value;
                OnPropertyChanged(() => MyCommandProperty);
                OnPropertyChanged(() => MyCommand);
            }
        }

        public string MyProperty
        {
            get { return myProperty; }
            set
            {
                myProperty = value;
                OnPropertyChanged(() => MyProperty);
            }
        }

        public string MyPropertyByName
        {
            get { return myPropertyByName; }
            set
            {
                myPropertyByName = value;
                OnPropertyChanged(nameof(MyPropertyByName));
            }
        }

        public string MyPropertyAutoChange
        {
            get { return myPropertyAutoChange; }
            set { SetProperty(ref myPropertyAutoChange, value); }

        }
        public string MyPropertyAutoGetSet
        {
            get { return GetValue<string>(); }
            set { SetValue(value);}
        }

        public string MyPropertyByMemberName
        {
            get { return myPropertyByMemberName; }
            set
            {
                myPropertyByMemberName = value;
                OnPropertyChanged();
            }
        }

        [Notify(nameof(MyDependCommand))]
        public string MyDependProperty
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }



        public int MyPropertyIntAutoGetSet
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public ObservableCollection<int> MyObservableProperty
        {
            get { return GetValue<ObservableCollection<int>>(); }
            set { SetValue(value); }
        }

        [DependsOn(nameof(MyObservableProperty))]
        public int MyObservablePropertyCount
        {
            get { return MyObservableProperty.Sum(); }
        }

        [DependsOn(nameof(MyPropertyIntAutoGetSet))]
        public int MyDependedPropertyInt => MyPropertyIntAutoGetSet * MyPropertyIntAutoGetSet;

        public MvvmCommand MyCommand
        {
            get
            {
                return new MvvmCommand(executedParam => { MyPropertyByName = (string)executedParam; },
                    canExecutedParam => MyCommandProperty == "1");
            }
        }

        public MvvmCommand MyDependCommand
        {
            get
            {
                return new MvvmCommand(executedParam => { MyPropertyByName = (string)executedParam; },
                    canExecutedParam => MyDependProperty == "1");
            }
        }

        public void UpdateAll()
        {
            OnPropertyChangedForAll();
        }

    }
}
