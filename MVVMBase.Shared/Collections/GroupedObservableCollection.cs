using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace MVVMBase.Collections
{
    /// <summary>
    ///     This is a simple observable collection which has a GroupBy key which can
    ///     be used to populate a ListView with grouping turned on
    /// </summary>
    /// <typeparam name="TKey">The type to use for the grouping key</typeparam>
    /// <typeparam name="TValue">The type to use for the items</typeparam>
    [DebuggerDisplay("Count={Count}")]
    public class GroupedObservableCollection<TKey, TValue> : OptimizedObservableCollection<TValue>
    {
        // Data
        private bool hasItems;

        /// <summary>
        ///     Initializes a grouped collection.
        /// </summary>
        public GroupedObservableCollection(TKey key)
        {
            Key = key;
        }

        /// <summary>
        ///     Initializes the grouped collection with a set of items.
        /// </summary>
        /// <param name="key">Grouping key value</param>
        /// <param name="items">Set of items for this group</param>
        public GroupedObservableCollection(TKey key, IEnumerable<TValue> items) : base(items)
        {
            Key = key;
        }

        /// <summary>
        ///     The read-only grouping key.
        /// </summary>
        /// <value>The group title.</value>
        public TKey Key { get; }

        /// <summary>
        ///     Simple property to allow us to collapse a group when it has no items.
        /// </summary>
        /// <value><c>true</c> if has items; otherwise, <c>false</c>.</value>
        public bool HasItems
        {
            get => hasItems;

            set
            {
                if(hasItems != value)
                {
                    hasItems = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(HasItems)));
                }
            }
        }

        /// <summary>
        ///     Handles the PropertyChanged notification. We use this to catch changes
        ///     to the Count and then update the <see cref="HasItems" /> property.
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if(e.PropertyName == nameof(Count))
            {
                HasItems = Count > 0;
            }
        }
    }
}