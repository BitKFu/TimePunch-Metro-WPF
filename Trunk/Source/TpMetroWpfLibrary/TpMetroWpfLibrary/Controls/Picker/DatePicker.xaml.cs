﻿// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using TimePunch.Metro.Wpf.Controller;
using TimePunch.Metro.Wpf.EventAggregation;
using TimePunch.Metro.Wpf.Events;
using TimePunch.Metro.Wpf.Helper;
using TimePunch.Metro.Wpf.ViewModel;

namespace TimePunch.Metro.Wpf.Controls.Picker
{
    /// <summary>
    /// The DatePicker is an extension to the DatePicker that Charles Petzold described.
    /// It's designed to work like the DatePicker of Windows Phone 7
    /// <see cref="http://msdn.microsoft.com/de-de/magazine/gg309180.aspx"/>
    /// </summary>
    public partial class DatePicker : IHandleMessage<DatePickerSelectDateRequest>, INotifyPropertyChanged
    {
        /// <summary>
        /// Unique Identifier used to identify the message from ListPickerFullModeViewModel
        /// </summary>
        private readonly Guid datePickerId = Guid.NewGuid();
        private ChangeAnimationModeRequest oldAnimationMode;
        
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the DatePicker class.
        /// </summary>
        public DatePicker()
        {
            InitializeComponent();
            DatePickerContent.DataContext = this;

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
        ~DatePicker()
        {
            if (!ViewModelBase.IsDesignMode)
                Kernel.Instance.EventAggregator.Unsubscribe(this);
        }

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Value Property
        /// </summary>
        public static DependencyProperty ValueProperty 
            = DependencyProperty.Register("Value", typeof(DateTime), typeof(DatePicker));

        /// <summary>
        /// FullMode Header that is shown in the fullmode page
        /// </summary>
        public static DependencyProperty FullModeHeaderProperty 
            = DependencyProperty.Register("FullModeHeader", typeof(string), typeof(DatePicker));

        /// <summary>
        /// IsReadonly property specifies if the value shall be shown, but not enabled for editing
        /// </summary>
        public static DependencyProperty IsReadonlyProperty
            = DependencyProperty.Register("IsReadonly", typeof(bool), typeof(DatePicker));

        /// <summary>
        /// IsTouchSelectionEnabledProperty defines if the touch selection is enabled
        /// </summary>
        public static DependencyProperty IsTouchSelectionEnabledProperty 
            = DependencyProperty.Register("IsTouchSelectionEnabled", typeof(bool), typeof(DatePicker), new PropertyMetadata(DeviceInfo.HasTouchInput()));

        private bool isPickerVisible;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the FullMode Header
        /// </summary>
        public string FullModeHeader
        {
            get { return (string)GetValue(FullModeHeaderProperty); }
            set { SetValue(FullModeHeaderProperty, value);  }
        }

        /// <summary>
        /// Gets the Display Value
        /// </summary>
        public DateTime Value
        {
            get { return (DateTime)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the DatePicker Value
        /// </summary>
        public DateTime DatePickerValue
        {
            get { return Value; }
            set
            {
                Value = value.Date.Add(Value.TimeOfDay);
                IsPickerVisible = false;

                if (PropertyChanged != null)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("DatePickerValue"));
            }
        }

        /// <summary>
        /// Gets the Unique Identifier used to identify the message from DatePickerFullModeViewModel
        /// </summary>
        public Guid DatePickerId
        {
            get { return datePickerId; }
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

            if (IsTouchSelectionEnabled)
            {
                oldAnimationMode =Kernel.Instance.EventAggregator.PublishMessage(new ChangeAnimationModeRequest(Frames.AnimationMode.Fade));
                Kernel.Instance.EventAggregator.PublishMessage(new DatePickerFullModeRequest(FullModeHeader, Value, DatePickerId));
            }
        }

        /// <summary>
        /// Send FullMode Request when the user touches the Selected Item
        /// </summary>
        private void OnEnterFullModeViaTouch(object sender, TouchEventArgs e)
        {
            if (IsReadonly)
                return;

            oldAnimationMode = Kernel.Instance.EventAggregator.PublishMessage(new ChangeAnimationModeRequest(Frames.AnimationMode.Fade));
            Kernel.Instance.EventAggregator.PublishMessage(new DatePickerFullModeRequest(FullModeHeader, Value, DatePickerId));
        }

        #endregion

        /// <summary>
        /// Handles a message of a specific type.
        /// </summary>
        /// <param name="message">the message to handle</param>
        public void Handle(DatePickerSelectDateRequest message)
        {
            if (message == null || message.DatePickerId != DatePickerId)
                return;

            // Set the selected Item
            Value = message.Value;
            Kernel.Instance.EventAggregator.PublishMessage(new GoBackNavigationRequest());

            // switch the animation mode
            if (oldAnimationMode != null)
                Kernel.Instance.EventAggregator.PublishMessage(oldAnimationMode);
        }

        /// <summary>
        /// Gets the visibility of the date label
        /// </summary>
        public Visibility LabelVisibility
        {
            get { return IsEnabled ? Visibility.Visible : Visibility.Hidden; }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


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
        /// Gets or sets the IsTouchSelectionEnabledProperty flag
        /// </summary>
        public bool IsTouchSelectionEnabled
        {
            get { return (bool)GetValue(IsTouchSelectionEnabledProperty); }
            set { SetValue(IsTouchSelectionEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the IsPickerVisible flag
        /// </summary>
        public bool IsPickerVisible
        {
            get
            {
                return isPickerVisible;
            }
            set
            {
                isPickerVisible = value;
                CalendarPopup.IsOpen = value;
            }
        }

        /// <summary>
        /// Gets the Picker Cursor 
        /// </summary>
        public Cursor PickerCursor
        {
            get { return IsReadonly || !IsEnabled ? Cursors.Arrow : Cursors.Hand; }
        }

        /// <summary>
        /// Opens the Popup Calendar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenCalendarPopup(object sender, RoutedEventArgs e)
        {
            IsPickerVisible = !IsPickerVisible;
        }

        private void ClosePopup(object sender, RoutedEventArgs e)
        {
            IsPickerVisible = false;
        }
    }
}
