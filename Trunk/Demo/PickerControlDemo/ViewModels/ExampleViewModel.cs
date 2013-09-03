using System;
using System.Collections.Generic;
using TimePunch.Metro.Wpf.ViewModel;

namespace PickerControlDemo.ViewModels
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
            Months = new List<Resource>()
                         {
                             new Resource("January"),
                             new Resource("Febuary"),
                             new Resource("March"),
                             new Resource("April"),
                             new Resource("May"),
                             new Resource("June"),
                             new Resource("July"),
                             new Resource("August"),
                             new Resource("September"),
                             new Resource("October"),
                             new Resource("November"),
                             new Resource("December"),
                         };

            SelectedMonth = Months[0];
            SelectedDate = DateTime.Now;
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

        #region Property Months

        /// <summary>
        /// Gets or sets Months list
        /// </summary>
        public List<Resource> Months
        {
            get { return GetPropertyValue(() => Months); }
            set { SetPropertyValue(() => Months, value); }
        }

        #endregion

        #region Property SelectedMonth

        /// <summary>
        /// Gets or sets the selected Month
        /// </summary>
        public Resource SelectedMonth
        {
            get { return GetPropertyValue(() => SelectedMonth); }
            set { SetPropertyValue(() => SelectedMonth, value); }
        }

        #endregion

        #region Property SelectedDate

        /// <summary>
        /// Gets or sets the SelectedDate
        /// </summary>
        public DateTime SelectedDate
        {
            get { return GetPropertyValue(() => SelectedDate); }
            set { SetPropertyValue(() => SelectedDate, value); }
        }

        #endregion

        #region Property SelectedTimeSpan

        /// <summary>
        /// Gets or sets the SelectedTimeSpan
        /// </summary>
        public TimeSpan SelectedTimeSpan
        {
            get { return GetPropertyValue(() => SelectedTimeSpan); }
            set { SetPropertyValue(() => SelectedTimeSpan, value); }
        }

        #endregion
    }
}
