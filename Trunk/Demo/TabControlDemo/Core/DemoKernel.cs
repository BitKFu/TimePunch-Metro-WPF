using TimePunch.MVVM.Controller;
using TimePunch.MVVM.EventAggregation;

namespace TabControlDemo.Core
{
    /// <summary>
    /// The Kernel is used to return instances of the concrete Event Aggregation class 
    /// and the concrete Controller class.
    /// </summary>
    public class DemoKernel : Kernel<DemoKernel, DemoController>
    {
    }
}
