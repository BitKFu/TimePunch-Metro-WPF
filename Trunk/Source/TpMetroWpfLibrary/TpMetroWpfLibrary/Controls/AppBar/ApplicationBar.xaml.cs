﻿// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;
using TimePunch.Metro.Wpf.Events;
using TimePunch.Metro.Wpf.Metro;
using TimePunch.MVVM.Controller;
using TimePunch.MVVM.EventAggregation;
using TimePunch.MVVM.ViewModels;

namespace TimePunch.Metro.Wpf.Controls.AppBar
{
    /// <summary>
    /// Interaction logic for ApplicationBar.xaml
    /// </summary>
    public partial class ApplicationBar : UserControl, IHandleMessage<HideMenuItemsRequest>, INotifyPropertyChanged
    {
        /// <summary>
        /// Image Source Dependency Property
        /// </summary>
        public static DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ApplicationBar));
        public static DependencyProperty IconsProperty = DependencyProperty.Register("Icons", typeof(ObservableCollection<ApplicationBarIcon>), typeof(ApplicationBar));

        // Property Changed Event
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new Application Bar 
        /// </summary>
        public ApplicationBar()
        {
            InitializeComponent();

            Icons = new ObservableCollection<ApplicationBarIcon>();
            Icons.CollectionChanged += OnIconCollectionChanged;

            MenuItems = new ObservableCollection<ApplicationBarMenuItem>();
            MenuItems.CollectionChanged += OnMenuItemCollectionChanged;

            MenuItemPanel.LayoutUpdated += AdjustHeightToShowOnlyIcons;
            SizeChanged += AdjustHorizontalSize;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;

            if (!ViewModelBase.IsDesignMode)
                Kernel.Instance?.EventAggregator.Subscribe(this);

            SetResourceReference(ImageSourceProperty, "BurgerMenu");
        }

        /// <summary>
        /// Unsubscribes the applicationbar from the event aggregator
        /// </summary>
        ~ApplicationBar()
        {
            if (!ViewModelBase.IsDesignMode)
                Kernel.Instance?.EventAggregator.Unsubscribe(this);
        }

        /// <summary>
        /// Initializes the navigation service
        /// </summary>
        /// <param name="sender">Sending UI Element</param>
        /// <param name="e">Routed Events</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            NavigationService ns;
            if (GetNavigationService(out ns)) return;

            // Add the handler
            ns.Navigating += HideOnNavigation;
            ns.Navigated += ShowWhenNavigated;
        }

        private bool GetNavigationService(out NavigationService ns)
        {
            FrameworkElement element = this;

            // Walk up, until we get an navigation service
            do
            {
                ns = NavigationService.GetNavigationService(element);
                element = element.Parent as FrameworkElement;
            } while (ns == null && element != null);
            if (ns == null)
                return true;
            return false;
        }

        /// <summary>
        /// de-Register the navigation service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            NavigationService ns;
            if (GetNavigationService(out ns)) return;

            // Remove the handler
            ns.Navigating -= HideOnNavigation;
            ns.Navigated -= ShowWhenNavigated;
        }

        #region State Mangement

        /// <summary>
        /// Visibility of the menuItems
        /// </summary>
        private Visibility menuItemVisibility = Visibility.Collapsed;

        /// <summary>
        /// True, if an animation is running
        /// </summary>
        private bool isAnimating;

        /// <summary>
        /// True, on the first start - this forces the window to slide in
        /// </summary>
        private bool firstStart = true;

        #endregion

        #region Animation Properties

        /// <summary>
        /// Sliding Animation
        /// </summary>
        DoubleAnimation slideAnimation;

        /// <summary>
        /// Duration of the Sliding Animation
        /// </summary>
        private static readonly TimeSpan AnimationDuration = TimeSpan.FromMilliseconds(200);

        #endregion

        #region Menu Icons

        /// <summary>
        /// Gets or sets the Application Bar Icons
        /// </summary>
        [Category("Behavior"), Description("Contains the icons shown in the application bar."), NotifyParentProperty(true)]
        public ObservableCollection<ApplicationBarIcon> Icons
        {
            get { return (ObservableCollection<ApplicationBarIcon>)GetValue(IconsProperty); }
            set
            {
                if (Icons == value)
                    return;

                SetValue(IconsProperty, value);
                OnNotifyPropertyChanged("Icons");
            }
        }

        /// <summary>
        /// Gets or sets the image source 
        /// </summary>
        [Category("Behavior"), DefaultValue(""), Description("Image source of the application bar icon"), NotifyParentProperty(true)]
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set
            {
                if (ImageSource == value)
                    return;

                SetValue(ImageSourceProperty, value);
                OnNotifyPropertyChanged("ImageSource");
            }
        }

        /// <summary>
        /// Notify on property change
        /// </summary>
        /// <param name="property">name of the property</param>
        private void OnNotifyPropertyChanged(string property)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// This method will be called, if the icon collection changes
        /// </summary>
        /// <param name="sender">The sending ui element</param>
        /// <param name="e">Event arguments</param>
        private void OnIconCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddIcons(e.NewStartingIndex, e.NewItems.Cast<ApplicationBarIcon>());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveIcons(e.OldItems.Cast<ApplicationBarIcon>());
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException("Replace is not implemented");
                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException("Move is not implemented");
                case NotifyCollectionChangedAction.Reset:
                    ResetIcons();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Reset all icons
        /// </summary>
        private void ResetIcons()
        {
            // Reset all icon panel childs
            IconPanel.Children.Clear();
        }

        /// <summary>
        /// Gets or sets the Data Context
        /// </summary>
        public new object DataContext
        {
            get { return base.DataContext; }
            set
            {
                base.DataContext = value;

                foreach (var icon in Icons)
                    icon.DataContext = value;

                foreach (var menuItem in MenuItems)
                    menuItem.DataContext = value;
            }
        }

        /// <summary>
        /// Add new Icons
        /// </summary>
        /// <param name="newStartingIndex">the starting index to add the icons</param>
        /// <param name="newItems">Enumerable collection of new application bar icons to add</param>
        private void AddIcons(int newStartingIndex, IEnumerable<ApplicationBarIcon> newItems)
        {
            foreach (var newItem in newItems)
            {
                // Set the DataContext of the child element
                newItem.DataContext = newItem.DataContext ?? DataContext;

                var stack = new StackPanel {Orientation = Orientation.Vertical, Tag = newItem};

                // Button 
                var content = new Image
                                  {
                                      Stretch = Stretch.UniformToFill,
                                      Width = (Double)TryFindResource("ApplicationBarIconSize"),
                                      Height= (Double)TryFindResource("ApplicationBarIconSize")
                                  };

                var button = new Button
                                  {
                                      Margin = new Thickness(12,3,12,3),
                                      Focusable = true,
                                      IsDefault = newItem.IsDefault,
                                      IsCancel = newItem.IsCancel
                                  };
                button.SetResourceReference(Control.StyleProperty, "ChromeButtonStyle");
                
                stack.Children.Add(button);

                // Description text
                var textBlock = new TextBlock()
                                    {
                                        Text = newItem.Description,
                                        HorizontalAlignment = HorizontalAlignment.Center,
                                        Focusable = newItem.Focusable
                                    };
                textBlock.SetResourceReference(Control.StyleProperty, "WinTextSmallStyle");
                stack.Children.Add(textBlock);

                // Add bindings
                var binding = new Binding() { Source = newItem, Path = new PropertyPath(ApplicationBarIcon.CommandProperty) };
                button.SetBinding(ButtonBase.CommandProperty, binding);

                binding = new Binding() { Source = newItem, Path = new PropertyPath(ApplicationBarIcon.DescriptionProperty) };
                textBlock.SetBinding(TextBlock.TextProperty, binding);

                var automationButton = new Binding(){Source = textBlock, Path = new PropertyPath(TextBlock.TextProperty) } ;
                button.SetBinding(AutomationProperties.NameProperty, automationButton);

                binding = new Binding() { Source = newItem, Path = new PropertyPath(ApplicationBarIcon.ImageSourceProperty)};
                content.SetBinding(Image.SourceProperty, binding);

                binding = new Binding() { Source = newItem, Path = new PropertyPath(ApplicationBarIcon.IsDefaultProperty)};
                button.SetBinding(Button.IsDefaultProperty, binding);

                binding = new Binding() { Source = newItem, Path = new PropertyPath(ApplicationBarIcon.IsCancelProperty)};
                button.SetBinding(Button.IsCancelProperty, binding);

                binding = new Binding() { Source = newItem, Path = new PropertyPath(VisibilityProperty) };
                stack.SetBinding(VisibilityProperty, binding);

                button.Resources.Add("Image", content);
                button.SetResourceReference(Button.ContentProperty, "Image");

                // Add it to the collection
                IconPanel.Children.Insert(newStartingIndex, stack);
                newStartingIndex++;
            }
        }

        /// <summary>
        /// Add new Icons
        /// </summary>
        /// <param name="oldItems">Enumerable collection of new application bar icons to remove</param>
        private void RemoveIcons(IEnumerable<ApplicationBarIcon> oldItems)
        {
            var removeableIcons = IconPanel.Children.Cast<FrameworkElement>()
                .Join(oldItems,
                      element => element.Tag, oldItem => oldItem,
                      (element, oldItem) => element).ToList();

            // Remove it from the iconpanel
            foreach (var toRemove in removeableIcons)
                IconPanel.Children.Remove(toRemove);
        }

        #endregion

        #region Menu Items

        /// <summary>
        /// Gets or sets the Application Bar Icons
        /// </summary>
        [Category("Behavior"), Description("Contains the menu items shown in the application bar.")]
        public ObservableCollection<ApplicationBarMenuItem> MenuItems { get; set; }

        /// <summary>
        /// This method will be called, if the MenuItem collection changes
        /// </summary>
        /// <param name="sender">The sending ui element</param>
        /// <param name="e">Event arguments</param>
        private void OnMenuItemCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddMenuItems(e.NewStartingIndex, e.NewItems.Cast<ApplicationBarMenuItem>());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveMenuItems(e.OldItems.Cast<ApplicationBarMenuItem>());
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException("Replace is not implemented");
                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException("Move is not implemented");
                case NotifyCollectionChangedAction.Reset:
                    ResetMenuItems();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Reset all MenuItems
        /// </summary>
        private void ResetMenuItems()
        {
            // Reset all MenuItem panel childs
            MenuItemPanel.Children.Clear();
        }

        /// <summary>
        /// Add new MenuItems
        /// </summary>
        /// <param name="newStartingIndex">the starting index to add the MenuItems</param>
        /// <param name="newItems">Enumerable collection of new application bar MenuItems to add</param>
        private void AddMenuItems(int newStartingIndex, IEnumerable<ApplicationBarMenuItem> newItems)
        {
            foreach (var newItem in newItems)
            {
                // Set the DataContext of the child element
                newItem.DataContext = newItem.DataContext ?? DataContext;

                // Button 
                var content = new TextBlock()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Style = (Style)TryFindResource("WinTextTitle3Style")
                    };

                var button = 
                    new Button()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Tag = newItem,
                        Style = (Style)TryFindResource("ChromeButtonStyle"),
                        Margin = (Thickness)TryFindResource("WinVerticalMargin"),
                        Content = content
                    };

                // Add bindings
                var binding = new Binding() {Source = newItem, Path = new PropertyPath(ApplicationBarMenuItem.CommandProperty)};
                button.SetBinding(ButtonBase.CommandProperty, binding);

                binding = new Binding() { Source = newItem, Path = new PropertyPath(ApplicationBarMenuItem.DescriptionProperty)};
                content.SetBinding(TextBlock.TextProperty, binding);

                binding = new Binding() { Source = newItem, Path = new PropertyPath(VisibilityProperty) };
                button.SetBinding(VisibilityProperty, binding);

                // Add it to the collection
                MenuItemPanel.Children.Insert(newStartingIndex, button);
                newStartingIndex++;
            }
        }

        /// <summary>
        /// Add new MenuItems
        /// </summary>
        /// <param name="oldItems">Enumerable collection of new application bar MenuItems to remove</param>
        private void RemoveMenuItems(IEnumerable<ApplicationBarMenuItem> oldItems)
        {
            var removeableMenuItems = MenuItemPanel.Children.Cast<FrameworkElement>()
                .Join(oldItems,
                      element => element.Tag, oldItem => oldItem,
                      (element, oldItem) => element).ToList();

            // Remove it from the MenuItempanel
            foreach (var toRemove in removeableMenuItems)
                MenuItemPanel.Children.Remove(toRemove);
        }

        #endregion

        #region Translation Methods

        /// <summary>
        /// Adjust the height of the control, so that only the icons are visible
        /// </summary>
        private void AdjustHeightToShowOnlyIcons(object sender, EventArgs eventArgs)
        {
            // If an animation is already running, than don't disturbe
            if (isAnimating)
                return;

            // Hide the window on the first start
            if (firstStart)
            {
                LayoutTranslation.Y = ActualHeight;
                firstStart = false;
            }

            // Calculate the new Y translation
            var menuItemHeight = 0.0;
            double newYPosition;
            RectangleGeometry newClipping;
            switch (menuItemVisibility)
            {
                case Visibility.Visible:
                    newYPosition = 0;
                    newClipping = new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight));
                    break;
                case Visibility.Hidden:
                    newYPosition = ActualHeight;
                    newClipping = new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight));
                    break;
                case Visibility.Collapsed:
                    newYPosition = menuItemHeight = MenuItemPanel.ActualHeight;
                    newClipping = new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight - (MenuItemPanel.ActualHeight>0 ? TransparentWindow.BORDER_SIZE : 0)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Check, if the NewYPosition changes to the current one
            if (newYPosition == LayoutTranslation.Y)
                return;

            // Set the new Clipping Geometry
            Clip = newClipping;

            // Create new animation and start it
            isAnimating = true;
            slideAnimation = new DoubleAnimation(newYPosition, new Duration(AnimationDuration))
                    {EasingFunction = new ExponentialEase
                                        {
                                            EasingMode = EasingMode.EaseOut
                                        }};

            slideAnimation.Completed += (EventHandler)
                ((s, e) =>
                {
                    var action = (Action) (() => Clip = new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight - menuItemHeight)));
                    try
                    {
                        Dispatcher.Invoke(action, DispatcherPriority.Normal,
                            CancellationToken.None, BaseController.InvocationTimeout);
                    }
                    catch (TimeoutException)
                    {
                        Trace.TraceWarning("Can't dispatch animation in time, so try to invoke async");
                        Dispatcher.InvokeAsync(action);
                    }

                    isAnimating = false;
                }); 
            LayoutTranslation.BeginAnimation(TranslateTransform.YProperty, slideAnimation);
        }

        /// <summary>
        /// Adjusts the Width in order to fill the window size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdjustHorizontalSize(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
                Clip = new RectangleGeometry(new Rect(0, 0, e.NewSize.Width, Clip != null ? Clip.Bounds.Height : e.NewSize.Height));
        }

        #endregion

        #region Behaviour Implementation

        /// <summary>
        /// Switches the visiblity of the menu items
        /// </summary>
        private void SwitchMenuItemVisibility(object sender, RoutedEventArgs e)
        {
            switch (menuItemVisibility)
            {
                case Visibility.Visible:
                    menuItemVisibility = Visibility.Collapsed;
                    break;
                case Visibility.Hidden:
                    break;
                case Visibility.Collapsed:
                    menuItemVisibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            AdjustHeightToShowOnlyIcons(sender, e);
        }

        /// <summary>
        /// This method show the pane when navigation has been finished
        /// </summary>
        private void ShowWhenNavigated(object sender, NavigationEventArgs e)
        {
            menuItemVisibility = Visibility.Collapsed;
        }

        /// <summary>
        /// This method hides the pane on navigation
        /// </summary>
        private void HideOnNavigation(object sender, NavigatingCancelEventArgs e)
        {
            menuItemVisibility = Visibility.Hidden;
        }

        #endregion

        /// <summary>
        /// Handles a message of a specific type.
        /// </summary>
        /// <param name="message">the message to handle</param>
        public void Handle(HideMenuItemsRequest message)
        {
            menuItemVisibility = Visibility.Collapsed;
            AdjustHeightToShowOnlyIcons(this, null);
        }
    }
}
