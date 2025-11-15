using TimePunch.Metro.Wpf.Controller;

namespace SnappedWindowDemo.Core
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
