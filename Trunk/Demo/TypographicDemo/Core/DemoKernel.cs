﻿using TimePunch.MVVM.Controller;
using TimePunch.MVVM.EventAggregation;

namespace TypographicDemo.Core
{
    /// <summary>
    /// The Kernel is used to return instances of the concrete Event Aggregation class 
    /// and the concrete Controller class.
    /// </summary>
    public class DemoKernel : Kernel
    {
        private IEventAggregator eventAggregator;
        private BaseController controller;

        #region Overrides of Kernel

        /// <summary>
        /// Gets the event aggregator. It is used to send messages (e.g. in order to navigate between views)
        /// </summary>
        public override IEventAggregator EventAggregator
        {
            get { return eventAggregator ?? (eventAggregator = new EventAggregator()); }
        }

        /// <summary>
        /// Gets the concrete Controller. It is used to manage the navigation flow
        /// </summary>
        public override IBaseController Controller
        {
            get { return controller ?? (controller = new DemoController()); }
        }

        #endregion
    }
}
