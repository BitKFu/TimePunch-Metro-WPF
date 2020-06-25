// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Globalization;

namespace TimePunch.Metro.Wpf.Controls.Picker
{
    public class DayInfo
    {
        // 1 through 28, 29, 30, 31
        public int DayNumber { set; get; }

        public int Year { set; get; }
        public int Month { set; get; }

        public string DayOfWeek => DateTimeFormatInfo.CurrentInfo.DayNames[(int)new DateTime(Year, Month, DayNumber).DayOfWeek];

        public override string ToString()
        {
            return $"{DayNumber} {DayOfWeek}";
        }
    }
}
