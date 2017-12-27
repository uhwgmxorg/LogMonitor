using ControlPanels;
using LogMonitor.Dialogs;
using LogMonitor.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WpfFileDialogs;
using System.Linq;

namespace LogMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private UCAboutBox _aboutDlg = new UCAboutBox();
        private ObservableCollection<DragDockPanel> _panels = new ObservableCollection<DragDockPanel>();
        private OpenFileDlgWindow _openFileDlgWindow;

        private int _panelNo = 0;

        #region INotify Propertie Changed
        private ObservableCollection<MenuItem> contextMenuItemList;
        public ObservableCollection<MenuItem> ContextMenuItemList
        {
            get { return contextMenuItemList; }
            set { SetField(ref contextMenuItemList, value, nameof(ContextMenuItemList)); }
        }
        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            DragDockPanelHostWithItemTemplate.ItemsSource = _panels;
            DragDockPanelHostWithItemTemplate.MinimizedPosition = MinimizedPositions.Right;

            _aboutDlg.Close_Button.Click += CloseDialog;

            _openFileDlgWindow = new OpenFileDlgWindow();
            _openFileDlgWindow.Closed += (o, args) => _openFileDlgWindow.Hide();
            _openFileDlgWindow.OkClick += OnOpenFileDlgWindowReturnCodeOk;
            _openFileDlgWindow.CancelClick += OnOpenFileDlgWindowReturnCodeCancel;

            ContextMenuItemList = new ObservableCollection<MenuItem>();
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        /// <summary>
        /// Button_Click_Add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            OpenFileDlgWindow();
        }

        /// <summary>
        /// Button_Click_Remove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Remove(object sender, RoutedEventArgs e)
        {
            if (_panels.Count == 0)
                return;
            _panels.RemoveAt(_panels.Count-1);
            ContextMenuItemList.RemoveAt(ContextMenuItemList.Count-1);
        }

        /// <summary>
        /// Button_Click_About
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_About(object sender, RoutedEventArgs e)
        {
            ShowDialog();
        }
        private new void ShowDialog()
        {
            DialogManager.ShowMetroDialogAsync(this, _aboutDlg);
        }

        #endregion
        /******************************/
        /*      Menu Events          */
        /******************************/
        #region Menu Events

        /// <summary>
        /// ContextMenuItem_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.OriginalSource as MenuItem;
            menuItem.Click -= ContextMenuItem_Click;
            var panelNo = System.Text.RegularExpressions.Regex.Match((string)menuItem.Header, @"\d+$").Value;

            RemovePannel(Convert.ToInt32(panelNo));
        }

        #endregion
        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        /// <summary>
        /// MetroWindow_Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Set Window Title
            string Version;
#if DEBUG
            Version = "    Debug Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
#else
            Version = "    Release Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
#endif
            Title += Version;
        }

        /// <summary>
        /// OnOpenFileDlgWindowReturnCodeOk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnOpenFileDlgWindowReturnCodeOk(object sender, RoutedEventArgs e)
        {
            if(_openFileDlgWindow.SelectedFile != null)
                AddPanel(_openFileDlgWindow.SelectedFile.LongName);
        }

        /// <summary>
        /// OnOpenFileDlgWindowReturnCodeCancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnOpenFileDlgWindowReturnCodeCancel(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// CloseDialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CloseDialog(object sender, RoutedEventArgs e)
        {
            await this.HideMetroDialogAsync(_aboutDlg);
        }

        /// <summary>
        /// MetroWindow_Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// OpenFileDlgWindow
        /// </summary>
        private void OpenFileDlgWindow()
        {
            _openFileDlgWindow.BorderThickness = new Thickness(1);
            _openFileDlgWindow.BorderBrush = null;
            _openFileDlgWindow.SetResourceReference(MetroWindow.GlowBrushProperty, "AccentColorBrush");
            _openFileDlgWindow.Show();
            _openFileDlgWindow.Activate();
        }

        /// <summary>
        /// AddPanel
        /// </summary>
        private void AddPanel(string fileName)
        {
            string truncatedPath;
            int truncat = 20;
            
            if (fileName.Length < truncat)
                truncatedPath = fileName;
            else
                truncatedPath = fileName.Substring(0, 16) + "..." + fileName.Substring((fileName.Length - truncat), truncat);
            // Panel adding
            _panels.Add(new DragDockPanel()
            {
                Margin = new Thickness(5),
                Header = string.Format("{0} {1} File: {2}", "Panel", ++_panelNo, truncatedPath),
                Content = new UCLogListBox()
                {
                    LongFileName = fileName,
                },
            });
            MenuItem menuItem = new MenuItem();
            menuItem.Header = string.Format("Remove Panel {0}", _panelNo);
            menuItem.ToolTip = fileName;
            menuItem.Click += ContextMenuItem_Click;
            ContextMenuItemList.Add(menuItem);
        }

        /// <summary>
        /// RemovePannel
        /// </summary>
        private void RemovePannel(int no)
        {
            if (_panels.Count == 0)
                return;

            string searchString = string.Format("{0} {1}", "Panel", no);
            var itemp = _panels.FirstOrDefault(x => ((string)x.Header).Contains(searchString));
            _panels.Remove(itemp);
            var itemm = ContextMenuItemList.FirstOrDefault(x => ((string)x.Header).Contains(searchString));
            ContextMenuItemList.Remove(itemm);

            if (_panels.Count == 0)
                _panelNo = 0;
        }

        /// <summary>
        /// SetField
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        private void OnPropertyChanged(string p)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }

        #endregion
    }
}
