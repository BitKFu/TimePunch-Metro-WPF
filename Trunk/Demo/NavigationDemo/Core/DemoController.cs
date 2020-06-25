using NavigationDemo.NavigationEvents;
using TimePunch.Metro.Wpf.Controller;
using TimePunch.MVVM.EventAggregation;

namespace NavigationDemo.Core
{
    /// <summary>
    /// The Controller is used to handle page transition navigation flow
    /// </summary>
    public class DemoController : BaseController,
        IHandleMessage<NavigateToPage1>,
        IHandleMessage<NavigateToPage2>
    {
        /// <summary>
        /// This method handles the navigation event to page 1
        /// </summary>
        /// <param name="message"></param>
        public void Handle(NavigateToPage1 message)
        {
            NavigateToPage("Views/Page1View.xaml");
        }

        /// <summary>
        /// This method handles the navigation event to page 2
        /// </summary>
        /// <param name="message"></param>
        public void Handle(NavigateToPage2 message)
        {
            NavigateToPage("Views/Page2View.xaml");
        }
    }
}
