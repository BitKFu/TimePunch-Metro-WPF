// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Windows.Controls;
using System.Windows.Input;

namespace TimePunch.Metro.Wpf.Controls.Picker
{
    /// <summary>
    /// Interaction logic for ListPickerFullMode.xaml
    /// </summary>
    public partial class ListPickerFullMode : Page
    {
        /// <summary>
        /// Initializes a new instance of the ListPicerFullMode class
        /// </summary>
        public ListPickerFullMode()
        {
            DataContext = new ListPickerFullModeViewModel();
            ((ListPickerFullModeViewModel)DataContext).Initialize();

            InitializeComponent();
        }

        /// <summary>
        /// Selected the current item
        /// </summary>
        /// <param name="sender">sending listpicker</param>
        /// <param name="e">event arguments</param>
        private void OnSelectItem(object sender, MouseButtonEventArgs e)
        {
            ExecuteItemSelection(sender);
        }

        /// <summary>
        /// Select an item either by mouse or by touch screen
        /// </summary>
        /// <param name="sender"></param>
        private void ExecuteItemSelection(object sender)
        {
            var box = sender as ListBox;
            if (box == null || box.SelectedItem == null)
                return;

            var context = ((ListPickerFullModeViewModel)DataContext);
            if (context.CheckCommand.CanExecute(null))
                context.CheckCommand.Execute(null);
        }
    }
}
