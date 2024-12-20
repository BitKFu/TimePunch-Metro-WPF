﻿// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

#nullable enable
using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TimePunch.Metro.Wpf.Events;
using TimePunch.Metro.Wpf.Helper;
using TimePunch.MVVM.Controller;
using TimePunch.MVVM.EventAggregation;
using TimePunch.MVVM.Events;
using TimePunch.MVVM.ViewModels;

namespace TimePunch.Metro.Wpf.Controls.Picker
{
    /// <summary>
    /// Filter Delegate to replace the IItemFilter interface
    /// </summary>
    /// <param name="text">displayed text</param>
    /// <param name="filter">filter input</param>
    /// <returns></returns>
    public delegate bool ListPickerFilterDelegate(string text, string filter);

    /// <summary>
    /// The ListPicker is a modification and extension to the DatePicker that Charles Petzold described.
    /// It's designed to work like the ListPicker of Windows Phone 7
    /// <see cref="http://msdn.microsoft.com/de-de/magazine/gg309180.aspx"/>
    /// </summary>
    public partial class ListPicker : IHandleMessage<ListPickerSelectItemRequest>, INotifyPropertyChanged
    {
        #region Dependency Properties

        /// <summary>
        /// Item source. That's an enumerable collection of elements
        /// </summary>
        public static DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ListPicker));

        /// <summary>
        /// DisplayValue
        /// </summary>
        public static DependencyProperty DisplayValueProperty = DependencyProperty.Register("DisplayValue", typeof(string), typeof(ListPicker));

        /// <summary>
        /// Selected item of the item source.
        /// </summary>
        public static DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(ListPicker), new PropertyMetadata(null, OnSelectedItemChangedCallback, OnCoerceValueCallback));

        /// <summary>
        /// IsReadonly property specifies if the value shall be shown, but not enabled for editing
        /// </summary>
        public static DependencyProperty IsReadonlyProperty = DependencyProperty.Register("IsReadonly", typeof(bool), typeof(ListPicker));

        /// <summary>
        /// Defines a filter property
        /// </summary>
        public static DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(ListPickerFilterDelegate), typeof(ListPicker));

        private static object OnCoerceValueCallback(DependencyObject d, object basevalue)
        {
            return basevalue;
        }

        private static void OnSelectedItemChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null && e.OldValue != null)
                d.SetValue(DisplayValueProperty, string.Empty);
            else
                ((ListPicker)d).UpdateDisplayValue();
        }

        /// <summary>
        /// Name of the displayed properties of the selected item.
        /// If null, than the selected item will be shown within accessing any property.
        /// </summary>
        public static DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(ListPicker), new PropertyMetadata(null, OnDisplayMemberPathChangedCallback));

        private static void OnDisplayMemberPathChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ListPicker)d).UpdateDisplayValue();
        }

        /// <summary>
        /// FullMode Header that is shown in the fullmode page
        /// </summary>
        public static DependencyProperty FullModeHeaderProperty =
            DependencyProperty.Register("FullModeHeader", typeof(string), typeof(ListPicker));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the FullMode Item Template
        /// http://chris.59north.com/post/Using-DataTemplates-in-custom-controls.aspx
        /// </summary>
        public DataTemplate? FullModeItemTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the bound items source collection
        /// </summary>
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// Gets or sets the selected item, which must be included in the items source
        /// </summary>
        public object? SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        /// Gets or sets the Display Member Path
        /// </summary>
        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }

        /// <summary>
        /// Gets or sets the FullMode Header
        /// </summary>
        public string FullModeHeader
        {
            get => (string)GetValue(FullModeHeaderProperty);
            set => SetValue(FullModeHeaderProperty, value);
        }

        /// <summary>
        /// Gets the Display Value
        /// </summary>
        public string DisplayValue
        {
            get => (string)GetValue(DisplayValueProperty);
            set => SetValue(DisplayValueProperty, value);
        }

        /// <summary>
        /// Gets the Display Value
        /// </summary>
        public ListPickerFilterDelegate Filter
        {
            get => (ListPickerFilterDelegate)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        /// <summary>
        /// Updates the DisplayValue
        /// </summary>
        private void UpdateDisplayValue()
        {
            // If no Item has been set, return an empty string
            if (SelectedItem == null)
            {
                DisplayValue = string.Empty;
                return;
            }

            // If the Display Member Path is not set, than use the SelectedItem as it is.
            if (string.IsNullOrEmpty(DisplayMemberPath))
            {
                DisplayValue = SelectedItem.ToString();
                return;
            }

            // Try to Get the Property out of the Selected Item
            var info = SelectedItem.GetType().GetProperty(DisplayMemberPath);
            if (info == null)
            {
                DisplayValue = $"<{DisplayMemberPath}> not found";
                return;
            }

            // Return the evaluted property content
            var value = info.GetValue(SelectedItem, null);
            DisplayValue = value?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Gets the Unique Identifier used to identify the message from ListPickerFullModeViewModel
        /// </summary>
        public Guid ListPickerId { get; } = Guid.NewGuid();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ListPicker class
        /// </summary>
        public ListPicker()
        {
            InitializeComponent();
            ListPickerContent.DataContext = this;

            if (!ViewModelBase.IsDesignMode)
                Kernel.Instance?.EventAggregator.Subscribe(this);

            IsEnabledChanged += (_, _) =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PickerCursor"));
            };
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~ListPicker()
        {
            if (!ViewModelBase.IsDesignMode)
                Kernel.Instance?.EventAggregator.Unsubscribe(this);
        }

        #endregion

        #region Send Request to Enter the FullMode

        private string? focusedControl;

        /// <summary>
        /// Send FullMode Request when the user clicks the Selected Item
        /// </summary>
        private void OnEnterFullModeViaClick(object sender, RoutedEventArgs routedEventArgs)
        {
            focusedControl = (Keyboard.FocusedElement as Control)?.Uid;

            oldAnimationMode = Kernel.Instance?.EventAggregator.PublishMessage(
                new ChangeAnimationModeRequest(Frames.AnimationMode.Fade));

            if (FullModeItemTemplate != null)
                Kernel.Instance?.EventAggregator.PublishMessage(
                    new ListPickerFullModeRequest(FullModeHeader, ItemsSource, SelectedItem, FullModeItemTemplate, ListPickerId, Filter, DisplayMemberPath));
        }

        /// <summary>
        /// Send FullMode Request when the user touches the Selected Item
        /// </summary>
        private void OnEnterFullModeViaTouch(object sender, TouchEventArgs e)
        {
            focusedControl = (Keyboard.FocusedElement as Control)?.Uid;

            oldAnimationMode = Kernel.Instance?.EventAggregator.PublishMessage(
                new ChangeAnimationModeRequest(Frames.AnimationMode.Fade));

            if (FullModeItemTemplate != null)
                Kernel.Instance?.EventAggregator.PublishMessage(
                    new ListPickerFullModeRequest(FullModeHeader, ItemsSource, SelectedItem, FullModeItemTemplate, ListPickerId, Filter, DisplayMemberPath));
        }

        #endregion

        /// <summary>
        /// Handles the item selection
        /// </summary>
        /// <param name="message">item selection message</param>
        public void Handle(ListPickerSelectItemRequest message)
        {
            if (message.ListPickerId != ListPickerId)
                return;

            // Set the selected Item
            SelectedItem = message.SelectedItem;
            Kernel.Instance?.EventAggregator.PublishMessage(new GoBackNavigationRequest());

            // switch the animation mode
            if (oldAnimationMode != null)
                Kernel.Instance?.EventAggregator.PublishMessage(oldAnimationMode);

            if (focusedControl != null)
                Task.Run(() =>
                {
                    Thread.Sleep(500);
                    if ((Parent as FrameworkElement)?.Parent is UIElement window)
                        window.Dispatcher.Invoke(() =>
                        {
                            var element = window.FindUid(focusedControl).FindUid(focusedControl);
                            element?.Focus();
                            Keyboard.Focus(element); 
                            focusedControl = null;
                        });
                });
        }


        /// <summary>
        /// Gets or sets the IsReadonly flag
        /// </summary>
        public bool IsReadonly
        {
            get => (bool)GetValue(IsReadonlyProperty);
            set
            {
                SetValue(IsReadonlyProperty, value);

                if (PropertyChanged != null)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("PickerCursor"));
            }
        }

        /// <summary>
        /// Gets the Picker Cursor 
        /// </summary>
        public Cursor PickerCursor => IsReadonly || !IsEnabled ? Cursors.Arrow : Cursors.Hand;

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        private ChangeAnimationModeRequest? oldAnimationMode;

        #endregion


    }
}
