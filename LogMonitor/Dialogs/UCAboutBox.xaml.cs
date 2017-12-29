using LogMonitor.Tools;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace LogMonitor.Dialogs
{
    /// <summary>
    /// Interaktionslogik für UCAboutBox.xaml
    /// </summary>
    public partial class UCAboutBox : CustomDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UCAboutBox()
        {
            InitializeComponent();
            DataContext = this;

            string version;
#if DEBUG
            version = "Debug Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " Rev " + Globals._revision;
#else
            version = "Release Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " Rev " + Globals._revision;
#endif
            Version.Text = version;

            try
            {
                ScrollText.Text = File.ReadAllText("LICENSE"); ;
            }
            catch (Exception)
            {
                ScrollText.Text = "The\n MIT License-File\n is missing";
            }
        }

        /// <summary>
        /// Button_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnClose();
        }

        /// <summary>
        /// Label_LogMonitorMouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_LogMonitorMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://uhwgmxorg.com/LogMonitor/"));
        }

        /// <summary>
        /// Label_ChangeLogMouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_ChangeLogMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://uhwgmxorg.com/LogMonitor/changelog.html"));
        }

    }
}
