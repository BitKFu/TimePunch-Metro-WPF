using SnappedWindowDemo.Core;
using TimePunch.Metro.Wpf.Controller;
using TimePunch.MVVM.Controller;

namespace SnappedWindowDemo.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
            DemoKernel.Instance.Controller.SetContentFrame(ContentFrame);

            ShowActivated = true;
            Topmost = true;
        }
    }
}
