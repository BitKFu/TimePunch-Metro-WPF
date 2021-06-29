// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Linq;
using TimePunch.Metro.Wpf.Helper;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace TimePunch.Metro.Wpf.Docking
{
    /// <summary>
    /// Implements a application task bar docking mechanism
    /// 
    /// This Docker, originally implemented by ColbyAfrica.Components.TaskBarDocker
    /// has been extended in order to enable a window to dock to all edges.
    /// 
    /// For more info see <see cref="http://code.msdn.microsoft.com/windowsdesktop/Task-Bar-Docking-Component-0975173a/view/SourceCode"/>
    /// </summary>
    public partial class TaskBarDocker
    {
        #region Instance Data

        private readonly Window form;
        private readonly Timer timer;
        private bool isFirstRun = true;
        private TaskBar.TaskBarEdge lastEdge;
        private Size lastSize;
        private Point lastLocation;
        private TaskBar.TaskBarEdge dockedTo = TaskBar.TaskBarEdge.NotDocked;

        public event Action<TaskBar.TaskBarEdge> OnDockingChanged = null;
       
        #endregion

        #region Private Constants
        private const int TIMER_INTERVAL = 1000;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the TaskBarDocker class
        /// </summary>
        /// <param name="form">WCF Window that needs to be adjusted</param>
        /// <param name="attachToTaskBarEvent"></param>
        public TaskBarDocker(Window form, bool attachToTaskBarEvent)
        {
            if (form == null)
                throw new ArgumentNullException("form");

            this.form = form;
            AttachedToTaskBarEvent = attachToTaskBarEvent;

            if (attachToTaskBarEvent)
            {
                timer = new Timer {Interval = TIMER_INTERVAL};
                timer.Tick += TimerTick;
                timer.Enabled = true;
            }
        }

        #endregion

        #region Public Properties

        public bool AttachedToTaskBarEvent { get; set; }

        public TaskBar.TaskBarEdge Position
        {
            get
            {
                return TaskBar.GetTaskBarEdge();
            }
        }

        #endregion

        #region Public Methods

        public void Dock(TaskBar.TaskBarEdge dockTo, int nr)
        {
            // Store the current docking
            dockedTo = dockTo;
            activeScreenIndex = nr;

            switch (dockTo)
            {
                case TaskBar.TaskBarEdge.Left:
                    PositionLeft();
                    break;
                case TaskBar.TaskBarEdge.Top:
                    PostionTop();
                    break;
                case TaskBar.TaskBarEdge.Right:
                    PostionRight();
                    break;
                case TaskBar.TaskBarEdge.Bottom:
                    PostionBottom();
                    break;

                case TaskBar.TaskBarEdge.NotDocked:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (OnDockingChanged != null)
                OnDockingChanged(dockTo);
        }

        #endregion

        #region Private Methods


        private void PositionLeft()
        {
            Screen? activeScreen = ActiveScreen;
            if (activeScreen == null)
            {
                ActiveScreenIndex = 0;
                activeScreen = ActiveScreen;
                if (activeScreen == null)
                    return;
            }

            var screenResolution = new ScreenResolution(form);
            var left = activeScreen.WorkingArea.Left;
            var top = activeScreen.WorkingArea.Top;
            var height = activeScreen.WorkingArea.Height;

            form.Left = screenResolution.ConvertXDpi(left);
            form.Top = screenResolution.ConvertYDpi(top);
            form.Height = screenResolution.ConvertYDpi(height);
        }
        
        public TaskBar.TaskBarEdge DockedTo
        {
            get { return dockedTo; }
        }

        public Screen? ActiveScreen => ActiveScreenIndex < Screen.AllScreens.Length ? Screen.AllScreens[ActiveScreenIndex] : null;

        private int activeScreenIndex = 0;
        public int ActiveScreenIndex 
        {
            get { return activeScreenIndex; }
            set { activeScreenIndex = Math.Max(0, Math.Min(value, Screen.AllScreens.Count()-1)); }
        }

        private void PostionTop()
        {
            Screen? activeScreen = ActiveScreen;
            if (activeScreen == null)
            {
                ActiveScreenIndex = 0;
                activeScreen = ActiveScreen;
                if (activeScreen == null)
                    return;
            }

            var screenResolution = new ScreenResolution(form);
            var left = activeScreen.WorkingArea.Left;
            var top = activeScreen.WorkingArea.Top;
            var width = activeScreen.WorkingArea.Width;

            form.Top = screenResolution.ConvertYDpi(top);
            form.Left = screenResolution.ConvertXDpi(left);
            form.Width = screenResolution.ConvertXDpi(width);
        }

        private void PostionRight()
        {
            Screen? activeScreen = ActiveScreen;
            if (activeScreen == null)
            {
                ActiveScreenIndex = 0;
                activeScreen = ActiveScreen;
                if (activeScreen == null)
                    return;
            }

            var screenResolution = new ScreenResolution(form);
            var left = activeScreen.WorkingArea.Left + activeScreen.WorkingArea.Width - screenResolution.ConvertXToScreen(form.Width);
            var top = activeScreen.WorkingArea.Top;
            var height = activeScreen.WorkingArea.Height;

            form.Left = screenResolution.ConvertXDpi(left);
            form.Top = screenResolution.ConvertYDpi(top);
            form.Height = screenResolution.ConvertYDpi(height);
        }

        private void PostionBottom()
        {
            Screen? activeScreen = ActiveScreen;
            if (activeScreen == null)
            {
                ActiveScreenIndex = 0;
                activeScreen = ActiveScreen;
                if (activeScreen == null)
                    return;
            }

            var screenResolution = new ScreenResolution(form);
            var left = activeScreen.WorkingArea.Left;
            var top = activeScreen.WorkingArea.Top + activeScreen.WorkingArea.Height - screenResolution.ConvertYToScreen(form.Height);
            var width = activeScreen.WorkingArea.Width;

            form.Top = screenResolution.ConvertYDpi(top);
            form.Left = screenResolution.ConvertXDpi(left);
            form.Width = screenResolution.ConvertXDpi(width);
        }

        #endregion

        #region Event Handlers

        private void TimerTick(object sender, EventArgs e)
        {
            try
            {
                Point currentPosition = TaskBar.GetTaskBarLocation();
                Size currentSize = TaskBar.GetTaskBarSize();
                TaskBar.TaskBarEdge currentEdge = TaskBar.GetTaskBarEdge();

                if (lastLocation != currentPosition || lastEdge != currentEdge || lastSize != currentSize)
                {
                    lastLocation = currentPosition;
                    lastEdge = currentEdge;
                    lastSize = currentSize;

                    if (!isFirstRun)
                        Dock(DockedTo, ActiveScreenIndex);

                    isFirstRun = false;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        #endregion

        /// <summary>
        /// Used to Update the display settings
        /// </summary>
        public void Update()
        {
            Dock(DockedTo, ActiveScreenIndex);
        }
    }

    public static class ScreenExtension
    {
        public static bool DeviceEquals(this Screen screen, Screen compareTo)
        {
            return screen.DeviceName == compareTo.DeviceName;
        }
    }
}