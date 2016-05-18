using System;
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
            ModelClass myClass = new ModelClass();

            myClass.PropertyChanged += (se, ev) =>
                                       {
                                           Assert.AreEqual(ev.PropertyName, "MyProperty"); 
                                       };

            myClass.MyProperty = "string";
            System.Threading.Thread.Sleep(1000);
        }

        [TestMethod]
        public void MyPropertyByNameTest()
        {
            ModelClass myClass = new ModelClass();

            myClass.PropertyChanged += (se, ev) =>
            {
                Assert.AreEqual(ev.PropertyName, "MyPropertyByName");
            };

            myClass.MyPropertyByName = "string";
            System.Threading.Thread.Sleep(1000);
        }

        [TestMethod]
        public void MyPropertyAutoChangeTest()
        {
            ModelClass myClass = new ModelClass();

            myClass.PropertyChanged += (se, ev) =>
            {
                Assert.AreEqual(ev.PropertyName, "MyPropertyAutoChange");
            };

            myClass.MyPropertyAutoChange = "string";
            System.Threading.Thread.Sleep(1000);
        }

        [TestMethod]
        public void MyCommandTest()
        {
            ModelClass myClass = new ModelClass();

            myClass.MyProperty = "1";
            Assert.AreEqual(myClass.MyCommand.CanExecute(null), true);
            myClass.MyCommand.Execute("1");
            Assert.AreEqual(myClass.MyPropertyByName, "1");

            myClass.MyProperty = "2";
            Assert.AreEqual(myClass.MyCommand.CanExecute(null), false);
            myClass.MyCommand.Execute("2");
            Assert.AreEqual(myClass.MyPropertyByName, "2");
        }


    }

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
                OnPropertyChanged(() => MyCommand);
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
                                               MyPropertyByName = (string)executedParam;
                                           },
                canExecutedParam => MyProperty == "1");
            }
        }
    }

}
