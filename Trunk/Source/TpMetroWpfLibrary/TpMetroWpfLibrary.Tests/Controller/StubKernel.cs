using TimePunch.Metro.Wpf.Controller;
using TimePunch.MVVM.Controller;
using TimePunch.MVVM.EventAggregation;
using BaseController = TimePunch.MVVM.Controller.BaseController;

namespace TpMetroWpfLibrary.Tests.Controller
{
    /// <summary>
    /// Stub Kernel implementation
    /// </summary>
    public class StubKernel : Kernel
    {
        private IEventAggregator eventAggregator;
        private BaseController controller;

        #region Overrides of Kernel

        /// <summary>
        /// Gets the event aggregator.
        /// </summary>
        /// <returns></returns>
        public override IEventAggregator EventAggregator
        {
            get { return eventAggregator ?? (eventAggregator = new EventAggregator());}
        }

        /// <summary>
        /// Gets the current used controller
        /// </summary>
        /// <returns></returns>
        public override IBaseController Controller
        {
            get { return controller ?? (controller = new StubController()); }
        }

        #endregion
    }
}
