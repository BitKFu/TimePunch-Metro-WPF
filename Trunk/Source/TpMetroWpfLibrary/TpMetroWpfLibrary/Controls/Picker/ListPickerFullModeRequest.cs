// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections;
using System.Windows;

namespace TimePunch.Metro.Wpf.Controls.Picker
{
    /// <summary>
    /// This event is used to request the full mode of the listpicker
    /// </summary>
    public class ListPickerFullModeRequest
    {
        /// <summary>
        /// Initializes a new instance of the ListPickerFullModeRequest
        /// </summary>
        public ListPickerFullModeRequest(
            string fullModeHeader, 
            IEnumerable itemsSource, 
            object selectedItem, 
            DataTemplate fullModeItemTemplate, 
            Guid listPickerId,
            ListPickerFilterDelegate? filterDelegate,
            string displayMemberPath
            )
        {
            FullModeHeader = fullModeHeader;
            ItemsSource = itemsSource;
            SelectedItem = selectedItem;
            FullModeItemTemplate = fullModeItemTemplate;
            ListPickerId = listPickerId;
            FilterDelegate = filterDelegate;
            DisplayMemberPath = displayMemberPath;
        }

        /// <summary>
        /// Gets or sets the ItemsSource
        /// </summary>
        public IEnumerable ItemsSource { get;}

        /// <summary>
        /// Gets or sets the selected item
        /// </summary>
        public object SelectedItem { get; }

        /// <summary>
        /// Gets or sets the FullModeItemTemplate
        /// </summary>
        public DataTemplate FullModeItemTemplate { get; }

        /// <summary>
        /// Gets or sets the FullMode Header
        /// </summary>
        public string FullModeHeader { get; }

        /// <summary>
        /// Gets the ListPicker Id
        /// </summary>
        public Guid ListPickerId { get;  }

        /// <summary>
        /// Gets the list picker filter delegate
        /// </summary>
        public ListPickerFilterDelegate? FilterDelegate { get; }

        /// <summary>
        /// Gets the display member path
        /// </summary>
        public string DisplayMemberPath { get; }
    }
}
