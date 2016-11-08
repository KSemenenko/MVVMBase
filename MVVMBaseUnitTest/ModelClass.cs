using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MVVMBase;

namespace MVVMBaseUnitTest
{
    public class ModelClass : BaseViewModel
    {
        private string myCommandProperty;
        private string myProperty;
        private string myPropertyAutoChange;
        private string myPropertyByName;
        private string myPropertyByMemberName;

        public ModelClass()
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
                return new DelegateCommand(executedParam => { MyPropertyByName = (string)executedParam; },
                    canExecutedParam => MyCommandProperty == "1");
            }
        }

        public void UpdateAll()
        {
            OnPropertyChangedForAll();
        }

    }
}
