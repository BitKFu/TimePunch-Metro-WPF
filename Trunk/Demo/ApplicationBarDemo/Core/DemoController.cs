using TimePunch.Metro.Wpf.Controller;
using TimePunch.MVVM.EventAggregation;

namespace ApplicationBarDemo.Core
{
    /// <summary>
    /// The Controller is used to handle page transition navigation flow
    /// </summary>
    public class DemoController : BaseController
    {
        public DemoController() : base(DemoKernel.Instance.EventAggregator)
        {
        }
    }
}
