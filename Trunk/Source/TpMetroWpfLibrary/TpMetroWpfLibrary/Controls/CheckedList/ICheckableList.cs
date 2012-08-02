// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Windows;

namespace TimePunch.Metro.Wpf.Controls.CheckedList
{
    public interface ICheckableList
    {
        /// <summary>
        /// Gets the is tip visible.
        /// </summary>
        /// <value>The is tip visible.</value>
        Visibility TipVisibility { get; }

        /// <summary>
        /// Gets the is check visible.
        /// </summary>
        /// <value>The is check visible.</value>
        Visibility CheckVisibility { get; }
    }
}
