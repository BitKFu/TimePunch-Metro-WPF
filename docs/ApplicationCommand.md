# Application Commands
Application Commands are already implemented commands that are directly provided by the .NET Framework. The .NET Framework offers the following Application Commands that can be found here [http://msdn.microsoft.com/de-de/library/system.windows.input.applicationcommands.aspx](http://msdn.microsoft.com/de-de/library/system.windows.input.applicationcommands.aspx)

The most important of the list, is the Close Command. For those standard commands, no ICommand property has to be created in the view model. The base view model offers a possibility to bind them directly to an Execute and CanExecute method.

**Binding Application Commands in the view model**
The following Example from the MainWindowViewModelBase class binds the Application Command Close to the view model.
{{
public override void Initialize()
{
    // This method registers the WPF Command Close
   RegisterApplicationCommand(ApplicationCommands.Close, ExecuteCloseCommand, CanExecuteCloseCommand, true);
}
}}
**Binding Application Commands in XAML**
The following example shows how to bind application commands in XAML.
{{
<Button x:Name="CloseButton" 
       Style="{StaticResource ChromeButtonStyle}" 
       Padding="4"
       Command="ApplicationCommands.Close" 
       x:Uid="CloseButton">
       <TextBlock TextWrapping="Wrap" Text="r" FontFamily="Webdings" Foreground="#FF919191" FontSize="11" x:Uid="r_TextBlock" />
</Button>
}}
**Important pre-requisites**
In order to bind the Application Commands correct, the window or view must reference the helper class RegisterCommandBindings.
{{
<Metro:TransparentWindow
xmlns:Commands="clr-namespace:TimePunch.Metro.Wpf.Commands;assembly=TpMetroWpfLibrary" 
Commands:ApplicationCommands.RegisterCommandBindings="{Binding RegisteredCommands}"
}}