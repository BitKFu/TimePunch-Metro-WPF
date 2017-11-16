# The Animation Frame
The animation frame is a combination of the [Slider Frame](SliderFrame) and [Fader Frame](FaderFrame). It can be used for both kind of animations and can be switched on runtime. Usually this should be your choice, because it covers both supported navigation transitions.

**XAML:**
{{
        <!-- The animation frame is used to have page transitions on page navigation -->
        <Frames:AnimationFrame x:Name="ContentFrame"
                               NavigationUIVisibility="Hidden"
                               Source="ExampleView.xaml" 
                               AnimationMode="Fade"
                               SlideDuration="0:0:0.300"
                               FadeDuration="0:0:0.050"
                               FadeOffset="0:0:0.150"/>
}}
**ChangeAnimationModeRequest**
You can switch the animation mode by sending an ChangeAnimationModeRequest event. Here's an example of how to switch the animation mode before navigating to a new page.
{{
public void ExecuteFadeToPage1Command(object sender, ExecutedRoutedEventArgs eventArgs)
{
   var oldAnimationMode = EventAggregator.PublishMessage(
     new ChangeAnimationModeRequest(TimePunch.Metro.Wpf.Frames.AnimationMode.Fade));
   EventAggregator.PublishMessage(new NavigateToPage1());
}
}}
**Dependency Properties:**
The AnimationFrame has the following Public Dependency Properties to define the stile.

* **AnimationMode** 
The Animation Mode can be set to **Slide** or **Fade**. It defines if a Slide or Fade Navigation will be applied.

* **SlideDuration**
The SlideDuration Property defines the duration of the slide navigation.

* **FadeDuration**
The FadeDuration Property defines the duration of the fade navigation.

* **FadeOffset**
The FadeOffset Property defines how long the navigation animation will wait until it starts. This can be useful, if there are another animations that have to finish before the navigation takes place. That's for example the case when using the ApplicationBar. Because the ApplicationBar slides out on navigation, this animation has to take place prior the frame navigation animation to catch the visual effect.

