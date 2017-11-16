# Time Picker
Using the time picker the user can select a time by scrolling the hour and minute selector panels. The TimePicker behaves like it does on Windows Phone 7.

**Here's an example of the TimePicker**
![](TimePicker_TimePicker1.png) ![](TimePicker_TimePicker2.png)

**How to define the TimePicker in XAML**
At first it's really important that the page has set the KeepAlive flag. That's because of the implementation of the Picker Control, which needs to hold an ID to identify the current instance of the Picker Control. If the KeepAlive flag is missing the ID might change and the result of the FullPage Selector won't be accepted.
{{
<Page x:Class="PickerControlDemo.Views.ExampleView"
      KeepAlive="True"  >
}}
The following example shows how to define the TimePicker in XAML.
{{
        <Picker:TimePicker 
            FullModeHeader="Select the time"
            Margin="{StaticResource WinMargin}" 
            IsTouchSelectionEnabled="True"
            Grid.Row="5" 
            Value="{Binding SelectedDate, Mode=TwoWay}"/>
}}
**Remark to IsTouchSelectionEnabled**
If IsTouchSelectionEnabled is not defined, the Framework will decided by it's own if a touch input dialog shall be shown or not. This depends if the user is working with a touchfriendly device or not. If you want to check that by your own, you can use the {{ DeviceInfo.HasTouchInput() }} method to proof it.
