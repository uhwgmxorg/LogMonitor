using MahApps.Metro.Controls.Dialogs;
using System;
using System.IO;

namespace LogMonitor.Dialogs
{
    /// <summary>
    /// Interaction logic for UCChangeLog.xaml
    /// </summary>
    public partial class UCChangeLog : CustomDialog
    {
        public UCChangeLog()
        {
            InitializeComponent();

            try
            {
                ChangeLog.Text = File.ReadAllText("ChangeLog.txt"); ;
            }
            catch (Exception)
            {
                ChangeLog.Text = "The\n ChangeLog-File\n is missing";
            }
        }
    }
}
