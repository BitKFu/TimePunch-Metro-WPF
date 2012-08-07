using System.Windows.Input;
using NavigationDemo.NavigationEvents;
using TimePunch.Metro.Wpf.Events;
using TimePunch.Metro.Wpf.ViewModel;

namespace NavigationDemo.ViewModels
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
            FadeToPage1Command = RegisterCommand(ExecuteFadeToPage1Command, CanExecuteFadeToPage1Command, true);
            SlideToPage2Command = RegisterCommand(ExecuteSlideToPage2Command, CanExecuteSlideToPage2Command, true);
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

        #region FadeToPage1 Command

        /// <summary>
        /// Gets or sets the FadeToPage1 command.
        /// </summary>
        /// <value>The FadeToPage1 command.</value>
        public ICommand FadeToPage1Command
        {
            get { return GetPropertyValue(() => FadeToPage1Command); }
            set { SetPropertyValue(() => FadeToPage1Command, value); }
        }

        /// <summary>
        /// Determines whether this instance can execute FadeToPage1 command.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance can execute FadeToPage1 command; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void CanExecuteFadeToPage1Command(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = true;
        }

        /// <summary>
        /// Executes the FadeToPage1 command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void ExecuteFadeToPage1Command(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            EventAggregator.PublishMessage(new ChangeAnimationModeRequest(TimePunch.Metro.Wpf.Frames.AnimationMode.Fade));
            EventAggregator.PublishMessage(new NavigateToPage1());
        }

        #endregion

        #region SlideToPage2 Command

        /// <summary>
        /// Gets or sets the SlideToPage2 command.
        /// </summary>
        /// <value>The SlideToPage2 command.</value>
        public ICommand SlideToPage2Command
        {
            get { return GetPropertyValue(() => SlideToPage2Command); }
            set { SetPropertyValue(() => SlideToPage2Command, value); }
        }

        /// <summary>
        /// Determines whether this instance can execute SlideToPage2 command.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance can execute SlideToPage2 command; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void CanExecuteSlideToPage2Command(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = true;
        }

        /// <summary>
        /// Executes the SlideToPage2 command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void ExecuteSlideToPage2Command(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            EventAggregator.PublishMessage(new ChangeAnimationModeRequest(TimePunch.Metro.Wpf.Frames.AnimationMode.Slide));
            EventAggregator.PublishMessage(new NavigateToPage2());
        }

        #endregion
    }
}
