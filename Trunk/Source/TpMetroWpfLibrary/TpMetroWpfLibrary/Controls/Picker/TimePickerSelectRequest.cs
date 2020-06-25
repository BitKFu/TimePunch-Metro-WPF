// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;

namespace TimePunch.Metro.Wpf.Controls.Picker
{
    public class TimePickerSelectRequest
    {
        /// <summary>
        /// Initializes a new instance of the TimePickerSelectRequest class
        /// </summary>
        /// <param name="value">selected date value to use</param>
        /// <param name="timePickerId">id of the corresponding listpicker</param>
        public TimePickerSelectRequest(DateTime value, Guid timePickerId)
        {
            Value = value;
            TimePickerId = timePickerId;
        }

        /// <summary>
        /// Gets or sets the selected item
        /// </summary>
        public DateTime Value { get; }

        /// <summary>
        /// Gets the TimePicker Id
        /// </summary>
        public Guid TimePickerId { get;}
    }
}
