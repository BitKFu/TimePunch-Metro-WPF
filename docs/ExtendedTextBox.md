# Extended TextBox

The ExtendedTextBox is a TextBox that is directly connected to the ApplicationBar. Thus enables it to communicate with the ApplicationBar in order to show two Buttons "ok" and "cancel". As an enhancement to the standard TextBox an Input can be rejected if the user presses "cancel" instead of "ok".

**Here's an example of the ExtendedTextBox Control**
[Image:TextBox.png)(Image_TextBox.png)

**How to define the ExtendedTextBox Cotrol in XAML**
The following example shows how to define the ExtendedTextBox Control in XAML and connect it with an existing ApplicationBar.
{{
<TextBox:ExtendedTextBox   
              Margin="{StaticResource WinMargin}" 
              Text="{Binding ExtendedText, Mode=TwoWay}"
              ApplicationBar="{Binding ElementName=AppBar}"
              Style="{StaticResource WinInputBoxNormalStyle}"/>

<AppBar:ApplicationBar Grid.Row="1" x:Name="AppBar">
     <AppBar:ApplicationBar.Icons>
          <AppBar:ApplicationBarIcon 
               ImageSource="/TpMetroWpfLibrary;component/Images/appbar.save.png"
               Description="save"
               Command="{Binding SaveCommand}"/>
      </AppBar:ApplicationBar.Icons>
</AppBar:ApplicationBar>
}}