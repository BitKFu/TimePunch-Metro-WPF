using System.Windows;
using ProgressBarDemo.Core;
using TimePunch.Metro.Wpf.Controller;
using TimePunch.MVVM.Controller;

namespace ProgressBarDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // At first, a instance of the conrecte Kernel has to be created and set
            Kernel.Instance = new DemoKernel();

            // The initialization of the kernel is optional, but maybe necessary for your concrete implementation
            Kernel.Instance.Controller.Init();
            
            base.OnStartup(e);
        }
    }
}
