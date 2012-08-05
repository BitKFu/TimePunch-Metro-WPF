﻿using TimePunch.Metro.Wpf.Controller;

namespace ApplicationBarDemo.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
            Kernel.Instance.Controller.SetRootFrame(ContentFrame);
        }
    }
}
