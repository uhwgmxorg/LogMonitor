using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LogMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public NLog.Logger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public App()
        {
            try
            {
                NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("NLog.config");
                _logger = NLog.LogManager.GetCurrentClassLogger();
                string StrVersion;
#if DEBUG
                StrVersion = "Debug Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " Revision " + Tools.Globals._revision;
#else
                StrVersion = "Release Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " Revision " + Tools.Globals._revision;
#endif
                _logger.Info("Starting ComMonitor " + StrVersion);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
