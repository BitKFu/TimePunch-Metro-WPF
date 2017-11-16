# Check Box
The CheckBox control has been restyled to match the new metro style. That means that the size of the control is bigger to enable easier tapping and make it touch-friendly.

**Here's an example of the restyled CheckBox control**
[Image:CheckBox.png)(Image_CheckBox.png)

**How to define the CheckBox in XAML**
The following example shows how to define the redesigned CheckBox using XAML.
{{
<StackPanel VerticalAlignment="Center" HorizontalAlignment="Center">
    <CheckBox Content="Tab this checkbox"
            Style="{StaticResource WinCheckBoxStyle}"/>

    <CheckBox Content="No, tab that checkbox"
            Style="{StaticResource WinCheckBoxStyle}"/>
</StackPanel>
}}