using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MVVMBase
{
    /// <summary>
    ///     Base View Model
    /// </summary>
    public partial class BaseViewModel : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> _storage = new Dictionary<string, object>();
        private readonly Dictionary<string, List<string>> dependencyDictionary = new Dictionary<string, List<string>>();
        readonly Dictionary<object, string> _collectionDependencies = new Dictionary<object, string>();
        private string dependencyPropertyName;

        private Action<Action> runOnUiThread;

        /// <summary>
        ///     Create BaseViewModel
        /// </summary>
        public BaseViewModel()
        {
            SetUiThread();
            // Update property dependencies
            ResolvePropertyAttribute();
        }

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        partial void SetUiThread();

        private void CallPropertyChangedEvent(string propertyName)
        {
            if(runOnUiThread != null)
            {
                runOnUiThread(() => Volatile.Read(ref PropertyChanged)?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
            }
            else
            {
                Volatile.Read(ref PropertyChanged)?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
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
            if(dependencyDictionary.TryGetValue(propertyName, out var lst))
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
            foreach(var item in GetType().GetTypeInfo().DeclaredProperties)
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
        ///     Sets a value in viewmodel storage and raises property changed if value has changed
        /// </summary>
        /// <param name="propertyName">Name.</param>
        /// <param name="value">Value.</param>
        /// <typeparam name="TValueType">The 1st type parameter.</typeparam>
        protected bool SetValue<TValueType>(TValueType value, [CallerMemberName] string propertyName = null)
        {
            SetObjectForKey(propertyName, value);
            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        ///     Returns a value from the viewmodel storage
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="property">Name.</param>
        /// <typeparam name="TValueType">The 1st type parameter.</typeparam>
        protected TValueType GetValue<TValueType>([CallerMemberName] string property = null)
        {
            return GetValue(() => default(TValueType), property);
        }

        /// <summary>
        ///     Returns a value from the viewmodel storage
        /// </summary>
        protected TValueType GetValue<TValueType>(Func<TValueType> defaultValueFunc, [CallerMemberName] string propertyName = null)
        {
            if(string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("propertyName");
            }

            return GetObjectForKey(propertyName, defaultValueFunc());
        }

        /// <summary>
        ///     Sets the object for key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected void SetObjectForKey<T>(string key, T value)
        {
            if(_storage.ContainsKey(key))
            {
                var existingValue = _storage[key];
                if(existingValue != null && existingValue is INotifyCollectionChanged)
                {
                    (existingValue as INotifyCollectionChanged).CollectionChanged -= HandleNotifyCollectionChangedEventHandler;
                    if(_collectionDependencies.ContainsKey(existingValue))
                    {
                        _collectionDependencies.Remove(existingValue);
                    }
                        
                }

                _storage[key] = value;
            }
            else
            {
                _storage.Add(key, value);
            }

            if(value != null && value is INotifyCollectionChanged)
            {
                _collectionDependencies.Add(value, key);
                (value as INotifyCollectionChanged).CollectionChanged += HandleNotifyCollectionChangedEventHandler;
            }

            //if (_storage.ContainsKey(key))
            //{
            //    _storage[key] = value;
            //}
            //else
            //{
            //    _storage.Add(key, value);
            //}
        }

        /// <summary>
        ///     Gets the object for key.
        /// </summary>
        /// <returns>The object for key.</returns>
        /// <param name="key">Key.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected T GetObjectForKey<T>(string key, T defaultValue)
        {
            if(string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("key");
            }

            if(!_storage.ContainsKey(key))
            {
                if(defaultValue == null)
                {
                    return defaultValue;
                }

                SetObjectForKey(key, defaultValue);
            }

            try
            {
                return (T)Convert.ChangeType(_storage[key], typeof(T));
            }
            catch
            {
                return (T)_storage[key];
            }
        }

        private void ResolvePropertyAttribute()
        {
            foreach(var dependantPropertyInfo in GetType().GetRuntimeProperties())
            {
                // Check for NotifyAttribute
                var notifyAttribute = dependantPropertyInfo.GetCustomAttribute<NotifyAttribute>();
                if(notifyAttribute != null)
                {
                    foreach(var property in notifyAttribute.SourceProperties)
                    {
                        ChangedObject(dependantPropertyInfo.Name).Notify(property);
                    }
                }

                // Check for DependsOnAttribute
                var dependsOnAttribute = dependantPropertyInfo.GetCustomAttribute<DependsOnAttribute>();
                if(dependsOnAttribute != null)
                {
                    foreach(var property in dependsOnAttribute.SourceProperties)
                    {
                        ChangedObject(property).Notify(dependantPropertyInfo.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the notify collection changed event handler.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void HandleNotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(sender is INotifyCollectionChanged && _collectionDependencies.ContainsKey(sender))
            {
                OnPropertyChanged(_collectionDependencies[sender]);
            }
        }

        /// <summary>
        ///     Raise the property and call the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        #region

        /// <summary>
        ///     Set property to ChangedObject.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public BaseViewModel ChangedObject(string propertyName)
        {
            dependencyPropertyName = propertyName;
            return this;
        }

        /// <summary>
        ///     Set property to ChangedObject.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public BaseViewModel ChangedObject<T>(Expression<Func<T>> propertyName)
        {
            dependencyPropertyName = GetPropertyName(propertyName);
            return this;
        }

        /// <summary>
        ///     Depends to property.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public BaseViewModel Notify(string propertyName)
        {
            if(string.IsNullOrEmpty(dependencyPropertyName))
            {
                throw new ArgumentException("Dependency PropertyNamenot set.");
            }

            ChangedObjectNotifyPropertyChange(dependencyPropertyName, propertyName);
            return this;
        }

        /// <summary>
        ///     Depends to property.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public BaseViewModel Notify<T>(Expression<Func<T>> propertyName)
        {
            if(string.IsNullOrEmpty(dependencyPropertyName))
            {
                throw new ArgumentException("Depends property not set.");
            }

            ChangedObjectNotifyPropertyChange(dependencyPropertyName, GetPropertyName(propertyName));
            return this;
        }

        /// <summary>
        ///     Associates OnPropertyChanged event to properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Main property</param>
        /// <param name="actions">Related properties</param>
        public void ChangedObjectNotifyPropertyChange(string propertyName, params string[] actions)
        {
            List<string> list;
            if(dependencyDictionary.TryGetValue(propertyName, out list))
            {
                list.AddRange(actions);
                dependencyDictionary[propertyName] = new List<string>(list.Distinct());
            }
            else
            {
                dependencyDictionary.Add(propertyName, new List<string>(actions));
            }
        }

        /// <summary>
        ///     Associates OnPropertyChanged event to properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Main property</param>
        /// <param name="propertyNames">Related properties</param>
        public void ChangedObjectNotifyPropertyChange<T>(Expression<Func<T>> propertyName, params string[] propertyNames)
        {
            var stringAction = GetPropertyName(propertyName);
            ChangedObjectNotifyPropertyChange(stringAction, propertyNames);
        }

        #endregion
    }
}