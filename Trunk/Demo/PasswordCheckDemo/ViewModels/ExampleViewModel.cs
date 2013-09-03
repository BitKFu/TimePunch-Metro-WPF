using System.Windows.Input;
using TimePunch.Metro.Wpf.ViewModel;

namespace PasswordCheckDemo.ViewModels
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
            ValidateCommand = RegisterCommand(ExecuteValidateCommand, CanExecuteValidateCommand, true);
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

        #region Property Validated

        /// <summary>
        /// Gets or sets a flag indicating whether the current password could be validatd
        /// </summary>
        public bool? Validated
        {
            get { return GetPropertyValue(() => Validated); }
            set { SetPropertyValue(() => Validated, value); }
        }

        #endregion

        #region Property Password1

        /// <summary>
        /// Gets or sets the password one
        /// </summary>
        public string Password1
        {
            get { return GetPropertyValue(() => Password1); }
            set { SetPropertyValue(() => Password1, value); }
        }

        #endregion

        #region Property Password2

        /// <summary>
        /// Gets or sets the password two
        /// </summary>
        public string Password2
        {
            get { return GetPropertyValue(() => Password2); }
            set { SetPropertyValue(() => Password2, value); }
        }

        #endregion

        #region Validate Command

        /// <summary>
        /// Gets or sets the Validate command.
        /// </summary>
        /// <value>The Validate command.</value>
        public ICommand ValidateCommand
        {
            get { return GetPropertyValue(() => ValidateCommand); }
            set { SetPropertyValue(() => ValidateCommand, value); }
        }

        /// <summary>
        /// Determines whether this instance can execute Validate command.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance can execute Validate command; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void CanExecuteValidateCommand(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = true;
        }

        /// <summary>
        /// Executes the Validate command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void ExecuteValidateCommand(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            Validated = null;

            // The Display Error Control is used to display any kind of error
            // Additionally you can set a Faulted Command that can be invoked by the Display Error Control
            if (string.IsNullOrEmpty(Password1))
            {
                Error = "A password is required";
                FaultedCommand = ValidateCommand;
                return;
            }
            
            Validated = Password1 == Password2;
            if (!Validated.Value)
            {
                Error = "Passwords do not match.";
                FaultedCommand = ValidateCommand;
            }
            else
            {
                // To hide the display error control, the error message must be resetted
                Error = string.Empty;
                FaultedCommand = null;
            }
        }

        #endregion
    }
}
