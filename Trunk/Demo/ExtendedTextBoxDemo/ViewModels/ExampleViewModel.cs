using System.Windows.Input;
using TimePunch.Metro.Wpf.Events;
using TimePunch.Metro.Wpf.ViewModel;

namespace ExtendedTextBoxDemo.ViewModels
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
            SaveCommand = RegisterCommand(ExecuteSaveCommand, CanExecuteSaveCommand, true);
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

        #region Save Command

        /// <summary>
        /// Gets or sets the Save command.
        /// </summary>
        /// <value>The Save command.</value>
        public ICommand SaveCommand
        {
            get { return GetPropertyValue(() => SaveCommand); }
            set { SetPropertyValue(() => SaveCommand, value); }
        }

        /// <summary>
        /// Determines whether this instance can execute Save command.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance can execute Save command; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void CanExecuteSaveCommand(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = true;
        }

        /// <summary>
        /// Executes the Save command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void ExecuteSaveCommand(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            EventAggregator.PublishMessage(new CloseApplicationCommand());
        }

        #endregion

        #region Property StandardText

        /// <summary>
        /// Gets or sets the Standard Text
        /// </summary>
        public string StandardText
        {
            get { return GetPropertyValue( () => StandardText ); }
            set { SetPropertyValue( () => StandardText, value); }
        }

        #endregion

        #region Property ExtendedText

        /// <summary>
        /// Gets or sets the Extended Text
        /// </summary>
        public string ExtendedText
        {
            get { return GetPropertyValue( () => ExtendedText ); }
            set { SetPropertyValue(() => ExtendedText, value); }
        }

        #endregion
}
}
