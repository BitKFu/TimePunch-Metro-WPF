# Radio Button
The RadioButton has been restyled to match the new metro style. The size has been improved to make it more touch-friendly.

**Here's an example for the new restyled RadioButton**
[Image:RadioButton.png)(Image_RadioButton.png)

**How to define the RadioButton in XAML**
The following example shows how to define the RadioButton in XAML.
{{
        <StackPanel>
            
            <TextBlock Text="PinStyle"
                Style="{StaticResource WinTextTitle1Style}"  
                Margin="{StaticResource WinMargin}"/>
            
            <RadioButton Content="Always On" 
                Style="{StaticResource WinRadioButtonStyle}"/>

            <RadioButton Content="Always Off" 
                Style="{StaticResource WinRadioButtonStyle}"/>

            <RadioButton Content="Touch friendly" 
                Style="{StaticResource WinRadioButtonStyle}"/>

            <RadioButton Content="Fixed" 
                Style="{StaticResource WinRadioButtonStyle}"/>
        </StackPanel>        
}}