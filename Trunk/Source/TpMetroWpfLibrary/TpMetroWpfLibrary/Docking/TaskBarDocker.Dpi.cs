using System;
using System.Runtime.InteropServices;

namespace TimePunch.Metro.Wpf.Docking
{
    public partial class TaskBarDocker
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int GetDeviceCaps(IntPtr hDC, int nIndex);

        public enum DeviceCap
        {
            /// <summary> 
            /// Logical pixels inch in X 
            /// </summary> 
            LOGPIXELSX = 88,
            /// <summary> 
            /// Logical pixels inch in Y 
            /// </summary> 
            LOGPIXELSY = 90

            // Other constants may be founded on pinvoke.net 
        }

        /// <summary>
        /// Gets or sets the X Dpi
        /// </summary>
        protected int Xdpi { get; set; }

        /// <summary>
        /// gets or sets the Y Dpi
        /// </summary>
        protected int Ydpi { get; set; }

        /// <summary>
        /// Converts the given integer to the screen dpi
        /// </summary>
        public double ConvertXDpi(double x)
        {
            return x*96.0/Xdpi;
        }

        /// <summary>
        /// Converts the given integer to the screen dpi
        /// </summary>
        public double ConvertYDpi(double y)
        {
            return y*96.0/Ydpi;
        }

        /// <summary>
        /// Converts the given form paramter to screen coordinates
        /// </summary>
        public double ConvertXToScreen(double x)
        {
            return x*Xdpi/96.0;
        }

        /// <summary>
        /// Converts the given form paramter to screen coordinates
        /// </summary>
        public double ConvertYToScreen(double y)
        {
            return y*Ydpi/96.0;
        }
    }
}
