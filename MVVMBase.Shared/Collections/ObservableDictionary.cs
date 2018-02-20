using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace MVVMBase.Collections
{
    /// <summary>
    ///     This is a Dictionary that supports INotifyCollectionChanged semantics.
    /// </summary>
    /// <remarks>
    ///     WARNING: this dictionary is NOT thread-safe!  You must still
    ///     provide synchronization to ensure no writes are done while the dictionary is being
    ///     enumerated!  This should not be a problem for most bindings as they rely on the
    ///     CollectionChanged information.
    /// </remarks>
    /// <typeparam name="TKey">Key</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    [DebuggerDisplay("Count={Count}")]
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        // Internal dictionary that holds values
        private readonly IDictionary<TKey, TValue> underlyingDictionary;
        private bool shouldRaiseNotifications;

        /// <summary>
        ///     Constructor
        /// </summary>
        public ObservableDictionary() : this(new Dictionary<TKey, TValue>())
        {
        }

        /// <summary>
        ///     Constructor that allows different storage initialization
        /// </summary>
        public ObservableDictionary(IDictionary<TKey, TValue> store)
        {
            if(store == null)
            {
                throw new ArgumentNullException("store");
            }

            underlyingDictionary = store;
        }

        /// <summary>
        ///     Constructor that takes an equality comparer
        /// </summary>
        /// <param name="comparer">Comparison class</param>
        public ObservableDictionary(IEqualityComparer<TKey> comparer) : this(new Dictionary<TKey, TValue>(comparer))
        {
        }

        /// <summary>
        ///     Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">
        ///     The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </param>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </exception>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            underlyingDictionary.Add(item);
            OnNotifyAdd(item);
        }

        /// <summary>
        ///     Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </exception>
        public void Clear()
        {
            underlyingDictionary.Clear();
            OnNotifyReset();
        }

        /// <summary>
        ///     Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an
        ///     <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from
        ///     <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based
        ///     indexing.
        /// </param>
        /// <param name="arrayIndex">
        ///     The zero-based index in <paramref name="array" /> at which copying begins.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex" /> is less than 0.</exception>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            underlyingDictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     Removes the first occurrence of a specific object from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>
        ///     true if <paramref name="item" /> was successfully removed from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if
        ///     <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        /// <param name="item">
        ///     The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </param>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </exception>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            bool flag = underlyingDictionary.Remove(item);
            if(flag)
            {
                OnNotifyRemove(item);
            }

            return flag;
        }

        /// <summary>
        ///     Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <returns>
        ///     true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />;
        ///     otherwise, false.
        /// </returns>
        /// <param name="item">
        ///     The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </param>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return underlyingDictionary.Contains(item);
        }

        /// <summary>
        ///     Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>
        ///     The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public int Count
        {
            get => underlyingDictionary.Count;
        }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>
        ///     true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get => false;
        }

        /// <summary>
        ///     Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">
        ///     The object to use as the key of the element to add.
        /// </param>
        /// <param name="value">
        ///     The object to use as the value of the element to add.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="key" /> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.IDictionary`2" /> is read-only.
        /// </exception>
        public void Add(TKey key, TValue value)
        {
            var item = new KeyValuePair<TKey, TValue>(key, value);
            underlyingDictionary.Add(item);
            OnNotifyAdd(item);
        }

        /// <summary>
        ///     Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the
        ///     specified key.
        /// </summary>
        /// <returns>
        ///     true if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise,
        ///     false.
        /// </returns>
        /// <param name="key">
        ///     The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="key" /> is null.
        /// </exception>
        public bool ContainsKey(TKey key)
        {
            return underlyingDictionary.ContainsKey(key);
        }

        /// <summary>
        ///     Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>
        ///     true if the element is successfully removed; otherwise, false.  This method also returns false if
        ///     <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        /// <param name="key">
        ///     The key of the element to remove.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="key" /> is null.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.IDictionary`2" /> is read-only.
        /// </exception>
        public bool Remove(TKey key)
        {
            TValue local = underlyingDictionary[key];
            bool flag = underlyingDictionary.Remove(key);
            OnNotifyRemove(new KeyValuePair<TKey, TValue>(key, local));

            return flag;
        }

        /// <summary>
        ///     Gets the value associated with the specified key.
        /// </summary>
        /// <returns>
        ///     true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element
        ///     with the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">
        ///     The key whose value to get.
        /// </param>
        /// <param name="value">
        ///     When this method returns, the value associated with the specified key, if the key is found; otherwise, the default
        ///     value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="key" /> is null.
        /// </exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return underlyingDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        ///     Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>
        ///     The element with the specified key.
        /// </returns>
        /// <param name="key">
        ///     The key of the element to get or set.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="key" /> is null.
        /// </exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="key" /> is not found.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2" /> is read-only.
        /// </exception>
        public TValue this[TKey key]
        {
            get => underlyingDictionary[key];
            set
            {
                if(underlyingDictionary.ContainsKey(key))
                {
                    TValue originalValue = underlyingDictionary[key];
                    underlyingDictionary[key] = value;
                    OnNotifyReplace(new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, originalValue));
                }
                else
                {
                    underlyingDictionary[key] = value;
                    OnNotifyAdd(new KeyValuePair<TKey, TValue>(key, value));
                }
            }
        }

        /// <summary>
        ///     Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the object that implements
        ///     <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        public ICollection<TKey> Keys
        {
            get => underlyingDictionary.Keys;
        }

        /// <summary>
        ///     Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the object that implements
        ///     <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        public ICollection<TValue> Values
        {
            get => underlyingDictionary.Values;
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return underlyingDictionary.GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return underlyingDictionary.GetEnumerator();
        }

        /// <summary>
        ///     Event raised for collection change notification
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        ///     Event raise for property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     This method turns off notifications until the returned object
        ///     is Disposed. At that point, the entire dictionary is invalidated.
        /// </summary>
        /// <returns>IDisposable</returns>
        public IDisposable BeginMassUpdate()
        {
            return new MassUpdater(this);
        }

        /// <summary>
        ///     This is used to notify insertions into the dictionary.
        /// </summary>
        /// <param name="item">Item</param>
        protected void OnNotifyAdd(KeyValuePair<TKey, TValue> item)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs(item.Key.ToString()));
        }

        /// <summary>
        ///     This is used to notify removals from the dictionary
        /// </summary>
        /// <param name="item">Item</param>
        protected void OnNotifyRemove(KeyValuePair<TKey, TValue> item)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs(item.Key.ToString()));
        }

        /// <summary>
        ///     This is used to notify replacements in the dictionary
        /// </summary>
        /// <param name="newItem">New item</param>
        /// <param name="oldItem">Old item</param>
        protected void OnNotifyReplace(KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem));
            OnPropertyChanged(new PropertyChangedEventArgs(oldItem.Key.ToString()));
        }

        /// <summary>
        ///     This is used to notify that the dictionary was completely reset.
        /// </summary>
        protected void OnNotifyReset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged" /> event with the
        ///     provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if(shouldRaiseNotifications == false)
            {
                return;
            }

            CollectionChanged?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the property change notification
        /// </summary>
        /// <param name="e">Property event args.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if(shouldRaiseNotifications == false)
            {
                return;
            }

            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        ///     IDisposable class which turns off updating
        /// </summary>
        private class MassUpdater : IDisposable
        {
            private readonly ObservableDictionary<TKey, TValue> parent;

            public MassUpdater(ObservableDictionary<TKey, TValue> parent)
            {
                this.parent = parent;
                parent.shouldRaiseNotifications = false;
            }

            public void Dispose()
            {
                parent.shouldRaiseNotifications = true;
                parent.OnNotifyReset();
            }

#if DEBUG
            ~MassUpdater()
            {
                Debug.Assert(true, "Did not dispose returned object from ObservableDictionary.BeginMassUpdate!");
            }
#endif
        }
    }
}