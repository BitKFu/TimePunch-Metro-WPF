# Relay Commands
Relay Commands are commands that can be directly bound to your view model. That has the advantage, that your view model has no knowledge about your view.

**Example of how to register a Relay Command**
Relay Commands can be created within your view model within the  Initialize() method.
{{
public override void Initialize()
{
    FadeToPage1Command = RegisterCommand(
         ExecuteFadeToPage1Command, CanExecuteFadeToPage1Command, true);
    SlideToPage2Command = RegisterCommand(
         ExecuteSlideToPage2Command, CanExecuteSlideToPage2Command, true);
}
}}
**Hint**
The third parameter of the RegisterCommand method tells the view model to disable the button, if the IsLoading property is set to true. E.g. This can be very useful when calling web services.

**Example of how to define a Relay Command**
Relay Commands do always consits of three parts. The command property itself, the CanExecute Method and the Execute Method.
{{
#region FadeToPage1 Command
public ICommand FadeToPage1Command
{
    get { return GetPropertyValue(() => FadeToPage1Command); }
    set { SetPropertyValue(() => FadeToPage1Command, value); }
}

public void CanExecuteFadeToPage1Command(object sender, CanExecuteRoutedEventArgs eventArgs)
{
    eventArgs.CanExecute = true;
}

public void ExecuteFadeToPage1Command(object sender, ExecutedRoutedEventArgs eventArgs)
{
    EventAggregator.PublishMessage(new NavigateToPage1());
}
#endregion
}}
**Example of how to bind a Relay Command**
Using XAML you can bind a relay command to any kind of controls that supports a Command Binding. For example, you can bind the command to a button.
{{
            <Button Content="Fade To Page 1"
                    HorizontalContentAlignment="Left"
                    FontWeight="ExtraBold"
                    Command="{Binding FadeToPage1Command}"
                    Style="{StaticResource WinButtonStyle}"/>
}}