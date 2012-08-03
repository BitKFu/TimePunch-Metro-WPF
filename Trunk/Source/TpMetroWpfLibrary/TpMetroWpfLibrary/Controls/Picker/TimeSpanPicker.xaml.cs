﻿// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TimePunch.Metro.Wpf.Controller;
using TimePunch.Metro.Wpf.EventAggregation;
using TimePunch.Metro.Wpf.Events;
using TimePunch.Metro.Wpf.ViewModel;

namespace TimePunch.Metro.Wpf.Controls.Picker
{
    /// <summary>
    /// The TimeSpanPicker is a modification and extension to the DatePicker that Charles Petzold described.
    /// <see cref="http://msdn.microsoft.com/de-de/magazine/gg309180.aspx"/>
    /// </summary>
    public partial class TimeSpanPicker : IHandleMessage<TimeSpanPickerSelectRequest>, INotifyPropertyChanged
    {
        /// <summary>
        /// Unique Identifier used to identify the message from TimePickerFullModeViewModel
        /// </summary>
        private readonly Guid timeSpanPickerId = Guid.NewGuid();

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the DatePicker class.
        /// </summary>
        public TimeSpanPicker()
        {
            InitializeComponent();
            TimeSpanPickerContent.DataContext = this;

            if (!ViewModelBase.IsDesignMode)
                Kernel.Instance.EventAggregator.Subscribe(this);

            IsEnabledChanged += (s, e) =>
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("LabelVisibility"));
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("PickerCursor"));
                }
            };
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~TimeSpanPicker()
        {
            if (!ViewModelBase.IsDesignMode)
                Kernel.Instance.EventAggregator.Unsubscribe(this);
        }

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Value Property
        /// </summary>
        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(TimeSpan), typeof(TimeSpanPicker));

        /// <summary>
        /// FullMode Header that is shown in the fullmode page
        /// </summary>
        public static DependencyProperty FullModeHeaderProperty = DependencyProperty.Register("FullModeHeader", typeof(string), typeof(TimeSpanPicker));

        /// <summary>
        /// IsReadonly property specifies if the value shall be shown, but not enabled for editing
        /// </summary>
        public static DependencyProperty IsReadonlyProperty = DependencyProperty.Register("IsReadonly", typeof(bool), typeof(TimeSpanPicker));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the FullMode Header
        /// </summary>
        public string FullModeHeader
        {
            get { return (string)GetValue(FullModeHeaderProperty); }
            set { SetValue(FullModeHeaderProperty, value); }
        }

        /// <summary>
        /// Gets the Display Value
        /// </summary>
        public TimeSpan Value
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets the Unique Identifier used to identify the message from DatePickerFullModeViewModel
        /// </summary>
        public Guid TimeSpanPickerId
        {
            get { return timeSpanPickerId; }
        }

        #endregion

        #region Send Request to Enter the FullMode

        /// <summary>
        /// Send FullMode Request when the user clicks the Selected Item
        /// </summary>
        private void OnEnterFullModeViaClick(object sender, MouseButtonEventArgs e)
        {
            if (IsReadonly)
                return;

            Kernel.Instance.EventAggregator.PublishMessage(
                new TimeSpanPickerFullModeRequest(FullModeHeader, Value, TimeSpanPickerId));
        }

        /// <summary>
        /// Send FullMode Request when the user touches the Selected Item
        /// </summary>
        private void OnEnterFullModeViaTouch(object sender, TouchEventArgs e)
        {
            if (IsReadonly)
                return;

            Kernel.Instance.EventAggregator.PublishMessage(
                new TimeSpanPickerFullModeRequest(FullModeHeader, Value, TimeSpanPickerId));
        }

        #endregion

        /// <summary>
        /// Handles a message of a specific type.
        /// </summary>
        /// <param name="message">the message to handle</param>
        public void Handle(TimeSpanPickerSelectRequest message)
        {
            if (message == null || message.TimeSpanPickerId != TimeSpanPickerId)
                return;

            // Set the selected Item
            Value = message.Value;
            Kernel.Instance.EventAggregator.PublishMessage(new GoBackNavigationRequest());
        }

        /// <summary>
        /// Gets the visibility of the date label
        /// </summary>
        public Visibility LabelVisibility
        {
            get { return IsEnabled ? Visibility.Visible : Visibility.Hidden; }
        }

        /// <summary>
        /// Gets or sets the IsReadonly flag
        /// </summary>
        public bool IsReadonly
        {
            get { return (bool)GetValue(IsReadonlyProperty); }
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
        public Cursor PickerCursor
        {
            get { return IsReadonly || !IsEnabled ? Cursors.Arrow : Cursors.Hand; }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}