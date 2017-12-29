using MahApps.Metro.Controls.Dialogs;
using System;
using System.IO;

namespace LogMonitor.Dialogs
{
    /// <summary>
    /// Interaction logic for UCListOfKnownBugs.xaml
    /// </summary>
    public partial class UCListOfKnownBugs : CustomDialog
    {
        public UCListOfKnownBugs()
        {
            InitializeComponent();

            try
            {
                ListOfKnownBugs.Text = File.ReadAllText("ListOfKnownBugs.txt"); ;
            }
            catch (Exception)
            {
                ListOfKnownBugs.Text = "The\n ChangeLog-File\n is missing";
            }
        }
    }
}
