# ViewModelBase
The ViewModelBase class offers a lot of functions that are needed when programming with the MVVM pattern. This class should be the base class for every view model, except the MainWindow view model which should use the [MainWindowViewModelBase](MainWindowViewModelBase) class.

The most important part, that needs to be done, is to implement the Initialize() method.
{{
        // Must be called after creating the viewmodel
        public override void Initialize()
        {
        }

        // Will be called when the page get's displayed (while the navigation event)
        public override void InitializePage(object extraData)
        {
        }
}}
That is extremly important, because nearly every ViewModel needs to initialize some properties or load data. In that case, calling the base class must not be forgotten.

## Command Registration
The base class offers two methods which handles the registration of commands that can be bound to the view.

**RegisterApplicationCommand**
This method can be used to register an [Application Command](ApplicationCommand). The difference to an user command is, that the ApplicationCommand is predefined by either .NET or the TimePunch WPF Metro Library.
{{
        protected void RegisterApplicationCommand(
            ICommand commandToRegister, 
            Action<Object,ExecutedRoutedEventArgs> execute, 
            Action<Object,CanExecuteRoutedEventArgs> canExecute, 
            bool disableOnLoading)
}}
**RegisterCommand**
This method registers a [Relay Command](RelayCommand) of the view model. This method should be called **only** within the Initialize() method of the derived view model.
{{
        protected ICommand RegisterCommand(
            Action<Object,ExecutedRoutedEventArgs> execute, 
            Action<Object,CanExecuteRoutedEventArgs> canExecute, 
            bool disableOnLoading)
}}

## INotifyPropertyChangedEvent Handling
The property changed handling has been implemented in the BaseViewModel in order to relieve the developer of the same standard work all over the time.

Therefore there are two methods GetPropertyValue() and SetPropertyValue() that can be used within a concrete property. The SetPropertyValue() will inform all registered views by using a INotifyPropertyChangedEvent of the data update.

**Example**
{{
        public int SelectedMonth
        {
            get { return GetPropertyValue(() => SelectedMonth); }
            set { SetPropertyValue(() => SelectedMonth, value); }
        }
}}
## Property change notifications
Sometimes it's necessary that other properties or even commands are getting validated again when a property Change. Within the Initialize() method of the view model property change notifications can be registered.

**Command Target**
The first method can be used to register commands that should be validated if the source property changes.
{{
        protected void AddPropertyChangedNotification<TSource>(
             Expression<Func<TSource>> sourceProperty, 
             params ICommand[]() dependendCommands);
}}
For example this can be useful if a command is only enabled if a property has a special value.
{{
        public override void Initialize()
        {
            base.Initialize();

            // Revalidate the commands after the selected month changed
            AddPropertyChangedNotification(() => SelectedMonth, 
                PrevCommand, NextCommand);
        }

        public void CanExecuteNextCommand(object sender, 
                 CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = SelectedMonth<11;
        }

        public void CanExecutePrevCommand(object sender, 
                 CanExecuteRoutedEventArgs eventArgs)
        {
            eventArgs.CanExecute = SelectedMonth>0;
        }
}}

**Property Target**
The second method can be used to register other properties as a notification target. This for example can be useful if you're binding against readonly properties that are a combination of other source properties. Now one can bind a control against the SelectedText property that gets automatically notified  when the SelectedMonth changes.

{{
        public override void Initialize()
        {
            // Send Property Changed notifaction when the selected month is set
            AddPropertyChangedNotification(
                () => SelectedMonth, () => SelectedText);
        }

        public string SelectedText
        {
            get { return months[SelectedMonth](SelectedMonth); }
        }
}}

## Properties
The ViewModelBase offers several properties that makes life easier.

* **Error**
The error property can be set by any method or try/catch handler in the ViewModel to mark that an error has been occured.
{{
        public string Error
        {
            get { return GetPropertyValue(() => Error); }
            set { SetPropertyValue(() => Error, value); }
        }
}}
* **IsDefective**
The IsDefective property uses the result of the Error property and converts it to a boolean flag that can be used for binding controls.
{{        
        public bool IsDefective
        {
            get { return !string.IsNullOrEmpty(Error); }
        }
}}
* **ShowDefective**
The ShowDefective property defines, if an error message will be shown to the user. This is usually only the case if the user does not do any navigation or webrequest.
{{
        public bool ShowDefective
        {
            get { return IsDefective && !IsLoading; }
        }
}}
* **IsLoading**
The IsLoading property can be set if the application do something that takes time. E.g. a WebRequest or some Special Background tasks. Depending on the IsLoading property controls can get disabled.
{{
        public bool IsLoading
        {
            get
            {
                return GetPropertyValue(() => IsLoading);
            }

            set
            {
                // Quite a lot functionatliy in here ...
            }
}}
* **IsReady**
The IsReady property is the opposite of the IsLoading property and can be used accordingly.
{{
        public bool IsReady
        {
            get { return !IsLoading; }
        }
}}
* **IsDesignMode**
The IsDesignMode property is useful if you have to prevent code that can only be executed when the application is up and running.
{{
        public static bool IsDesignMode { get; }
}}
## Threading
The ViewModelBase class offers several methods to handle the Dispatcher.

* **Dispatch**
Using the Dispatch() method an action gets dispatched, but only if dispatching to the main thread is really necessary.
{{
        protected void Dispatch(Action action)
}}
* **DispatchDelayed**
DispatchDelayed() invokes the given action on the Dispatcher. Doesn't matter, if the Action really needs to be asynchronously invoked.
{{
        protected void DispatchDelayed(Action action)
}}
* **ExecuteAsync**
The ExecuteAsync() method executes an action on a worker thread. Optionally you can specify a delay time in milliseconds when the action starts.
{{
        protected void ExecuteAsync(Action action, int delay);

        protected void ExecuteAsync<T>(Action<T> action, T parameter, int delay);
}}