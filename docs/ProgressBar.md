# Progress Bar
The library offers a style that can be used to show an intermediate progress bar in Windows 8 style. 

**Here's an example for the styled ProgressBar**
[Image:ProgressBar.png)(Image_ProgressBar.png)

**How to define the style for the ProgressBar in XAML**
Here's an example how to define the ProgressBar in XAML.
{{
<ProgressBar 
    Visibility="{Binding IsLoading, Converter={StaticResource BooleanToVisibilityConverter} }"
    Style="{StaticResource PerformanceProgressBar}"/>
}}