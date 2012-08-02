// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Windows;
using System.Windows.Input;
using TimePunch.Metro.Wpf.ViewModel;

namespace TimePunch.Metro.Wpf.Controls.CheckedList
{
    public abstract class CheckableListItem<T> : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableListItem"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected CheckableListItem(T entity)
        {
            OnCheckedCommand = RegisterCommand(OnCheckedExecute, OnCheckedCanExecute, false);
            Entity = entity;
        }

        /// <summary>
        /// Gets or sets the checked entity
        /// </summary>
        public T Entity
        {
            get { return GetPropertyValue(() => Entity); }
            set { SetPropertyValue(() => Entity, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableListItem"/> class.
        /// </summary>
        protected CheckableListItem()
        {
            OnCheckedCommand = RegisterCommand(OnCheckedExecute, OnCheckedCanExecute, false);
        }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public ICheckableList Parent { get; set; }

        #region Property Content

        private string content;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Content
        {
            get { return content; }
            set
            {
                if (value == content) return;

                content = value;
                OnPropertyChanged(() => Content);

                IsEditable = false;
            }
        }

        #endregion

        #region Property IsChecked

        private bool isChecked;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is checked.
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (value == isChecked) return;

                isChecked = value;
                OnPropertyChanged(() => IsChecked);
                OnPropertyChanged(() => TipVisibility);
                OnPropertyChanged(() => CheckVisibility);
            }
        }

        #endregion

        #region Property TipVisibility

        /// <summary>
        /// Gets or sets a value indicating whether this instance is tip visible.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is tip visible; otherwise, <c>false</c>.
        /// </value>
        public virtual Visibility TipVisibility
        {
            get { return Parent.TipVisibility; }
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
            get { return Parent.CheckVisibility; }
        }

        #endregion

        #region Property IsEditable

        private bool isEditable;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is editable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is editable; otherwise, <c>false</c>.
        /// </value>
        public bool IsEditable
        {
            get { return isEditable; }
            set
            {
                if (isEditable == value)
                    return;

                isEditable = value;
                OnPropertyChanged(() => IsEditable);
                OnPropertyChanged(() => EditVisibility);
                OnPropertyChanged(() => ViewVisibility);
            }
        }

        #endregion

        #region Property EditVisibility

        /// <summary>
        /// Gets the apply visibility.
        /// </summary>
        /// <value>The apply visibility.</value>
        public Visibility EditVisibility
        {
            get { return IsEditable ? Visibility.Visible : Visibility.Collapsed; }
        }

        #endregion

        #region Property ViewVisibility

        /// <summary>
        /// Gets the apply visibility.
        /// </summary>
        /// <value>The apply visibility.</value>
        public Visibility ViewVisibility
        {
            get { return IsEditable ? Visibility.Collapsed : Visibility.Visible; }
        }

        #endregion

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            //@todo: Add Initialize calling
        }

        /// <summary>
        /// Initializes the page.
        /// This method will be called every time the user navigates to the page
        /// </summary>
        /// <param name="extraData">The extra Data, if there's any. Otherwise NULL</param>
        public override void InitializePage(object extraData)
        {
        }

        /// <summary>
        /// Called when [checked].
        /// </summary>
        /// <value>The on checked.</value>
        public ICommand OnCheckedCommand
        {
            get { return GetPropertyValue(() => OnCheckedCommand); }
            set { SetPropertyValue(() => OnCheckedCommand, value); }
        }

        /// <summary>
        /// Called when [checked can execute].
        /// </summary>
        private static void OnCheckedCanExecute(object obj, CanExecuteRoutedEventArgs canExecuteRoutedEventArgs)
        {
            canExecuteRoutedEventArgs.CanExecute = true;
        }

        /// <summary>
        /// Called when [checked execute].
        /// </summary>
        private void OnCheckedExecute(object obj, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            IsChecked = true;
        }
    }
}
