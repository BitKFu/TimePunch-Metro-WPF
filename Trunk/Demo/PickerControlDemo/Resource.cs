using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using TimePunch.Metro.Wpf.Controls.Picker;

namespace PickerControlDemo
{
    public class Resource : IItemFilter
    {
        /// <summary>
        /// Initializes a new instance of a Resource class
        /// </summary>
        /// <param name="text"></param>
        public Resource (string text)
        {
            Text = text;
        }

        /// <summary>
        /// Gets the Text value
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Filter the resources for the list picker
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool IsValueAccepted(string filter)
        {
            return filter == null || Text.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
