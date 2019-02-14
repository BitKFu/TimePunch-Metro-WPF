﻿// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Globalization;
using System.Windows.Data;

namespace TimePunch.Metro.Wpf.Converter
{
    /// <summary>
    /// This Converter is used to show a date value as a short timespan string
    /// </summary>
    public class ShortTimeSpanConverter : IValueConverter
    {
        private TimeSpan fallbackTimeSpan;

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (value is string && value.ToString() == string.Empty) )
                return "-.-";

            var timeSpan = (TimeSpan)value;
            if (timeSpan == TimeSpan.Zero)
                return "-.-";

            fallbackTimeSpan = timeSpan;
            return timeSpan.ToString(@"hh\:mm");
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Use the current culture
            culture = CultureInfo.CurrentCulture;

            // try zero
            if (string.IsNullOrWhiteSpace(value?.ToString()) || value?.ToString() == "-.-")
                return targetType == typeof(TimeSpan) ? TimeSpan.Zero : (TimeSpan?) null;

            // Try to convert to double
            if (double.TryParse(value.ToString(), NumberStyles.Number, culture.NumberFormat, out var dblValue))
            {
                // Maybe we have a format like 2000 - means 20 o´clock / 730 - means 7:30
                if (dblValue > 100)
                {
                    var hours = (int)(Math.Abs(dblValue) / 100);
                    var minutes = (int)Math.Abs(dblValue) % 100;
                    return TimeSpan.FromHours(Math.Min(23, hours)).Add(TimeSpan.FromMinutes(Math.Min(59, minutes)));
                }
                else
                {
                    // Maybe the double value is below 24
                    if (dblValue < 24)
                        return TimeSpan.FromHours(Math.Max(0, Math.Min(23.99, dblValue)));
                }
            }

            // Try to convert with TimeSpan first
            if (TimeSpan.TryParse(value.ToString(), culture.DateTimeFormat, out var result))
                return result;

            return fallbackTimeSpan;
            
        }
    }
}
