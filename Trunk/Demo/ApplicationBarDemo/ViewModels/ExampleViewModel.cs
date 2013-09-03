using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TimePunch.Metro.Wpf.ViewModel;

namespace ApplicationBarDemo.ViewModels
{
    /// <summary>
    /// That's the ViewModel that belongs to the Example View
    /// </summary>
    public class ExampleViewModel : ViewModelBase
    {
        #region Fields

        private string[] months = {
                                      "January", "Febuary", "March", "April", "May", "June", "July", "August", "September",
                                      "October", "November", "December"
                                  };

        #endregion

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

            // Register the commands at all
            PrevCommand = RegisterCommand(ExecutePrevCommand, CanExecutePrevCommand, true);
            NextCommand = RegisterCommand(ExecuteNextCommand, CanExecuteNextCommand, true);

            // Send Property Changed notifaction when the selected month is set
            AddPropertyChangedNotification(() => SelectedMonth, () => SelectedText);

            // Revalidate the commands after the selected month changed
            AddPropertyChangedNotification(() => SelectedMonth, PrevCommand, NextCommand);
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

        #region Property SelectedText

        /// <summary>
        /// Gets or sets DESCRIPTION
        /// </summary>
        public string SelectedText
        {
            get { return months[SelectedMonth]; }
        }

        #endregion

        #region Property SelectedMonth

        /// <summary>
        /// Gets or sets DESCRIPTION
        /// </summary>
        public int SelectedMonth
        {
            get { return GetPropertyValue(() => SelectedMonth); }
            set { SetPropertyValue(() => SelectedMonth, value); }
        }

        #endregion

        #region Prev Command

        /// <summary>
        /// Gets or sets the Prev command.
        /// </summary>
        /// <value>The Prev command.</value>
        public ICommand PrevCommand
        {
            get { return GetPropertyValue(() => PrevCommand); }
            set { SetPropertyValue(() => PrevCommand, value); }
        }

        /// <summary>
        /// Determines whether this instance can execute Prev command.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance can execute Prev command; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void CanExecutePrevCommand(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = SelectedMonth>0;
        }

        /// <summary>
        /// Executes the Prev command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void ExecutePrevCommand(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            SelectedMonth--;
        }

        #endregion

        #region Next Command

        /// <summary>
        /// Gets or sets the Next command.
        /// </summary>
        /// <value>The Next command.</value>
        public ICommand NextCommand
        {
            get { return GetPropertyValue(() => NextCommand); }
            set { SetPropertyValue(() => NextCommand, value); }
        }

        /// <summary>
        /// Determines whether this instance can execute Next command.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance can execute Next command; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void CanExecuteNextCommand(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = SelectedMonth<11;
        }

        /// <summary>
        /// Executes the Next command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void ExecuteNextCommand(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            SelectedMonth++;
        }

        #endregion
    }
}
