using System;
using System.Windows.Forms;
using System.Windows.Markup;
using TimePunch.Metro.Wpf.Helper;
using TimePunch.MVVM.ViewModels;

namespace TimePunch.Metro.Wpf.Extensions
{
    [MarkupExtensionReturnType(typeof(Uri))] 
    public class StylesAccordingToCurrentResolution : MarkupExtension
    {
        [ConstructorArgument("Resource")]
        public string Resource { get; set; }

        [ConstructorArgument("Resource768")]
        public string Resource768 { get; set; }

        private readonly ScreenResolution screenResolution = new ScreenResolution();

        #region Overrides of MarkupExtension

        /// <summary>
        /// When implemented in a derived class, returns an object that is set as the value of the target property for this markup extension. 
        /// </summary>
        /// <returns>
        /// The object value to set on the property where the extension is applied. 
        /// </returns>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            try
            {
                if (ViewModelBase.IsDesignMode)
                    return new Uri(Resource, UriKind.RelativeOrAbsolute);

                var activeScreen = Screen.PrimaryScreen;
                var dpi = screenResolution.Xdpi;

                // Get the width and height, you might want to at least round these to a few values. 
                var height = activeScreen.Bounds.Height;
                string resourceName;
            
                // Use the smaller sizes, if the screen resolution is higher than 96 dpi (which is standard) or the height is smaller or equal to 768
                if (height <= 768 || dpi > 96)
                    resourceName = Resource768;
                else
                    resourceName = Resource;

                // Add the resource to the app. 
                return new Uri(resourceName, UriKind.RelativeOrAbsolute);
            }
            catch (Exception)
            {
                // Don't throw an exception at this place.
                // Use the default
                return new Uri(Resource, UriKind.RelativeOrAbsolute);
            }
        }

        #endregion
    }
}