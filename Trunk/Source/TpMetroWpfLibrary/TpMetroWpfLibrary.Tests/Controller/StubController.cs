using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using TimePunch.Metro.Wpf.Controller;
using TimePunch.MVVM.EventAggregation;

namespace TpMetroWpfLibrary.Tests.Controller
{
    public class StubController : BaseController, INavigationController
    {
        public StubController() 
            : base(StubKernel.Instance.EventAggregator)
        {
        }
    }
}
