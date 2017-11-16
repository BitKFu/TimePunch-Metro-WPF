# Kernel

The Kernel class holds references to the two most important classes. The Kernel class must be implemented by every Application and it's a perfect place for using Ninject to create the event aggregator and controller.

**The Event Aggregator**
This is most time the given EventAggregator class, but you can also use your own implementation. E.g. for event logging purposes. The event aggregator is used for sending messages between views.

**The Controller**
The controller class is important and unique for every application. It controls the navigation flow within the application. 

The Kernel class must be set within the Application OnStartup Method.

{{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // At first, a instance of the conrecte Kernel has to be created and set
            Kernel.Instance = new DemoKernel();

            // The initialization of the kernel is optional, but maybe necessary for your concrete implementation
            Kernel.Instance.Controller.Init();
            
            base.OnStartup(e);
        }
    }
}}

**Demo Implementation of the Kernel class**
This is a simple implementation that shows how to implement a Kernel class.

{{
    /// <summary>
    /// The Kernel is used to return instances of the concrete Event Aggregation class 
    /// and the concrete Controller class.
    /// </summary>
    public class DemoKernel : Kernel
    {
        private IEventAggregator eventAggregator;
        private INavigationController controller;

        #region Overrides of Kernel

        /// <summary>
        /// Gets the event aggregator. It is used to send messages
        /// (e.g. in order to navigate between views)
        /// </summary>
        public override IEventAggregator EventAggregator
        {
            get { return eventAggregator ?? (eventAggregator = new EventAggregator()); }
        }

        /// <summary>
        /// Gets the concrete Controller. It is used to manage the navigation flow
        /// </summary>
        public override INavigationController Controller
        {
            get { return controller ?? (controller = new DemoController()); }
        }

        #endregion
    }
}}