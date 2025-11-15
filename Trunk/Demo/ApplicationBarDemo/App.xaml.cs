using System.Windows;
using ApplicationBarDemo.Core;
using TimePunch.Metro.Wpf.Controller;
using TimePunch.MVVM.Controller;

namespace ApplicationBarDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // The initialization of the kernel is optional, but maybe necessary for your concrete implementation
            DemoKernel.Instance.Controller.Init();

            base.OnStartup(e);
        }
    }
}
