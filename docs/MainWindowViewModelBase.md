# MainWindow view model base
The MainWindowViewModelBase class should be used as the base class for the main window. The main window has to be handled slightly different to all other view models because it has to handle some Application Commands like Close, Minimize, Maximize and Normalize.

These commands are getting registered by that base class and have a standard implementation that should fit the most needs.

If you actually do need a special handling for such functionality you can overwrite the following virtual methods:

{{
public virtual void CanExecuteCloseCommand(
     object sender, 
     CanExecuteRoutedEventArgs e);

public virtual void CanExecuteMaximizeCommand(
     object sender, 
     CanExecuteRoutedEventArgs eventArgs);

public virtual void CanExecuteMinimizeCommand(
     object sender, 
     CanExecuteRoutedEventArgs eventArgs);

public virtual void CanExecuteNormalizeCommand(
     object sender, 
     CanExecuteRoutedEventArgs eventArgs)
}}
Because the MainWindowViewModelBase class is derived from the [ViewModelBase](ViewModelBase), you can use all the functionality that has been described.