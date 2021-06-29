using System.Windows;
using System.Windows.Input;
using TimePunch.Metro.Wpf.Metro;
using TimePunch.Metro.Wpf.ViewModel;

namespace SnappedWindowDemo.ViewModels
{
    /// <summary>
    /// That's the ViewModel that belongs to the Example View
    /// </summary>
    public class ExampleViewModel : ViewModelBase
    {
        #region Overrides of ViewModelBase

        /// <summary>
        /// Initializes the ViewModel. 
        /// 
        /// This is used to handle initialization that can't be done in the constructor.
        /// The method should only called once, after the ViewModel has been created.
        /// </summary>
        public override void Initialize()
        {
            DockToBottomCommand = RegisterCommand(ExecuteDockToBottomCommand, CanExecuteDockToBottomCommand, true);
            DockToTopCommand = RegisterCommand(ExecuteDockToTopCommand, CanExecuteDockToTopCommand, true);
            DockToRightCommand = RegisterCommand(ExecuteDockToRightCommand, CanExecuteDockToRightCommand, true);
            DockToLeftCommand = RegisterCommand(ExecuteDockToLeftCommand, CanExecuteDockToLeftCommand, true);

            PinStyle = PinStyle.TouchFriendly;
            AddPropertyChangedNotification(() => PinStyle, () => PinStyleAlwaysOn);
        }

        /// <summary>
        /// Initializes the Page.
        /// 
        /// This method is used to do some page initialization. 
        /// The calling page can start the new page with some extra data for page initialization.
        /// 
        /// This method is also called on a GoBackNavigationRequest, but without parameter data.
        /// </summary>
        /// <param name="extraData"></param>
        public override void InitializePage(object extraData)
        {
        }

        #endregion

        #region DockToTop Command

        /// <summary>
        /// Gets or sets the DockToTop command.
        /// </summary>
        /// <value>The DockToTop command.</value>
        public ICommand DockToTopCommand
        {
            get { return GetPropertyValue(() => DockToTopCommand); }
            set { SetPropertyValue(() => DockToTopCommand, value); }
        }

        /// <summary>
        /// Determines whether this instance can execute DockToTop command.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance can execute DockToTop command; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void CanExecuteDockToTopCommand(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = true;
        }

        /// <summary>
        /// Executes the DockToTop command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void ExecuteDockToTopCommand(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            var window = (SnappedTransparentWindow)Application.Current.MainWindow;
            window.Dock(TimePunch.Metro.Wpf.Docking.TaskBar.TaskBarEdge.Top, 0);
        }

        #endregion

        #region DockToBottom Command

        /// <summary>
        /// Gets or sets the DockToBottom command.
        /// </summary>
        /// <value>The DockToBottom command.</value>
        public ICommand DockToBottomCommand
        {
            get { return GetPropertyValue(() => DockToBottomCommand); }
            set { SetPropertyValue(() => DockToBottomCommand, value); }
        }

        /// <summary>
        /// Determines whether this instance can execute DockToBottom command.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance can execute DockToBottom command; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void CanExecuteDockToBottomCommand(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = true;
        }

        /// <summary>
        /// Executes the DockToBottom command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void ExecuteDockToBottomCommand(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            var window = (SnappedTransparentWindow)Application.Current.MainWindow;
            window.Dock(TimePunch.Metro.Wpf.Docking.TaskBar.TaskBarEdge.Bottom, 0);
        }

        #endregion
    
        #region DockToLeft Command

        /// <summary>
        /// Gets or sets the DockToLeft command.
        /// </summary>
        /// <value>The DockToLeft command.</value>
        public ICommand DockToLeftCommand
        {
            get { return GetPropertyValue(() => DockToLeftCommand); }
            set { SetPropertyValue(() => DockToLeftCommand, value); }
        }

        /// <summary>
        /// Determines whether this instance can execute DockToLeft command.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance can execute DockToLeft command; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void CanExecuteDockToLeftCommand(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = true;
        }

        /// <summary>
        /// Executes the DockToLeft command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void ExecuteDockToLeftCommand(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            var window = (SnappedTransparentWindow)Application.Current.MainWindow;
            window.Dock(TimePunch.Metro.Wpf.Docking.TaskBar.TaskBarEdge.Left, 0);
        }

        #endregion

        #region DockToRight Command

        /// <summary>
        /// Gets or sets the DockToRight command.
        /// </summary>
        /// <value>The DockToRight command.</value>
        public ICommand DockToRightCommand
        {
            get { return GetPropertyValue(() => DockToRightCommand); }
            set { SetPropertyValue(() => DockToRightCommand, value); }
        }

        /// <summary>
        /// Determines whether this instance can execute DockToRight command.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance can execute DockToRight command; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void CanExecuteDockToRightCommand(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = true;
        }

        /// <summary>
        /// Executes the DockToRight command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void ExecuteDockToRightCommand(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            var window = (SnappedTransparentWindow)Application.Current.MainWindow;
            window.Dock(TimePunch.Metro.Wpf.Docking.TaskBar.TaskBarEdge.Right, 0);
        }

        #endregion

        #region Property PinStyle

        /// <summary>
        /// Gets or sets the Pin Style
        /// </summary>
        public PinStyle PinStyle
        {
            get { return GetPropertyValue(() => PinStyle); }
            set
            {
                if (SetPropertyValue(() => PinStyle, value))
                {
                    var window = (SnappedTransparentWindow)Application.Current.MainWindow;
                    window.PinStyle = PinStyle;
                }
            }
        }
        
        #endregion

        #region Property PinStyleAlwaysOn

        /// <summary>
        /// Gets a value indicating whether the PinStyle is set to always on
        /// </summary>
        public bool PinStyleAlwaysOn
        {
            get { return PinStyle == PinStyle.AlwaysOn; }
        }

        #endregion
    }
}
