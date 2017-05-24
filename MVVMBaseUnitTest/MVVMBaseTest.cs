using System.Collections.Generic;
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
            Assert.AreEqual(count, 6); // 4 properties and 2 commands
        }

        [TestMethod]
        public void BindToPropertyChangeTest()
        {
            var myClass = new BindClass();
            List<string> propertyList = new List<string>();
            myClass.PropertyChanged += (se, ev) => {propertyList.Add(ev.PropertyName);};

            myClass.BindToPropertyChange(nameof(myClass.MyProperty), nameof(myClass.MyCommand));
            myClass.BindToPropertyChange(() => myClass.MyCommand, nameof(myClass.MyPropertyByName));

            myClass.MyProperty = "string";

            Thread.Sleep(1000);
            
            Assert.AreEqual(propertyList.Count,3);
        }

        [TestMethod]
        public void BindToTest()
        {
            var myClass = new BindClass();
            List<string> propertyList = new List<string>();
            myClass.PropertyChanged += (se, ev) => { propertyList.Add(ev.PropertyName); };

            myClass.Bind(nameof(myClass.MyProperty)).To(nameof(myClass.MyCommand));

            myClass.Bind(() => myClass.MyCommand).To(()=> myClass.MyPropertyByName);

            myClass.MyProperty = "string";

            Thread.Sleep(1000);

            Assert.AreEqual(propertyList.Count, 3);
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