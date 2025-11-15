// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

#nullable enable
using System;
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
                GenericKernel.Instance!.EventAggregator.Subscribe(this);

            IsEnabledChanged += (_, _) =>
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
                GenericKernel.Instance?.EventAggregator.Unsubscribe(this);
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

        /// <summary>
        /// IsTouchSelectionEnabledProperty defines if the touch selection is enabled
        /// </summary>
        public static DependencyProperty IsTouchSelectionEnabledProperty
            = DependencyProperty.Register("IsTouchSelectionEnabled", typeof(bool), typeof(TimeSpanPicker), new PropertyMetadata(DeviceInfo.HasTouchInput()));

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
        private void OnEnterFullModeViaClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (IsReadonly)
                return;

            focusedControl = (Keyboard.FocusedElement as Control)?.Uid;

            if (IsTouchSelectionEnabled)
            {
                oldAnimationMode = GenericKernel.Instance?.EventAggregator.PublishMessage(new ChangeAnimationModeRequest(Frames.AnimationMode.Fade));
                GenericKernel.Instance?.EventAggregator.PublishMessage(new TimeSpanPickerFullModeRequest(FullModeHeader, Value, TimeSpanPickerId));
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
                oldAnimationMode = GenericKernel.Instance?.EventAggregator.PublishMessage(new ChangeAnimationModeRequest(Frames.AnimationMode.Fade));
                GenericKernel.Instance?.EventAggregator.PublishMessage(new TimeSpanPickerFullModeRequest(FullModeHeader, Value, TimeSpanPickerId));
            }
        }

        #endregion

        /// <summary>
        /// Handles a message of a specific type.
        /// </summary>
        /// <param name="message">the message to handle</param>
        public void Handle(TimeSpanPickerSelectRequest message)
        {
            if (message.TimeSpanPickerId != TimeSpanPickerId)
                return;

            // Set the selected Item
            Value = message.Value;
            GenericKernel.Instance?.EventAggregator.PublishMessage(new GoBackNavigationRequest());

            // switch the animation mode
            if (oldAnimationMode != null)
                GenericKernel.Instance?.EventAggregator.PublishMessage(oldAnimationMode);

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
        /// Gets the Picker Cursor 
        /// </summary>
        public Cursor PickerCursor
        {
            get { return IsReadonly || !IsEnabled ? Cursors.Arrow : Cursors.Hand; }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        private object? oldAnimationMode;
        private string? focusedControl;

        #endregion
    }
}
