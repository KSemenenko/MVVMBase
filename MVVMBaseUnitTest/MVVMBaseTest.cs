using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVMBase;
using MVVMBase.Extensions;

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
        public void MyPropertyAutoGetSetTest()
        {
            var myClass = new ModelClass();

            myClass.PropertyChanged += (se, ev) => { Assert.AreEqual(ev.PropertyName, "MyPropertyAutoGetSet"); };

            myClass.MyPropertyAutoGetSet = "string";
            Thread.Sleep(1000);
            Assert.AreEqual(myClass.MyPropertyAutoGetSet, "string");
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

            myClass.MyCommandProperty = "1";
            myClass.MyCommand.Run("3");
            Assert.AreEqual(myClass.MyPropertyByName, "3");

            myClass.MyCommandProperty = "2";
            myClass.MyCommand.Run("4");
            Assert.AreEqual(myClass.MyPropertyByName, "3");
        }

        [TestMethod]
        public void MyAsyncCommandTest()
        {
            var myClass = new ModelClass();

            myClass.MyCommandProperty = "1";
            Assert.AreEqual(myClass.MyAsyncCommand.CanExecute(null), true);
            myClass.MyCommand.Execute("1");
            Assert.AreEqual(myClass.MyPropertyByName, "1");

            myClass.MyCommandProperty = "2";
            Assert.AreEqual(myClass.MyAsyncCommand.CanExecute(null), false);
            myClass.MyCommand.Execute("2");
            Assert.AreEqual(myClass.MyPropertyByName, "2");

            myClass.MyCommandProperty = "1";
            myClass.MyCommand.Run("3");
            Assert.AreEqual(myClass.MyPropertyByName, "3");

            myClass.MyCommandProperty = "2";
            myClass.MyCommand.Run("4");
            Assert.AreEqual(myClass.MyPropertyByName, "3");
        }

        [TestMethod]
        public void AllOnPropertyChanged()
        {
            var myClass = new ModelClass();
            int count = 0;

            myClass.PropertyChanged += (se, ev) => Interlocked.Increment(ref count);

            myClass.UpdateAll();

            Thread.Sleep(1000);
            Assert.AreEqual(count, 8); // 5 properties and 3 commands
        }

        [TestMethod]
        public void ChangedObjectNotifyPropertyChangeTest()
        {
            var myClass = new BindClass();
            List<string> propertyList = new List<string>();
            myClass.PropertyChanged += (se, ev) => {propertyList.Add(ev.PropertyName);};

            myClass.ChangedObjectNotifyPropertyChange(nameof(myClass.MyProperty), nameof(myClass.MyCommand));
            myClass.ChangedObjectNotifyPropertyChange(() => myClass.MyCommand, nameof(myClass.MyPropertyByName));

            myClass.MyProperty = "string";

            Thread.Sleep(1000);
            
            Assert.AreEqual(propertyList.Count,3);
        }

        [TestMethod]
        public void DependsToTest()
        {
            var myClass = new BindClass();
            List<string> propertyList = new List<string>();
            myClass.PropertyChanged += (se, ev) => { propertyList.Add(ev.PropertyName); };

            myClass.ChangedObject(nameof(myClass.MyProperty)).Notify(nameof(myClass.MyCommand));

            myClass.ChangedObject(() => myClass.MyCommand).Notify(()=> myClass.MyPropertyByName);

            myClass.MyProperty = "string";

            Thread.Sleep(1000);

            Assert.AreEqual(propertyList.Count, 3);
        }

        [TestMethod]
        public void NotifyToTest()
        {
            var myClass = new BindClass();
            List<string> propertyList = new List<string>();
            myClass.PropertyChanged += (se, ev) => { propertyList.Add(ev.PropertyName); };

            myClass.ChangedObject(nameof(myClass.MyProperty)).Notify(nameof(myClass.MyCommand));

            myClass.ChangedObject(() => myClass.MyCommand).Notify(() => myClass.MyPropertyByName);

            myClass.MyProperty = "string";

            Thread.Sleep(1000);

            Assert.AreEqual(propertyList.Count, 3);
        }

        [TestMethod]
        public void DependOnAttributeTest()
        {
            var myClass = new BindClass();
            List<string> propertyList = new List<string>();
            myClass.PropertyChanged += (se, ev) =>
            {
                propertyList.Add(ev.PropertyName);
            };
            myClass.MyPropertyIntAutoGetSet = 10; 
            Assert.AreEqual(propertyList.Count, 2);
            Assert.AreEqual(myClass.MyDependedPropertyInt, myClass.MyPropertyIntAutoGetSet * myClass.MyPropertyIntAutoGetSet);
            
        }

        [TestMethod]
        public void ObservableTest()
        {
            var myClass = new BindClass();
            List<string> propertyList = new List<string>();
            myClass.PropertyChanged += (se, ev) =>
            {
                propertyList.Add(ev.PropertyName);
            };
            myClass.MyObservableProperty = new ObservableCollection<int>();
            myClass.MyObservableProperty.Add(1);
            myClass.MyObservableProperty.Add(2);
            Assert.AreEqual(propertyList.Count, 6);
            Assert.AreEqual(myClass.MyObservablePropertyCount, myClass.MyObservableProperty.Sum());

        }

        [TestMethod]
        public void NotifyPropertyChangedExtensionText()
        {
            var myClass1 = new NotifyPropertyChangedExtensionClass();
            myClass1.PropertyChanged += (se, ev) => { Assert.AreEqual(ev.PropertyName, "Poperty1"); };
            myClass1.Poperty1 = "string";

            var myClass2 = new NotifyPropertyChangedExtensionClass();
            myClass2.PropertyChanged += (se, ev) => { Assert.AreEqual(ev.PropertyName, "Poperty2"); };
            myClass2.Poperty2 = "string";

            Thread.Sleep(1000);
            Assert.AreEqual(myClass1.Poperty1, "string");
            Assert.AreEqual(myClass2.Poperty2, "string");
        }
    }
}