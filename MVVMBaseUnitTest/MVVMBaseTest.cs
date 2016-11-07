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
            Assert.AreEqual(myClass.MyProperty, "string");
        }

        [TestMethod]
        public void MyPropertyByMemberNameTest()
        {
            var myClass = new ModelClass();

            myClass.PropertyChanged += (se, ev) => { Assert.AreEqual(ev.PropertyName, "MyPropertyByMemberName"); };

            myClass.MyPropertyByMemberName = "string";
            Thread.Sleep(1000);
            Assert.AreEqual(myClass.MyPropertyByMemberName, "string");
        }

        [TestMethod]
        public void MyPropertyByNameTest()
        {
            var myClass = new ModelClass();

            myClass.PropertyChanged += (se, ev) => { Assert.AreEqual(ev.PropertyName, "MyPropertyByName"); };

            myClass.MyPropertyByName = "string";
            Thread.Sleep(1000);
            Assert.AreEqual(myClass.MyPropertyByName, "string");
        }

        [TestMethod]
        public void MyPropertyAutoChangeTest()
        {
            var myClass = new ModelClass();

            myClass.PropertyChanged += (se, ev) => { Assert.AreEqual(ev.PropertyName, "MyPropertyAutoChange"); };

            myClass.MyPropertyAutoChange = "string";
            Thread.Sleep(1000);
            Assert.AreEqual(myClass.MyPropertyAutoChange, "string");
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

        [TestMethod]
        public void AllOnPropertyChanged()
        {
            var myClass = new ModelClass();
            int count = 0;

            myClass.PropertyChanged += (se, ev) => Interlocked.Increment(ref count);

            myClass.UpdateAll();

            Thread.Sleep(1000);
            Assert.AreEqual(count, 6); // 4 properties and 2 commands
        }
    }

    public class ModelClass : BaseViewModel
    {
        private string myCommandProperty;
        private string myProperty;
        private string myPropertyAutoChange;
        private string myPropertyByName;
        private string myPropertyByMemberName;

        public ModelClass()
        {
            this.BindToPropertyChange(() => MyCommand, () => MyCommand);
            this.BindToPropertyChange("s", "s");
            this.BindToPropertyChange(()=> MyCommand, "s");
            this.BindToPropertyChange("s", () => MyCommand);
            
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