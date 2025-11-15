using ExtendedTextBoxDemo.Core;
using TimePunch.Metro.Wpf.Controller;
using TimePunch.MVVM.Controller;

namespace ExtendedTextBoxDemo.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();

            // The initialization of the kernel is optional, but maybe necessary for your concrete implementation
            DemoKernel.Instance.Controller.SetContentFrame(ContentFrame);
        }
    }
}
