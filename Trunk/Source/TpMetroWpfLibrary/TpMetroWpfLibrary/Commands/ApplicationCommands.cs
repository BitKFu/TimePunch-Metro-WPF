// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Windows;
using System.Windows.Input;

namespace TimePunch.Metro.Wpf.Commands
{
    /// <summary>
    /// The ApplicationCommands Helper enables the binding of ApplicationCommands to the ViewModel.
    /// Inventor of this class was Micah Martin. Kudos to him.
    /// If you want to know more. Visit his webpage: http://codingcontext.wordpress.com/2008/12/10/commandbindings-in-mvvm/
    /// </summary>
    public class ApplicationCommands
    {
        public static DependencyProperty RegisterCommandBindingsProperty = DependencyProperty.RegisterAttached("RegisterCommandBindings", typeof(CommandBindingCollection), typeof(ApplicationCommands), new PropertyMetadata(null, OnRegisterCommandBindingChanged));

        /// <summary>
        /// Sets the register command bindings.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetRegisterCommandBindings(UIElement element, CommandBindingCollection value)
        {
            if (element != null)
                element.SetValue(RegisterCommandBindingsProperty, value);
        }

        /// <summary>
        /// Gets the register command bindings.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static CommandBindingCollection GetRegisterCommandBindings(UIElement element)
        {
            return (CommandBindingCollection) element?.GetValue(RegisterCommandBindingsProperty);
        }

        /// <summary>
        /// Called when [register command binding changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnRegisterCommandBindingChanged
        (DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is UIElement element)) 
                return;

            if (e.NewValue is CommandBindingCollection bindings)
                element.CommandBindings.AddRange(bindings);
        }
    }
}
