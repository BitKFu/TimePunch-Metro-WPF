// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

#nullable enable
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TimePunch.Metro.Wpf.Controller;
using TimePunch.Metro.Wpf.Events;
using TimePunch.Metro.Wpf.Helper;
using TimePunch.MVVM.Controller;
using TimePunch.MVVM.EventAggregation;
using TimePunch.MVVM.Events;
using TimePunch.MVVM.ViewModels;

namespace TimePunch.Metro.Wpf.Controls.Picker
{
    /// <summary>
    /// The TimePicker is a modification and extension to the DatePicker that Charles Petzold described.
    /// It's designed to work like the TimePicker of Windows Phone 7
    /// <see cref="http://msdn.microsoft.com/de-de/magazine/gg309180.aspx"/>
    /// </summary>
    public partial class TimePicker : IHandleMessage<TimePickerSelectRequest>, INotifyPropertyChanged
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the DatePicker class.
        /// </summary>
        public TimePicker()
        {
            InitializeComponent();
            TimePickerContent.DataContext = this;

            if (!ViewModelBase.IsDesignMode)
                Kernel.Instance?.EventAggregator.Subscribe(this);

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
        ~TimePicker()
        {
            if (!ViewModelBase.IsDesignMode)
                Kernel.Instance?.EventAggregator.Unsubscribe(this);
        }

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Value Property
        /// </summary>
        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(DateTime), typeof(TimePicker), new PropertyMetadata(OnValueChanged));

        /// <summary>
        /// FullMode Header that is shown in the fullmode page
        /// </summary>
        public static DependencyProperty FullModeHeaderProperty = DependencyProperty.Register("FullModeHeader", typeof(string), typeof(TimePicker));

        /// <summary>
        /// IsReadonly property specifies if the value shall be shown, but not enabled for editing
        /// </summary>
        public static DependencyProperty IsReadonlyProperty = DependencyProperty.Register("IsReadonly", typeof(bool), typeof(TimePicker));

        /// <summary>
        /// IsTouchSelectionEnabledProperty defines if the touch selection is enabled
        /// </summary>
        public static DependencyProperty IsTouchSelectionEnabledProperty
            = DependencyProperty.Register("IsTouchSelectionEnabled", typeof(bool), typeof(TimePicker), new PropertyMetadata(DeviceInfo.HasTouchInput()));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the FullMode Header
        /// </summary>
        public string FullModeHeader
        {
            get => (string)GetValue(FullModeHeaderProperty);
            set
            {
                if (FullModeHeader == value)
                    return;

                SetValue(FullModeHeaderProperty, value);
            }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("TimePicker Value Changed to {0}", d.GetValue(e.Property));

            if (d is TimePicker picker && picker.PropertyChanged != null)
                picker.PropertyChanged.Invoke(picker, new PropertyChangedEventArgs("TimePickerValue"));
        }

        /// <summary>
        /// Gets the Display Value
        /// </summary>
        public DateTime Value
        {
            get => (DateTime)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Gets or sets the TimePicker Value
        /// </summary>
        public DateTime TimePickerValue
        {
            get => Value;
            set => Value = Value.Date.Add(value.TimeOfDay);
        }

        /// <summary>
        /// Gets the Unique Identifier used to identify the message from DatePickerFullModeViewModel
        /// </summary>
        public Guid TimePickerId { get; } = Guid.NewGuid();

        #endregion

        #region Send Request to Enter the FullMode

        /// <summary>
        /// Send FullMode Request when the user clicks the Selected Item
        /// </summary>
        private void OnEnterFullModeViaClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (IsReadonly) 
                return;

            focusedControl = (Keyboard.FocusedElement as Control)?.Uid;

            if (IsTouchSelectionEnabled)
            {
                oldAnimationMode = Kernel.Instance?.EventAggregator.PublishMessage(new ChangeAnimationModeRequest(Frames.AnimationMode.Fade));
                Kernel.Instance?.EventAggregator.PublishMessage(new TimePickerFullModeRequest(FullModeHeader, Value, TimePickerId));
            }
        }

        /// <summary>
        /// Send FullMode Request when the user touches the Selected Item
        /// </summary>
        private void OnEnterFullModeViaTouch(object sender, TouchEventArgs e)
        {
            if (IsReadonly) 
                return;
            
            focusedControl = (Keyboard.FocusedElement as Control)?.Uid;

            if (IsTouchSelectionEnabled)
            {
                oldAnimationMode = Kernel.Instance?.EventAggregator.PublishMessage(new ChangeAnimationModeRequest(Frames.AnimationMode.Fade));
                Kernel.Instance?.EventAggregator.PublishMessage(new TimePickerFullModeRequest(FullModeHeader, Value, TimePickerId));
            }
        }

        #endregion

        /// <summary>
        /// Handles a message of a specific type.
        /// </summary>
        /// <param name="message">the message to handle</param>
        public void Handle(TimePickerSelectRequest message)
        {
            if (message.TimePickerId != TimePickerId)
                return;

            // Set the selected Item
            Value = message.Value;
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

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        private object? oldAnimationMode;
        private string? focusedControl;

        #endregion

        /// <summary>
        /// Gets or sets the IsReadonly flag
        /// </summary>
        public bool IsReadonly
        {
            get => (bool) GetValue(IsReadonlyProperty);
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
            get => (bool)GetValue(IsTouchSelectionEnabledProperty);
            set => SetValue(IsTouchSelectionEnabledProperty, value);
        }

        /// <summary>
        /// Gets the Picker Cursor 
        /// </summary>
        public Cursor PickerCursor => IsReadonly || !IsEnabled ? Cursors.Arrow : Cursors.Hand;
    }
}
