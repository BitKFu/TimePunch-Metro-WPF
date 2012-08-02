// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace TimePunch.Metro.Wpf.Frames
{

    /// <summary>
    /// The FaderFrame is based on the work of Dr.WPF 2007.
    /// I enhanced it with some features that allows stacking page navigations. 
    /// The previous implementation failed when a navigation event is directly followed by a second one.
    /// 
    /// Original implementation can be found here <see cref="http://drwpf.com/blog/Portals/0/Code/FaderFrame.cs.txt"/>
    /// </summary>
    public class FaderFrame : Frame
    {
        #region FadeDuration

        public static readonly DependencyProperty FadeDurationProperty =
            DependencyProperty.Register("FadeDuration", typeof(Duration), typeof(FaderFrame),
                new FrameworkPropertyMetadata(new Duration(TimeSpan.FromMilliseconds(300))));

        public static readonly DependencyProperty FadeOffsetProperty = 
            DependencyProperty.Register("FadeOffset", typeof(TimeSpan), typeof(FaderFrame),
                new FrameworkPropertyMetadata(TimeSpan.FromMilliseconds(0)));

        /// <summary>
        /// FadeDuration will be used as the duration for Fade Out and Fade In animations
        /// </summary>
        public Duration FadeDuration
        {
            get { return (Duration)GetValue(FadeDurationProperty); }
            set { SetValue(FadeDurationProperty, value); }
        }

        /// <summary>
        /// FadeOffset defines, how long the thread sleeps until the fade starts
        /// </summary>
        public TimeSpan FadeOffset
        {
            get { return (TimeSpan) GetValue(FadeOffsetProperty); }
            set { SetValue(FadeOffsetProperty, value); }
        }

        #endregion

        public FaderFrame()
        {
            // watch for navigations
            Navigating += OnNavigating;
            NavigationService.Navigated += FadeOutCompleted;
        }

        public override void OnApplyTemplate()
        {
            // get a reference to the frame's content presenter
            // this is the element we will fade in and out
            contentPresenter = GetTemplateChild("PART_FrameCP") as ContentPresenter;
            base.OnApplyTemplate();
        }

        protected void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (Content == null || contentPresenter == null || e.NavigationMode == NavigationMode.Refresh)
                return;

            // if we did not internally initiate the navigation:
            //   1. cancel the navigation,
            //   2. cache the target,
            //   3. disable hittesting during the fade, and
            //   4. fade out the current content
            if (!navigationEvents.Contains(e.Uri))
            {
                e.Cancel = true;
                navArgs.Enqueue(e);

                if (navArgs.Count == 1)
                {
                    IsHitTestVisible = contentPresenter.IsHitTestVisible = false;

                    // Start Animation
                    var da = new DoubleAnimation(0.0d, FadeDuration)
                                 {
                                     DecelerationRatio = 1.0d,
                                     BeginTime = FadeOffset
                                 };

                    da.Completed += FadeOutCompleted;
                    contentPresenter.BeginAnimation(OpacityProperty, da, HandoffBehavior.Compose);
                }
            }
            else
            {
                e.Cancel = false;
                navigationEvents.Remove(e.Uri);
            }
        }

        private void FadeOutCompleted(object sender, EventArgs e)
        {
            if (contentPresenter == null || navArgs.Count == 0)  
                return;

            IsHitTestVisible = contentPresenter.IsHitTestVisible = true;

            var nav = navArgs.Dequeue();
            navigationEvents.Add(nav.Uri);
            switch (nav.NavigationMode)
            {
                case NavigationMode.New:
                    if (nav.Uri == null)
                    {
                        NavigationService.Navigate(nav.Content, nav.ExtraData);
                    }
                    else
                    {
                        NavigationService.Navigate(nav.Uri, nav.ExtraData);
                    }
                    break;

                case NavigationMode.Back:
                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();
                    break;

                case NavigationMode.Forward:
                    if (NavigationService.CanGoForward)
                        NavigationService.GoForward();
                    break;

                case NavigationMode.Refresh:
                    NavigationService.Refresh();
                    break;
            }

            // Start Animation
            var da = new DoubleAnimation(1.0d, FadeDuration) {AccelerationRatio = 1.0d};
            contentPresenter.BeginAnimation(OpacityProperty, da, HandoffBehavior.Compose);
        }

        private ContentPresenter contentPresenter = null;
        private readonly HashSet<Uri> navigationEvents = new HashSet<Uri>();
        private readonly Queue<NavigatingCancelEventArgs> navArgs = new Queue<NavigatingCancelEventArgs>();
    }
}
