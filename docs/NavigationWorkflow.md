# Navigation Workflow
The navigation workflow has been implemented by using events. That means, if you want to navigate from one to another page, you are sending events instead of directly jumping to the new view.

**The advantage:** This decouples the view model from the concrete implementation of the view.
Therefore the BaseViewModel offers a property called EventAggregator that returns the instance of the used EventAggregator. 

**The diagram shows the single navigation steps**
[Image:NavigationWorkflow.png)(Image_NavigationWorkflow.png)

**Step 1**
Implement a new navigation event. The simplest implementation looks like:
{{
    public class NavigateToPage1
    {
    }
}}
Send this navigation event within your view model. For example
{{
        public void ExecuteFadeToPage1Command(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            EventAggregator.PublishMessage(new NavigateToPage1());
        }
}}
**Step 2**
Catch the navigation event in your controller class.
**Hint:** You can put the message itself as a second parameter to the NavigateToPage method in order to give some data to the target page.
{{
    public class DemoController : BaseController,
        IHandleMessage<NavigateToPage1>
    {
        /// <summary>
        /// This method handles the navigation event to page 1
        /// </summary>
        /// <param name="message"></param>
        public void Handle(NavigateToPage1 message)
        {
            NavigateToPage("Views/Page1View.xaml");
        }
    }
}}
**Step 3**
Initialize the viewmodel of your target page with the method InitializePage. Remember that the extraData parameter might be filled with the message, if you put it into the second parameter of NavigateToPage.
{{
        public override void InitializePage(object extraData)
        {
        }
}}