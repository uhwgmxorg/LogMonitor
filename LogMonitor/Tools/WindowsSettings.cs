using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;

namespace LogMonitor.Tools
{
    public class WindowsSettings
    {
        private Window _window = null;
        private WindowApplicationSettings _windowApplicationSettings = null;

        /// <summary>
        /// Register the "Save" attached property and the "OnSaveInvalidated" callback 
        /// </summary>
        public static readonly DependencyProperty SaveProperty = DependencyProperty.RegisterAttached("Save", typeof(bool), typeof(WindowsSettings), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSaveInvalidated)));

        [Browsable(false)]
        public WindowApplicationSettings Settings
        {
            get
            {
                if (_windowApplicationSettings == null)
                {
                    this._windowApplicationSettings = CreateWindowApplicationSettingsInstance();
                }
                return this._windowApplicationSettings;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_window"></param>
        public WindowsSettings(Window window)
        {
            _window = window;
        }

        /// <summary>
        /// SetSave
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="enabled"></param>
        public static void SetSave(DependencyObject dependencyObject, bool enabled)
        {
            dependencyObject.SetValue(SaveProperty, enabled);
        }

        /// <summary>
        /// Called when Save is changed on an object.
        /// </summary>
        private static void OnSaveInvalidated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Window Window = dependencyObject as Window;
            if (Window != null)
            {
                if ((bool)e.NewValue)
                {
                    WindowsSettings settings = new WindowsSettings(Window);
                    settings.Attach();
                }
            }
        }

        /// <summary>
        /// Load the Window Size Location and State from the settings object
        /// </summary>
        protected virtual void LoadWindowState()
        {
            Settings.Reload();
            if (Settings.Location != Rect.Empty)
            {
                _window.Left = Settings.Location.Left;
                _window.Top = Settings.Location.Top;
                _window.Width = Settings.Location.Width;
                _window.Height = Settings.Location.Height;
            }

            if (Settings.WindowState != WindowState.Maximized)
            {
                _window.WindowState = Settings.WindowState;
            }
        }

        /// <summary>
        /// Save the Window Size, Location and State to the settings object
        /// </summary>
        protected virtual void SaveWindowState()
        {
            Settings.WindowState = _window.WindowState;
            Settings.Location = _window.RestoreBounds;
            Settings.Save();
        }

        /// <summary>
        /// Attach
        /// </summary>
        private void Attach()
        {
            if (_window != null)
            {
                _window.Closing += new CancelEventHandler(WindowClosing);
                _window.Initialized += new EventHandler(WindowInitialized);
                _window.Loaded += new RoutedEventHandler(WindowLoaded);
            }
        }

        /// <summary>
        /// WindowLoaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (this.Settings.WindowState == WindowState.Maximized)
            {
                _window.WindowState = this.Settings.WindowState;
            }
        }

        /// <summary>
        /// WindowInitialized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowInitialized(object sender, EventArgs e)
        {
            LoadWindowState();
        }

        /// <summary>
        /// WindowClosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClosing(object sender, CancelEventArgs e)
        {
            SaveWindowState();
        }

        /// <summary>
        /// CreateWindowApplicationSettingsInstance
        /// </summary>
        /// <returns></returns>
        protected virtual WindowApplicationSettings CreateWindowApplicationSettingsInstance()
        {
            return new WindowApplicationSettings(this);
        }

        #region WindowApplicationSettings Helper Class
        public class WindowApplicationSettings : ApplicationSettingsBase
        {
            private WindowsSettings _windowSettings;

            [UserScopedSetting]
            public Rect Location
            {
                get
                {
                    if (this["Location"] != null)
                    {
                        return ((Rect)this["Location"]);
                    }
                    return Rect.Empty;
                }
                set
                {
                    this["Location"] = value;
                }
            }

            [UserScopedSetting]
            public WindowState WindowState
            {
                get
                {
                    if (this["WindowState"] != null)
                    {
                        return (WindowState)this["WindowState"];
                    }
                    return WindowState.Normal;
                }
                set
                {
                    this["WindowState"] = value;
                }
            }

#pragma warning disable 618 // PersistId is an obsolete property and may be removed in a future release.  The value of this property is not defined.
            /// <summary>
            /// Construcor
            /// </summary>
            /// <param name="_windowSettings"></param>
            public WindowApplicationSettings(WindowsSettings windowSettings) : base(windowSettings._window.PersistId.ToString())
            {
                _windowSettings = windowSettings;
            }
#pragma warning restore
        }
        #endregion
    }
}
