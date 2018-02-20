using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MVVMBase
{
    public partial class NotifyObject : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private Dictionary<string, object> _storage = new Dictionary<string, object>();
        private Dictionary<string, List<string>> _dependencyDictionary = new Dictionary<string, List<string>>();
        private Dictionary<object, string> _collectionDependencies = new Dictionary<object, string>();
        private string _dependencyPropertyName;

        private Action<Action> _runOnUiThread;

        partial void SetUiThread();
        /// <summary>
        ///     Create NotifyObject
        /// </summary>
        public NotifyObject() : this(false)
        {

        }

        /// <summary>
        ///     Create NotifyObject
        /// </summary>
        public NotifyObject(bool resolvePropertyAttributes)
        {
            SetUiThread();
            //_runOnUiThread = Device.BeginInvokeOnMainThread;

            // Update property dependencies
            if (resolvePropertyAttributes)
            {
                ResolvePropertyAttributes();
            }
            
        }

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        ~NotifyObject()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            _runOnUiThread = null;
            _storage = null;
            _dependencyDictionary = null;
            _collectionDependencies = null;
            PropertyChanged = null;
        }

        private void CallPropertyChangedEvent(string propertyName)
        {
            _runOnUiThread(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
        }

        /// <summary>
        ///     Run Action on UI Thread
        /// </summary>
        /// <param name="action">Action</param>
        public void RunOnUiThread(Action action)
        {
            if (_runOnUiThread != null)
            {
                _runOnUiThread(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        ///     Run Action on UI Thread
        /// </summary>
        /// <param name="action">Action</param>
        public Task RunOnUiThreadAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();

            _runOnUiThread(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }


        /// <summary>
        ///     Run Action on UI Thread
        /// </summary>
        /// <param name="func">Function</param>
        public Task<T> RunOnUiThreadAsync<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();

            _runOnUiThread(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }

        /// <summary>
        ///     Run Action on UI Thread
        /// </summary>
        public Task<T> RunOnUiThreadAsync<T>(Task<T> task)
        {
            var tcs = new TaskCompletionSource<T>();

            _runOnUiThread(async () =>
            {
                try
                {
                    tcs.SetResult(await task);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }

        /// <summary>
        /// Run Action on UI Thread
        /// </summary>
        /// <param name="action">Action</param>
        public Task RunOnUiThreadAsync(Task task)
        {
            var tcs = new TaskCompletionSource<bool>();

            _runOnUiThread(async () =>
            {
                try
                {
                    await task;
                    tcs.SetResult(true);

                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }

        /// <summary>
        ///     Changes the property and call the PropertyChanged event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">Action With Propery Name like ()=>MyPropertyName</param>
        protected void OnPropertyChanged<T>(Expression<Func<T>> action)
        {
            string propertyName = GetPropertyName(action);
            OnPropertyChanged(propertyName);
        }

        private static string GetPropertyName<T>(Expression<Func<T>> action)
        {
            var expression = (MemberExpression) action.Body;
            string propertyName = expression.Member.Name;
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
            if (_dependencyDictionary.TryGetValue(propertyName, out List<string> lst))
            {
                foreach (string item in lst)
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
            foreach (string name in propertyNames)
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
        ///     Sets a value in viewmodel storage and raises property changed if value has changed
        /// </summary>
        /// <param name="propertyName">Name.</param>
        /// <param name="value">Value.</param>
        /// <typeparam name="TValueType">The 1st type parameter.</typeparam>
        protected bool SetValueOnUiThread<TValueType>(TValueType value, [CallerMemberName] string propertyName = null)
        {
            SetObjectForKey(propertyName, value);
            RunOnUiThread(()=>OnPropertyChanged(propertyName));
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
            if (string.IsNullOrEmpty(propertyName))
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
            if(_storage == null)
            {
                return;
            }

            if (_storage.ContainsKey(key))
            {
                object existingValue = _storage[key];
                if (existingValue != null && existingValue is INotifyCollectionChanged)
                {
                    (existingValue as INotifyCollectionChanged).CollectionChanged -= HandleNotifyCollectionChangedEventHandler;
                    if (_collectionDependencies.ContainsKey(existingValue))
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

            if (value != null && value is INotifyCollectionChanged)
            {
                _collectionDependencies.Add(value, key);
                (value as INotifyCollectionChanged).CollectionChanged += HandleNotifyCollectionChangedEventHandler;
            }
        }

        protected T GetObjectForKey<T>(string key, T defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("key");
            }

            if(_storage == null)
            {
                return defaultValue;
            }

            if (!_storage.ContainsKey(key))
            {
                if (defaultValue == null)
                {
                    return defaultValue;
                }

                SetObjectForKey(key, defaultValue);
            }

            return (T) _storage[key];
        }

        /// <summary>
        ///     Resolve Property Attributes
        /// </summary>
        protected void ResolvePropertyAttributes()
        {
            foreach (PropertyInfo dependantPropertyInfo in ReflectionExtensions.GetProperties(GetType()))
            {
                // Check for NotifyAttribute
                var notifyAttribute = dependantPropertyInfo.GetCustomAttribute<NotifyPropertyAttribute>();
                if (notifyAttribute != null)
                {
                    foreach (string property in notifyAttribute.SourceProperties)
                    {
                        ChangedObjectNotifyPropertyChange(dependantPropertyInfo.Name, property);
                    }
                }

                // Check for DependsOnAttribute
                var dependsOnAttribute = dependantPropertyInfo.GetCustomAttribute<DependsOnPropertyAttribute>();
                if (dependsOnAttribute != null)
                {
                    foreach (string property in dependsOnAttribute.SourceProperties)
                    {
                        ChangedObjectNotifyPropertyChange(property, dependantPropertyInfo.Name);
                    }
                }
            }
        }

        /// <summary>
        ///     Handles the notify collection changed event handler.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void HandleNotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is INotifyCollectionChanged && _collectionDependencies.ContainsKey(sender))
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
        public NotifyObject ChangedObject(string propertyName)
        {
            _dependencyPropertyName = propertyName;
            return this;
        }

        /// <summary>
        ///     Set property to ChangedObject.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public NotifyObject ChangedObject<T>(Expression<Func<T>> propertyName)
        {
            _dependencyPropertyName = GetPropertyName(propertyName);
            return this;
        }

        /// <summary>
        ///     Depends to property.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public NotifyObject Notify(string propertyName)
        {
            if (string.IsNullOrEmpty(_dependencyPropertyName))
            {
                throw new ArgumentException("Dependency PropertyNamenot set.");
            }

            ChangedObjectNotifyPropertyChange(_dependencyPropertyName, propertyName);
            return this;
        }

        /// <summary>
        ///     Depends to property.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public NotifyObject Notify<T>(Expression<Func<T>> propertyName)
        {
            if (string.IsNullOrEmpty(_dependencyPropertyName))
            {
                throw new ArgumentException("Depends property not set.");
            }

            ChangedObjectNotifyPropertyChange(_dependencyPropertyName, GetPropertyName(propertyName));
            return this;
        }

        /// <summary>
        ///     Associates OnPropertyChanged event to properties.
        /// </summary>
        /// <param name="propertyName">Main property</param>
        /// <param name="actions">Related properties</param>
        public void ChangedObjectNotifyPropertyChange(string propertyName, params string[] actions)
        {
            if (_dependencyDictionary.TryGetValue(propertyName, out List<string> list))
            {
                _dependencyDictionary[propertyName] = new List<string>(list.Union(actions));
            }
            else
            {
                _dependencyDictionary.Add(propertyName, new List<string>(actions));
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
            string stringAction = GetPropertyName(propertyName);
            ChangedObjectNotifyPropertyChange(stringAction, propertyNames);
        }

        #endregion
    }
}