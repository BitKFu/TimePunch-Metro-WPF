using TimePunch.Metro.Wpf.Controller;
using TimePunch.MVVM.Controller;
using TimePunch.MVVM.EventAggregation;
using BaseController = TimePunch.MVVM.Controller.BaseController;

namespace TpMetroWpfLibrary.Tests.Controller
{
    /// <summary>
    /// Stub Kernel implementation
    /// </summary>
    public class StubKernel : Kernel<StubKernel, StubController>
    {
    }
}
