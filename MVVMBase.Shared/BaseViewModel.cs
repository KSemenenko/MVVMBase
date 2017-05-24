using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Reflection;

namespace MVVMBase
{
    /// <summary>
    ///     Base View Model
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        private readonly Dictionary<string, List<string>> bindDictionary = new Dictionary<string, List<string>>();
        private string bindPropertyName;

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        private void CallPropertyChangedEvent(string propertyName)
        {
            Volatile.Read(ref PropertyChanged)?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Changes the property and call the PropertyChanged event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">Action With Propery Name like ()=>MyPropertyName</param>
        protected void OnPropertyChanged<T>(Expression<Func<T>> action)
        {
            var propertyName = GetPropertyName(action);
            OnPropertyChanged(propertyName);

        }

        private static string GetPropertyName<T>(Expression<Func<T>> action)
        {
            var expression = (MemberExpression)action.Body;
            var propertyName = expression.Member.Name;
            return propertyName;
        }

        /// <summary>
        ///     Changes the property and call the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void OnPropertyChanged(string propertyName)
        {
            CallPropertyChangedEvent(propertyName);

            //bind properties
            List<string> lst;
            if (bindDictionary.TryGetValue(propertyName, out lst))
            {
                foreach(var item in lst)
                {
                    OnPropertyChanged(item);
                }
            }
        }

        /// <summary>
        ///     Changes the property and call the PropertyChanged event.
        /// </summary>
        /// <param name="propertyNames">Array of Property name</param>
        protected void OnPropertyChanged(params string[] propertyNames)
        {
            foreach(var name in propertyNames)
            {
                OnPropertyChanged(name);
            }
            
        }

        /// <summary>
        ///     Changes the property and call the PropertyChanged event for this property name.
        /// </summary>
        [SuppressMessage("ReSharper", "MethodOverloadWithOptionalParameter")]
        protected void OnPropertyChanged([CallerMemberName] string hiddenPropertyName = null, bool hiddenCallMemberName = true)
        {
            OnPropertyChanged(hiddenPropertyName);
        }

        /// <summary>
        ///     Call OnPropertyChanged for all properties
        /// </summary>
        protected void OnPropertyChangedForAll()
        {
            foreach (PropertyInfo item in GetType().GetTypeInfo().DeclaredProperties)
            {
                OnPropertyChanged(item.Name);
            }
        }

        /// <summary>
        ///     Changes the property and call the PropertyChanged event.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <param name="storage">Reference to current value.</param>
        /// <param name="value">New value to be set.</param>
        /// <param name="propertyName">The name of the property to raise the PropertyChanged event for.</param>
        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            storage = value;
            OnPropertyChanged(propertyName);
        }



        public BaseViewModel Bind(string propertyName)
        {
            bindPropertyName = propertyName;
            return this;
        }

        public BaseViewModel Bind<T>(Expression<Func<T>> propertyName)
        {
            bindPropertyName = GetPropertyName(propertyName);
            return this;
        }

        public BaseViewModel To(string propertyName)
        {
            if (string.IsNullOrEmpty(bindPropertyName))
            {
                throw new ArgumentException("Bind property not set.");
            }

            BindToPropertyChange(bindPropertyName, propertyName);
            return this;
        }

        public BaseViewModel To<T>(Expression<Func<T>> propertyName)
        {
            if (string.IsNullOrEmpty(bindPropertyName))
            {
                throw new ArgumentException("Bind property not set.");
            }

            BindToPropertyChange(bindPropertyName, GetPropertyName(propertyName));
            return this;
        }

        /// <summary>
        /// Associates OnPropertyChanged event to properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Main property</param>
        /// <param name="actions">Related properties</param>
        public void BindToPropertyChange(string propertyName, params string[] actions)
        {
            List<string> list;
            if(bindDictionary.TryGetValue(propertyName, out list))
            {
                list.AddRange(actions);
                bindDictionary[propertyName] = new List<string>(list.Distinct());
            }
            else
            {
                bindDictionary.Add(propertyName, new List<string>(actions));
            }
        }

        /// <summary>
        /// Associates OnPropertyChanged event to properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Main property</param>
        /// <param name="propertyNames">Related properties</param>
        public void BindToPropertyChange<T>(Expression<Func<T>> propertyName, params string[] propertyNames)
        {
            var stringAction = GetPropertyName(propertyName);
            BindToPropertyChange(stringAction, propertyNames);
        }

        /// <summary>
        ///     Raise the property and call the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

    }
}