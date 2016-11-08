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
        private Dictionary<string, List<string>> bindDictionary = new Dictionary<string, List<string>>(); 

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
            Volatile.Read(ref PropertyChanged)?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        /// <summary>
        /// Associates OnPropertyChanged event to properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">Main property</param>
        /// <param name="actions">Related properties</param>
        public void BindToPropertyChange<T>(Expression<Func<T>> action, params Expression<Func<T>>[] actions)
        {
            var stringAction = GetPropertyName(action);
            var stringActions = new List<string>(actions.Length);
            stringActions.AddRange(actions.Select(GetPropertyName));
            BindToPropertyChange(stringAction, stringActions.ToArray());
        }

        /// <summary>
        /// Associates OnPropertyChanged event to properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">Main property</param>
        /// <param name="actions">Related properties</param>
        public void BindToPropertyChange(string action, params string[] actions)
        {
            List<string> lst;
            if(bindDictionary.TryGetValue(action, out lst))
            {
                lst.AddRange(actions);
                bindDictionary[action] = new List<string>(lst.Distinct());
            }
            else
            {
                bindDictionary.Add(action, new List<string>(actions));
            }
        }

        /// <summary>
        /// Associates OnPropertyChanged event to properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">Main property</param>
        /// <param name="actions">Related properties</param>
        public void BindToPropertyChange<T>(Expression<Func<T>> action, params string[] actions)
        {
            var stringAction = GetPropertyName(action);
            BindToPropertyChange(stringAction, actions);
        }

        /// <summary>
        /// Associates OnPropertyChanged event to properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">Main property</param>
        /// <param name="actions">Related properties</param>
        public void BindToPropertyChange<T>(string action, params Expression<Func<T>>[] actions)
        {
            var stringActions = new List<string>(actions.Length);
            stringActions.AddRange(actions.Select(GetPropertyName));
            BindToPropertyChange(action, stringActions.ToArray());
        }
    }
}