# Metro Style Resources
The TimePunch Metro WPF Library offers two resource files that includes both the same set of styles. The difference is the size of the controls.

Whereas the first style offers big controls that fits best for a screen height above 768 pixel, the second is styled for small devices with 1024x768 or 1366x768 pixels.

**Style for big control size**
{{
<ResourceDictionary.MergedDictionaries>
   <!-- Use this style for big control size -->
   <ResourceDictionary Source="/TpMetroWpfLibrary;component/Styles/MetroStyles.xaml"/>
</ResourceDictionary.MergedDictionaries>
}}
**Style for small control size**
{{
<ResourceDictionary.MergedDictionaries>
   <!-- Use this style for small control size -->
   <ResourceDictionary Source="/TpMetroWpfLibrary;component/Styles/MetroStyles768.xaml"/>
</ResourceDictionary.MergedDictionaries>
}}
## How to automatically choose the right style
We implemented a behaviour that chooses the right styles by looking at the screen resolution of the primary screen. 
{{
<ResourceDictionary.MergedDictionaries>
   <ResourceDictionary Source="{Metro:MetroStylesAccordingToCurrentResolution}" />
</ResourceDictionary.MergedDictionaries>
}}
**Choose the right sized style for your own style**
If you want to use that feature for your own styles, we also implemented a behaviour that helps you with that. It's called the StylesAccordingToCurrentResolution behaviour and it's used like that.
{{
<ResourceDictionary.MergedDictionaries>
   <ResourceDictionary Source="{Metro:StylesAccordingToCurrentResolution
            Resource=Styles/YourStyles.xaml, 
            Resource768=Styles/YourStyles768.xaml}"/>
</ResourceDictionary.MergedDictionaries>
}}