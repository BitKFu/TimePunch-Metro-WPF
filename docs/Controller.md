# The Controller 

The main purpose of the controller is to decouple the navigation from the concrete view model implementation. In other words, the view model should not be aware of the concrete view nor know their name.

Therefore your application should implement a Controller class like the following:
{{
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
            NavigateToPage("Views/Page2View.xaml", message);
        }
    }
}}
**Hint:**
Have a look at the [Navigation Workflow](NavigationWorkflow) to see how the navigation process is handled.