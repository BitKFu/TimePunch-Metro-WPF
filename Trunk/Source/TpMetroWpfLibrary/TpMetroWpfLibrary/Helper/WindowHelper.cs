using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace TimePunch.Metro.Wpf.Helper
{
    public static class WindowHelper
    {
        public static UIElement FindUid(this DependencyObject parent, string uid)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                UIElement el = VisualTreeHelper.GetChild(parent, i) as UIElement;
                if (el != null)
                {
                    if (el.Uid == uid) { return el; }
                    el = el.FindUid(uid);
                }
            }
            return null;
        }
    }

}
