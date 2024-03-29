﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Documents;
using Microsoft.Phone.Tasks;
using workout7.Helpers;

namespace workout7
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();

            enableStreakCounter.IsChecked = SettingsHelper.StreakCounterEnabled;
        }

        private void webBrowser_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://well.blogs.nytimes.com/2013/05/09/the-scientific-7-minute-workout/");
            webBrowserTask.Show(); 
        }

        private void enableStreakCounter_Click(object sender, RoutedEventArgs e)
        {
            SettingsHelper.StreakCounterEnabled = (bool)enableStreakCounter.IsChecked;
        }
    }
}