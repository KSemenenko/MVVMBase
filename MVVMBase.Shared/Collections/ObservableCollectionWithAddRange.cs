﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace MVVMBase
{
    public class ObservableCollectionWithAddRange<T> : ObservableCollection<T>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ObservableCollectionWithAddRange()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="items"></param>
        public ObservableCollectionWithAddRange(IEnumerable<T> items) : base(items)
        {

        }
        #endregion

        #region Public Members

        /// <summary>
        /// Adds the range of new items
        /// </summary>
        /// <param name="list"></param>
        public void AddRange(IEnumerable<T> list)
        {
            foreach (var item in list)
                Items.Add(item);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Replaces the range.
        /// </summary>
        /// <param name="list">List.</param>
        public void ReplaceRange(IEnumerable<T> list)
        {
            base.Items.Clear();

            foreach (T current in list)
                base.Items.Add(current);

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, list));
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="list">List.</param>
        public void RemoveRange(IEnumerable<T> list)
        {
            foreach (T current in list)
                base.Items.Remove(current);

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, list));
        }

        /// <summary>
        /// Inserts the range.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="list">List.</param>
        public void InsertRange(int index, IEnumerable<T> list)
        {
            foreach (T current in list)
                base.Items.Insert(index, current);

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, list));
        }
        #endregion
    }
}
