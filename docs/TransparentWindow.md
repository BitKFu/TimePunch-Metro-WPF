# Transparent Window
The TransparentWindow class is an extension to the Window class of WPF. It's used as a container for all views of your application.

**Hint**
What really matters is that the ApplicationCommands are registered properly like in the example below. Otherwise the window can't be closed.

{{
Commands:ApplicationCommands.RegisterCommandBindings="{Binding RegisteredCommands}"
}}
**Example Window**
Here's an example window and how it looks like:
![](TransparentWindow_TransparentWindow.png)

**Demo Implementation**
A good point to start with is the EmptyMetroDemo that you'll find in the demo folder.
{{
<Metro:TransparentWindow x:Class="EmptyMetroDemo.Views.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" 
xmlns:Metro="clr-namespace:TimePunch.Metro.Wpf.Metro;assembly=TpMetroWpfLibrary" 
xmlns:Commands="clr-namespace:TimePunch.Metro.Wpf.Commands;assembly=TpMetroWpfLibrary" 
xmlns:Frames="clr-namespace:TimePunch.Metro.Wpf.Frames;assembly=TpMetroWpfLibrary" 
DataContext="{Binding Source={StaticResource ViewModelLocator}, Path=MainWindowViewModel}"
Commands:ApplicationCommands.RegisterCommandBindings="{Binding RegisteredCommands}"
Title="MainWindow" Height="350" Width="525">
    <Grid>
            
    </Grid>
</Metro:TransparentWindow>
}}