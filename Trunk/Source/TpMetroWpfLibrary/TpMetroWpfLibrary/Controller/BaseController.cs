// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TimePunch.Metro.Wpf.Controls.Picker;
using TimePunch.Metro.Wpf.Events;
using TimePunch.Metro.Wpf.Frames;
using TimePunch.Metro.Wpf.Helper;
using TimePunch.MVVM.EventAggregation;

namespace TimePunch.Metro.Wpf.Controller
{
    /// <summary>
    /// Base class for module specific controllers
    /// </summary>
    public abstract class BaseController : TimePunch.MVVM.Controller.BaseController,
        IHandleMessage<CloseApplicationCommand>,
        IHandleMessage<ListPickerFullModeRequest>,
        IHandleMessage<DatePickerFullModeRequest>,
        IHandleMessage<TimePickerFullModeRequest>,
        IHandleMessage<TimeSpanPickerFullModeRequest>,
        IHandleMessage<WindowStateApplicationCommand>,
        IHandleMessage<ForceBindingUpdateEvent>,
        IHandleMessage<ChangeAnimationModeRequest>,
        INavigationController
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BaseController class.
        /// </summary>
        protected BaseController() 
            : base(Kernel.Instance.EventAggregator)
        {
        }

        /// <summary>
        /// This method can do some initializations
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        /// Handles a message of a specific type.
        /// </summary>
        /// <param name="message">the message to handle</param>
        public virtual void Handle(ForceBindingUpdateEvent message)
        {
            // Check, if we need to dispatch
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                var waitHandle = new AutoResetEvent(false);
                var action = (Action)(
                    () =>
                    {
                        Handle(message);
                        waitHandle.Set();
                    });

                try
                {
                    Application.Current.Dispatcher.Invoke(action, DispatcherPriority.Normal, CancellationToken.None, InvocationTimeout);
                }
                catch (TimeoutException)
                {
                    Trace.TraceWarning("Can't dispatch ForceBindingUpdateEvent in time, so try to invoke async");
                    Application.Current.Dispatcher.InvokeAsync(action);
                }

                waitHandle.WaitOne();
                return;
            }

            var topWindow = Application.Current.Windows.Count - 1;
            var element = Application.Current.Windows[topWindow];
            if (element == null)
                return;

            var focusObj = FocusManager.GetFocusedElement(element);
            if (!(focusObj is TextBox))
                return;

            var binding = (focusObj as TextBox).GetBindingExpression(TextBox.TextProperty);

            if (binding != null)
                binding.UpdateSource();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="BaseController"/> is reclaimed by garbage collection.
        /// </summary>
        ~BaseController()
        {
            EventAggregator.Unsubscribe(this);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Navigates to page.
        /// </summary>
        /// <param name="navigateToPage">The navigate to page.</param>
        public override void NavigateToPage(string navigateToPage)
        {
            if (CurrentPage == null)
                Application.Current.Dispatcher.BeginInvoke((ThreadStart)(() =>
                    ContentFrame.Navigate(new Uri(navigateToPage, UriKind.Relative))));
            else
            {
                if (CurrentPage.Dispatcher.CheckAccess())
                {
                    if (CurrentPage.NavigationService == null)
                        return;

                    PasswordBoxAssistant.IsNavigating = true;
                    try
                    {
                        CurrentPage.NavigationService.Navigate(new Uri(navigateToPage, UriKind.Relative));
                    }
                    finally
                    {
                        PasswordBoxAssistant.IsNavigating = false;
                    }
                }
                else
                    CurrentPage.Dispatcher.BeginInvoke((ThreadStart) (() => NavigateToPage(navigateToPage)));
            }
        }

        /// <summary>
        /// Navigates to a page and add additional data
        /// </summary>
        /// <param name="navigateToPage">The navigate to page.</param>
        /// <param name="message">The message thas will be send to the page</param>
        public override void NavigateToPage(string navigateToPage, object message)
        {
            if (CurrentPage == null)
                Application.Current.Dispatcher.BeginInvoke((ThreadStart)(() => 
                    ContentFrame.Navigate(new Uri(navigateToPage, UriKind.Relative), message)));
            else
            {
                if (CurrentPage.Dispatcher.CheckAccess())
                {
                    if (CurrentPage.NavigationService == null)
                        return;

                    PasswordBoxAssistant.IsNavigating = true;
                    try
                    {
                        CurrentPage.NavigationService.Navigate(new Uri(navigateToPage, UriKind.Relative), message);
                    }
                    finally
                    {
                        PasswordBoxAssistant.IsNavigating = false;    
                    }
                }
                else
                    CurrentPage.Dispatcher.BeginInvoke((ThreadStart) (() => NavigateToPage(navigateToPage, message)));
            }
        }

        #endregion

        #region IHandleMessage<CloseApplicationCommand>

        /// <summary>
        /// Handles a message of a specific type.
        /// </summary>
        /// <param name="message">the message to handle</param>
        public virtual void Handle(CloseApplicationCommand message)
        {
            // Maybe we can't access it directly
            if (!Application.Current.CheckAccess())
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() => Handle(message), DispatcherPriority.Normal,
                        CancellationToken.None, InvocationTimeout);
                }
                catch (TimeoutException)
                {
                    Trace.TraceWarning("Can't dispatch CloseApplicationCommand in time, so try to invoke async");
                    Application.Current.Dispatcher.InvokeAsync(() => Handle(message));
                }
                return;
            }

            ForceClosing = message.ForceClosing;
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Close();
        }

        /// <summary>
        /// Gets a value indicating whether the closing of the current window shall be forced.
        /// </summary>
        public virtual bool ForceClosing { get; private set; }

        #endregion

        #region IHandleMessage<WindowStateApplicationCommand>

        /// <summary>
        /// Handles a message of a specific type.
        /// </summary>
        /// <param name="message">the message to handle</param>
        public virtual void Handle(WindowStateApplicationCommand message)
        {
            // Maybe we can't access it directly
            if (!Application.Current.CheckAccess())
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() => Handle(message), DispatcherPriority.Normal,
                        CancellationToken.None, InvocationTimeout);
                }
                catch (TimeoutException)
                {
                    Trace.TraceWarning("Can't dispatch WindowStateApplicationCommand in time, so try to invoke async");
                    Application.Current.Dispatcher.InvokeAsync(() => Handle(message));
                }
                return;
            }

            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.WindowState = message.WindowState;
        }

        #endregion

        #region IHandleMessage<ListPickerFullModeRequest>

        /// <summary>
        /// Handles a message of a specific type.
        /// </summary>
        /// <param name="message">the message to handle</param>
        public virtual void Handle(ListPickerFullModeRequest message)
        {
            NavigateToPage("/TpMetroWpfLibrary;component/Controls/Picker/ListPickerFullMode.xaml", message);
        }

        #endregion

        #region IHandleMessage<DatePickerFullModeRequest>

        /// <summary>
        /// Handles a message of a specific type.
        /// </summary>
        /// <param name="message">the message to handle</param>
        public virtual void Handle(DatePickerFullModeRequest message)
        {
            NavigateToPage("/TpMetroWpfLibrary;component/Controls/Picker/DatePickerFullMode.xaml", message);
        }

        #endregion

        #region IHandleMessage<TimePickerFullModeRequest>

        /// <summary>
        /// Handles a message of a specific type.
        /// </summary>
        /// <param name="message">the message to handle</param>
        public virtual void Handle(TimePickerFullModeRequest message)
        {
            NavigateToPage("/TpMetroWpfLibrary;component/Controls/Picker/TimePickerFullMode.xaml", message);
        }

        #endregion

        #region IHandleMessage<TimePickerFullModeRequest>

        /// <summary>
        /// Handles a message of a specific type.
        /// </summary>
        /// <param name="message">the message to handle</param>
        public virtual void Handle(TimeSpanPickerFullModeRequest message)
        {
            NavigateToPage("/TpMetroWpfLibrary;component/Controls/Picker/TimeSpanPickerFullMode.xaml", message);
        }

        #endregion

        #region IHandleMessage<ChangeAnimationModeRequest>

        /// <summary>
        /// This method will change the animation mode
        /// </summary>
        /// <param name="message"></param>
        public virtual void Handle(ChangeAnimationModeRequest message)
        {
            if (!(ContentFrame is AnimationFrame animationFrame))
                return;

            // Maybe we can't access it directly
            if (!animationFrame.CheckAccess())
            {
                try
                {
                    animationFrame.Dispatcher.Invoke(() => Handle(message), DispatcherPriority.Normal, CancellationToken.None, InvocationTimeout);
                }
                catch (TimeoutException)
                {
                    Trace.TraceWarning("Can't dispatch ChangeAnimationModeRequest in time, so try to invoke async");
                    animationFrame.Dispatcher.InvokeAsync(() => Handle(message));
                }

                return;
            }

            // Switch the Animation Mode
            AnimationMode oldMode = animationFrame.AnimationMode;
            animationFrame.AnimationMode = message.AnimationMode;
            message.AnimationMode = oldMode;
        }

        #endregion
    }
}