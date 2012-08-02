// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using TimePunch.Metro.Wpf.Controller;

namespace TimePunch.Metro.Wpf.Controls.CheckedList
{
    public class CheckableList<T, TE> : ObservableCollection<T>, ICheckableList where T : CheckableListItem<TE>
    {
        private int countChecked;
        public Action<Visibility> CheckVisibilityChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableList&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="entries">The entries.</param>
        public CheckableList(List<T> entries)
        {
            for (int x = 0; x < entries.Count(); x++)
                InsertItem(x, entries[x]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableList&lt;T&gt;"/> class.
        /// </summary>
        public CheckableList()
        {
        }

        /// <summary>
        /// Inserts the item.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        protected override void InsertItem(int index, T item)
        {
            item.Parent = this;
            item.PropertyChanged += OnClientItemChanged;
            base.InsertItem(index, item);
        }

        /// <summary>
        /// Sends an item changed notification
        /// </summary>
        public void NotifyCollectionChanged(int index)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)))
            );
        }

        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <param name="index">The index.</param>
        protected override void RemoveItem(int index)
        {
            Items[index].PropertyChanged -= OnClientItemChanged;
            countChecked--;

            // Call the delegate if the count turns to 0
            if (countChecked == 0)
            {
                // Call the delegate
                if (CheckVisibilityChanged != null)
                    CheckVisibilityChanged(CheckVisibility);

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                Kernel.Instance.EventAggregator.PublishMessage(new CheckableListChangeEvent<T>());
            }

            base.RemoveItem(index);
        }

        /// <summary>
        /// Clears the items.
        /// </summary>
        protected override void ClearItems()
        {
            countChecked = 0;
            base.ClearItems();
        }

        /// <summary>
        /// Called when [client item changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnClientItemChanged(object sender, PropertyChangedEventArgs e)
        {
            // Only react on IsChecked changes, because the complete list must be bound again
            var newCountCheck = Items.Where(x => x.IsChecked).Count();
            if (e.PropertyName == "IsChecked")
            {
                if (newCountCheck > 0 != countChecked > 0)
                {
                    countChecked = newCountCheck;

                    // Call the delegate
                    RaiseCheckVisibilityEvent();
                }
                else
                    countChecked = newCountCheck;
            }
        }

        private void RaiseCheckVisibilityEvent()
        {
            if (CheckVisibilityChanged != null)
                CheckVisibilityChanged(CheckVisibility);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            Kernel.Instance.EventAggregator.PublishMessage(new CheckableListChangeEvent<T>());
        }

        /// <summary>
        /// Resets the checks.
        /// </summary>
        public void ResetChecks()
        {
            if (countChecked == 0)
                return;

            // Reset all Checks
            foreach (var item in Items.Where(x => x.IsChecked).ToList())
                item.IsChecked = false;

            countChecked = 0;
            RaiseCheckVisibilityEvent();
        }

        /// <summary>
        /// Resets the editable.
        /// </summary>
        public void ResetEditable()
        {
            // Reset all editable items
            foreach (var item in Items.Where(x => x.IsEditable).ToList())
                item.IsEditable = false;
        }

        #region Property TipVisibility

        /// <summary>
        /// Gets or sets a value indicating whether this instance is tip visible.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is tip visible; otherwise, <c>false</c>.
        /// </value>
        public Visibility TipVisibility
        {
            get { return countChecked == 0 ? Visibility.Visible : Visibility.Collapsed; }
        }

        #endregion

        #region Property CheckVisibility

        /// <summary>
        /// Gets a value indicating whether this instance is check visible.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is check visible; otherwise, <c>false</c>.
        /// </value>
        public Visibility CheckVisibility
        {
            get { return countChecked > 0 ? Visibility.Visible : Visibility.Collapsed; ; }
        }

        /// <summary>
        /// Gets or sets the make check visible.
        /// </summary>
        /// <value>The make check visible.</value>
        public void SwitchCheckVisibility()
        {
            if (countChecked == 0 && Items.Count > 0)
            {
                countChecked = 1;
                RaiseCheckVisibilityEvent();
            }
            else
                ResetChecks();
        }

        #endregion
    }
}
