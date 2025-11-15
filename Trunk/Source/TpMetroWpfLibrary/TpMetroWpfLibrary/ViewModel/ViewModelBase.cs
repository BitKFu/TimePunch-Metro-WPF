using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimePunch.Metro.Wpf.Controller;
using TimePunch.MVVM.Controller;
using TimePunch.MVVM.EventAggregation;

namespace TimePunch.Metro.Wpf.ViewModel
{
    public abstract class ViewModelBase : MVVM.ViewModels.ViewModelBase
    {
        protected ViewModelBase(IEventAggregator eventAggregator = null) 
            : base(eventAggregator ?? GenericKernel.Instance!.EventAggregator)
        {
        }
    }
}
