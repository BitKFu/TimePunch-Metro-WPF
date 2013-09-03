using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ListSwapDemo.Entities;
using TimePunch.Metro.Wpf.ViewModel;

namespace ListSwapDemo.ViewModels
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
            List1 = new ObservableCollection<Item>()
                        {
                            new Item(1, "January"),
                            new Item(2, "Febuary"),
                            new Item(3, "March"),
                            new Item(4, "April"),
                            new Item(5, "May"),
                            new Item(6, "June"),
                            new Item(7, "July"),
                            new Item(8, "August"),
                            new Item(9, "September"),
                            new Item(10, "October"),
                            new Item(11, "November"),
                            new Item(12, "December")
                        };
            List2 = new ObservableCollection<Item>();

            CopyAllItemsCommand = RegisterCommand(ExecuteCopyAllItemsCommand, CanExecuteCopyAllItemsCommand, true);
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

        #region Property CurrentItem1

        /// <summary>
        /// Gets or sets CurrentItem1
        /// </summary>
        public Item CurrentItem1
        {
            get { return GetPropertyValue(() => CurrentItem1); }
            set
            {
                SetPropertyValue(() => CurrentItem1, value);
                if (value != null)
                {
                    CurrentItem1 = null;
                    List2.Add(value);
                    List1.Remove(value);

                    OnPropertyChanged(() => List2);
                    OnPropertyChanged(() => List1);
                }
            }
        }

        #endregion

        #region Property CurrentItem2

        /// <summary>
        /// Gets or sets CurrentItem2
        /// </summary>
        public Item CurrentItem2
        {
            get { return GetPropertyValue(() => CurrentItem2); }
            set
            {
                SetPropertyValue(() => CurrentItem2, value);
                if (value != null)
                {
                    CurrentItem2 = null;
                    List1.Add(value);
                    List2.Remove(value);

                    OnPropertyChanged(() => List2);
                    OnPropertyChanged(() => List1);
                }
            }
        }

        #endregion

        #region Property List1

        /// <summary>
        /// Gets or sets List1
        /// </summary>
        public ObservableCollection<Item> List1
        {
            get { return GetPropertyValue(() => List1); }
            set { SetPropertyValue(() => List1, value); }
        }

        #endregion

        #region Property List2

        /// <summary>
        /// Gets or sets List2
        /// </summary>
        public ObservableCollection<Item> List2
        {
            get { return GetPropertyValue(() => List2); }
            set { SetPropertyValue(() => List2, value); }
        }

        #endregion

        #region CopyAllItems Command

        /// <summary>
        /// Gets or sets the CopyAllItems command.
        /// </summary>
        /// <value>The CopyAllItems command.</value>
        public ICommand CopyAllItemsCommand
        {
            get { return GetPropertyValue(() => CopyAllItemsCommand); }
            set { SetPropertyValue(() => CopyAllItemsCommand, value); }
        }

        /// <summary>
        /// Determines whether this instance can execute CopyAllItems command.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance can execute CopyAllItems command; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void CanExecuteCopyAllItemsCommand(object sender, CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = true;
        }

        /// <summary>
        /// Executes the CopyAllItems command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">Event arguments</param>
        public void ExecuteCopyAllItemsCommand(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            foreach (var item in List1)
                List2.Add(item);

            List1.Clear();
            OnPropertyChanged(() => List1);
            OnPropertyChanged(() => List2);
        }

        #endregion
    }
}
