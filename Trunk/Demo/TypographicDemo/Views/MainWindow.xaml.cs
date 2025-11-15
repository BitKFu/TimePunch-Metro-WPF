using TimePunch.Metro.Wpf.Controller;
using TimePunch.MVVM.Controller;
using TypographicDemo.Core;

namespace TypographicDemo.Views
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
        }
    }
}
