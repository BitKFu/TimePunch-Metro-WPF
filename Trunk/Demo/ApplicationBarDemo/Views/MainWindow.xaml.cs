using TimePunch.Metro.Wpf.Controller;
using TimePunch.MVVM.Controller;

namespace ApplicationBarDemo.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
            Kernel.Instance.Controller.SetContentFrame(ContentFrame);
        }
    }
}
