using System.Windows.Input;
using TimePunch.Metro.Wpf.ViewModel;

namespace ProgressBarDemo.ViewModels
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
            base.Initialize();
            ProgressCommand = RegisterCommand(ExecuteProgressCommand, CanExecuteProgressCommand, true);
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

        #region Progress Command

        public ICommand ProgressCommand
        {
            get { return GetPropertyValue(() => ProgressCommand); }
            set { SetPropertyValue(() => ProgressCommand, value); }
        }

        /// <summary>
        /// Determines whether this instance can execute Prev command.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance can execute Prev command; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void CanExecuteProgressCommand(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = true;
        }

        /// <summary>
        /// Executes the Prev command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void ExecuteProgressCommand(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            IsLoading = true;
            ExecuteAsync(() => IsLoading = false, 10000);
        }

        #endregion
    }
}
