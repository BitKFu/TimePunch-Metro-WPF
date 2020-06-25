using TimePunch.Metro.Wpf.Controller;

namespace NavigationDemo.Views
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
