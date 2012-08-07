using NavigationDemo.ViewModels;

namespace NavigationDemo.Core
{
    /// <summary>
    /// The ViewModel Locator handles the ViewModels of the application.
    /// I would recommend to use Ninject or something similar to handle ViewModel instances.
    /// </summary>
    public class DemoViewModelLocator
    {
        private MainWindowViewModel mainWindowViewModel;
        private ExampleViewModel exampleViewModel;
        private Page1ViewModel page1ViewModel;
        private Page2ViewModel page2ViewModel;

        /// <summary>
        /// Gets the MainWindow ViewModel
        /// </summary>
        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                // Creates the MainWindow ViewModel
                if (mainWindowViewModel == null)
                {
                    mainWindowViewModel = new MainWindowViewModel();
                    mainWindowViewModel.Initialize();
                }
                return mainWindowViewModel;
            }
        }

        /// <summary>
        /// Gets the Example ViewModel
        /// </summary>
        public ExampleViewModel ExampleViewModel
        {
            get
            {
                // Creates the Example ViewModel
                if (exampleViewModel == null)
                {
                    exampleViewModel = new ExampleViewModel();
                    exampleViewModel.Initialize();
                }
                return exampleViewModel;
            }
        }

        /// <summary>
        /// Gets the Page1ViewModel
        /// </summary>
        public Page1ViewModel Page1ViewModel
        {
            get
            {
                // Creates the Example ViewModel
                if (page1ViewModel == null)
                {
                    page1ViewModel = new Page1ViewModel();
                    page1ViewModel.Initialize();
                }
                return page1ViewModel;
            }
        }

        /// <summary>
        /// Gets the Page2ViewModel
        /// </summary>
        public Page2ViewModel Page2ViewModel
        {
            get
            {
                // Creates the Example ViewModel
                if (page2ViewModel == null)
                {
                    page2ViewModel = new Page2ViewModel();
                    page2ViewModel.Initialize();
                }
                return page2ViewModel;
            }
        }
    }
}
