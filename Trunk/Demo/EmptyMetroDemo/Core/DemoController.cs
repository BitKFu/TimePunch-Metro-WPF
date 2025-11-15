using TimePunch.Metro.Wpf.Controller;

namespace EmptyMetroDemo.Core
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
