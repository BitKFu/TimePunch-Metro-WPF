# Inter-ViewModel Communication
Although the [navigation workflow](NavigationWorkflow) is the most common to use event aggregation, it can also be useful if you want to send messages from one to another view model. E.g. for returning results from a client view model to the parent view model.

In that case your parent view model can be the receiver of the message and the client view model is the sender.

{{
    public class ClientViewModel : ViewModelBase
    {
        public void ExecuteOkCommand(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            EventAggregator.PublishMessage(
               new ResultOfPageX ( << all the data you want to send >> ));

            EventAggregator.PublishMessage(new GoBackNavigationRequest());
        }
    }

    public class ParentViewModel : ViewModelBase,
        IHandleMessage<ResultOfPageX>
    {
        public void Handle(ResultOfPageX message)
        {
            // Handle the message right here
        }
    }
}}