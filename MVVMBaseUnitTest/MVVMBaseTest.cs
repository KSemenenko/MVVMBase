using System.Threading;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVMBase;

namespace MVVMBaseUnitTest
{
    [TestClass]
    public class MVVMBaseTest
    {
        [TestMethod]
        public void MyPropertyTest()
        {
            var myClass = new ModelClass();

            myClass.PropertyChanged += (se, ev) => { Assert.AreEqual(ev.PropertyName, "MyProperty"); };

            myClass.MyProperty = "string";
            Thread.Sleep(1000);
        }

        [TestMethod]
        public void MyPropertyByNameTest()
        {
            var myClass = new ModelClass();

            myClass.PropertyChanged += (se, ev) => { Assert.AreEqual(ev.PropertyName, "MyPropertyByName"); };

            myClass.MyPropertyByName = "string";
            Thread.Sleep(1000);
        }

        [TestMethod]
        public void MyPropertyAutoChangeTest()
        {
            var myClass = new ModelClass();

            myClass.PropertyChanged += (se, ev) => { Assert.AreEqual(ev.PropertyName, "MyPropertyAutoChange"); };

            myClass.MyPropertyAutoChange = "string";
            Thread.Sleep(1000);
        }

        [TestMethod]
        public void MyCommandTest()
        {
            var myClass = new ModelClass();

            myClass.MyCommandProperty = "1";
            Assert.AreEqual(myClass.MyCommand.CanExecute(null), true);
            myClass.MyCommand.Execute("1");
            Assert.AreEqual(myClass.MyPropertyByName, "1");

            myClass.MyCommandProperty = "2";
            Assert.AreEqual(myClass.MyCommand.CanExecute(null), false);
            myClass.MyCommand.Execute("2");
            Assert.AreEqual(myClass.MyPropertyByName, "2");
        }
    }

    public class ModelClass : BaseViewModel
    {
        private string myCommandProperty;
        private string myProperty;
        private string myPropertyAutoChange;
        private string myPropertyByName;

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

        public ICommand MyCommand
        {
            get
            {
                return new DelegateCommand(executedParam => { MyPropertyByName = (string)executedParam; },
                    canExecutedParam => MyCommandProperty == "1");
            }
        }
    }
}