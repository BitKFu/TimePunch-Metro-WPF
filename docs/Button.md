# Button
The library offers a style that can be used to show the buttons in Metro style. That means the button does not have borders nor other chrome. If the button gets pressed the text or image become grayed out in order to give the user feedback.

**How to use the button style in XAML**
The following example shows how to use the button style in XAML.
{{
            <Button Content="Validation" 
                    Command="{Binding ValidateCommand}"
                    Style="{StaticResource WinButtonStyle}"/>
}}