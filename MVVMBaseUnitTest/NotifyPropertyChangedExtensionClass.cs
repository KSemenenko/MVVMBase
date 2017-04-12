using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMBase;

namespace MVVMBaseUnitTest
{
    class NotifyPropertyChangedExtensionClass : INotifyPropertyChanged
    {
        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        private string property1;

        public string Poperty1
        {
            get { return property1; }
            set
            {
                property1 = value;
                this.RaiseOnPropertyChanged(PropertyChanged);
            }
        }

        private string property2;

        public string Poperty2
        {
            get { return property2; }
            set
            {
                property2 = value;
                this.RaiseOnPropertyChanged(PropertyChanged, nameof(Poperty2));
            }
        }

        public string Poperty3 { get; set; }

    }
}
