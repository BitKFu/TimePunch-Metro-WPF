using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace PickerControlDemo
{
    public class Resource
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
    }
}
