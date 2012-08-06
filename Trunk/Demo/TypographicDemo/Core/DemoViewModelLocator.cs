using TypographicDemo.ViewModels;

namespace TypographicDemo.Core
{
    /// <summary>
    /// The ViewModel Locator handles the ViewModels of the application.
    /// I would recommend to use Ninject or something similar to handle ViewModel instances.
    /// </summary>
    public class DemoViewModelLocator
    {
        private MainWindowViewModel mainWindowViewModel;
        private ExampleViewModel exampleViewModel;

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
    }
}
