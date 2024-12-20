﻿// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

#nullable enable
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TimePunch.MVVM.Events;
using ViewModelBase = TimePunch.Metro.Wpf.ViewModel.ViewModelBase;

namespace TimePunch.Metro.Wpf.Controls.Picker
{
    public class ListPickerFullModeViewModel : ViewModelBase
    {
        #region Initialization Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            CheckCommand = RegisterCommand(ExecuteCheckCommand, CanExecuteCheckCommand, true);
            CancelCommand = RegisterCommand(ExecuteCancelCommand, CanExecuteCancelCommand, true);
        }

        /// <summary>
        /// Initializes the page.
        /// This method will be called every time the user navigates to the page
        /// </summary>
        /// <param name="extraData">The extra Data, if there's any. Otherwise NULL</param>
        public override void InitializePage(object extraData)
        {
            if (!(extraData is ListPickerFullModeRequest request))
                return;

            // Initialize Page
            ItemsSource = request.ItemsSource;
            FullModeItemTemplate = request.FullModeItemTemplate;
            FullModeHeader = request.FullModeHeader;
            SelectedItem = request.SelectedItem;
            ListPickerId = request.ListPickerId;
            FilterDelegate = request.FilterDelegate;
            DisplayMemberPath = request.DisplayMemberPath;

            AddPropertyChangedNotification(() => FilterText, () => FilteredItemSource);
            OnPropertyChanged(() => FilteredItemSource);
        }

        /// <summary>
        /// Gets the ListPickerId
        /// </summary>
        public Guid ListPickerId { get; private set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the ItemsSource
        /// </summary>
        public IEnumerable? ItemsSource 
        {
            get { return GetPropertyValue(() => ItemsSource); }
            set { SetPropertyValue(() => ItemsSource, value); }
        }

        /// <summary>
        /// Returns the filtered items source
        /// </summary>
        public IEnumerable? FilteredItemSource
        {
            get
            {
                var dmp = DisplayMemberPath;
                if (dmp == null)
                    return null;

                return FilterDelegate != null
                           ? ItemsSource?.Cast<object>()
                               .Where(item => FilterDelegate(item.GetType().GetProperty(dmp).GetValue(item).ToString(), FilterText))
                           : ItemsSource;
            }
        }

        private int CountFiltered => (FilterDelegate != null && ItemsSource != null && DisplayMemberPath != null)
            ? ItemsSource
                .Cast<object>()
                .Count(item => FilterDelegate(item.GetType().GetProperty(DisplayMemberPath).GetValue(item).ToString(), FilterText))
            : ItemsSource?.Cast<object>().Count() ?? 0;

        /// <summary>
        /// Gets or sets the selected item
        /// </summary>
        public object? SelectedItem
        {
            get { return GetPropertyValue(() => SelectedItem); }
            set { SetPropertyValue(() => SelectedItem, value); }
        }

        /// <summary>
        /// Gets or sets the FullModeItemTemplate
        /// </summary>
        public DataTemplate? FullModeItemTemplate
        {
            get { return GetPropertyValue(() => FullModeItemTemplate); }
            set { SetPropertyValue(() => FullModeItemTemplate, value); }
        }

        /// <summary>
        /// Gets or sets the FullMode Header
        /// </summary>
        public string? FullModeHeader
        {
            get { return GetPropertyValue(() => FullModeHeader); }
            set { SetPropertyValue(() => FullModeHeader, value); }
        }

        /// <summary>
        /// Gets or sets the Filter text
        /// </summary>
        public string? FilterText
        {
            get { return GetPropertyValue(() => FilterText); }
            set
            {
                if (SetPropertyValue(() => FilterText, value))
                {
                    if (CountFiltered == 1 && FilteredItemSource != null)
                    {
                        var firstElement = FilteredItemSource.GetEnumerator();
                        firstElement.MoveNext();
                        SelectedItem = firstElement.Current;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the list picker filter delegate
        /// </summary>
        public ListPickerFilterDelegate? FilterDelegate
        {
            get { return GetPropertyValue(() => FilterDelegate); }
            set { SetPropertyValue(() => FilterDelegate, value); }
        }

        /// <summary>
        /// Gets or sets the display member path
        /// </summary>
        public string? DisplayMemberPath
        {
            get { return GetPropertyValue(() => DisplayMemberPath); }
            set { SetPropertyValue(() => DisplayMemberPath, value); }
        }

        #endregion

        #region Save Command

        /// <summary>
        /// Gets or sets the Save command.
        /// </summary>
        /// <value>The Save command.</value>
        public ICommand? CheckCommand
        {
            get { return GetPropertyValue(() => CheckCommand); }
            set { SetPropertyValue(() => CheckCommand, value); }
        }

        /// <summary>
        /// Determines whether this instance can execute Save command.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance can execute Save command; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void CanExecuteCheckCommand(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = true;
        }

        /// <summary>
        /// Executes the Save command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void ExecuteCheckCommand(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            EventAggregator.PublishMessage(new ListPickerSelectItemRequest(SelectedItem, ListPickerId));
        }

        #endregion

        #region Cancel Command

        /// <summary>
        /// Gets or sets the Cancel command.
        /// </summary>
        /// <value>The Cancel command.</value>
        public ICommand? CancelCommand
        {
            get { return GetPropertyValue(() => CancelCommand); }
            set { SetPropertyValue(() => CancelCommand, value); }
        }

        /// <summary>
        /// Determines whether this instance can execute Cancel command.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance can execute Cancel command; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void CanExecuteCancelCommand(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = true;
        }

        /// <summary>
        /// Executes the Cancel command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void ExecuteCancelCommand(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            EventAggregator.PublishMessage(new GoBackNavigationRequest());
        }

        #endregion
    }
}
