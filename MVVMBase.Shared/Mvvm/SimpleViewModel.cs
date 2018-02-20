using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MVVMBase.Mvvm
{
    /// <summary>
    ///     This is a simple base class for MVVM.
    /// </summary>
    public class SimpleViewModel : INotifyPropertyChanged
    {
        /// <summary>
        ///     Event to raise when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Inform any bindings that ALL property values must be read.
        /// </summary>
        protected void RaiseAllPropertiesChanged()
        {
            // By convention, an empty string indicates all properties are invalid.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }

        /// <summary>
        ///     Raises a specific property change event using an expression.
        /// </summary>
        /// <param name="propExpr">Property expr.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> propExpr)
        {
            var prop = (PropertyInfo)((MemberExpression)propExpr.Body).Member;
            RaisePropertyChanged(prop.Name);
        }

        /// <summary>
        ///     Raises a specific property change event using a string for the property name.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Changes a field's value and raises property change notifications.
        /// </summary>
        /// <returns><c>true</c>, if property value was set, <c>false</c> otherwise.</returns>
        /// <param name="storageField">Storage field.</param>
        /// <param name="newValue">New value.</param>
        /// <param name="propExpr">Property expr.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected bool SetPropertyValue<T>(ref T storageField, T newValue, Expression<Func<T>> propExpr)
        {
            if(Equals(storageField, newValue))
            {
                return false;
            }

            storageField = newValue;
            var prop = (PropertyInfo)((MemberExpression)propExpr.Body).Member;
            RaisePropertyChanged(prop.Name);

            return true;
        }

        /// <summary>
        ///     Changes a field's value and raises property change notifications.
        /// </summary>
        /// <returns><c>true</c>, if property value was set, <c>false</c> otherwise.</returns>
        /// <param name="storageField">Storage field.</param>
        /// <param name="newValue">New value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected bool SetPropertyValue<T>(ref T storageField, T newValue, [CallerMemberName] string propertyName = "")
        {
            if(Equals(storageField, newValue))
            {
                return false;
            }

            storageField = newValue;
            RaisePropertyChanged(propertyName);

            return true;
        }
    }
}