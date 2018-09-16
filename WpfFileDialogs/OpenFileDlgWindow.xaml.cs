using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace WpfFileDialogs
{
    /// <summary>
    /// Interaktionslogik for OpenFileDlgWindow.xaml
    /// WpfFileDialogs lazy loading logic comes from manuc66 on GitHub:
    /// https://github.com/manuc66/LazyTreeView
    /// </summary>
    public partial class OpenFileDlgWindow : MetroWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private const string LIST_FILE_NAME = "OpenFileDlgWindowRecentFiles.xml";

        public enum OpenFileDlgWindowReturnCode
        {
            Ok = 1,
            Camcel
        }

        public event RoutedEventHandler OkClick;
        public event RoutedEventHandler CancelClick;

        public OpenFileDlgWindowReturnCode OFDWReturnCode { get; set; }

        private string longFileNameToolTip;
        public string LongFileNameToolTip
        {
            get { return longFileNameToolTip; }
            set { SetField(ref longFileNameToolTip, value, nameof(LongFileNameToolTip)); }
        }

        public RecentFiles _theRrecentFiles = new RecentFiles();
        private ObservableCollection<FileName> fileList;
        public ObservableCollection<FileName> FileList
        {
            get { return fileList; }
            set { SetField(ref fileList, value, nameof(FileList)); }
        }
        private FileName selectedFile;
        public FileName SelectedFile
        {
            get { return selectedFile; }
            set { SetField(ref selectedFile, value, nameof(SelectedFile)); if (selectedFile != null) LongFileNameToolTip = selectedFile.LongName; else LongFileNameToolTip = "..."; }
        }

        private ObservableCollection<string> filterList;
        public ObservableCollection<string> FilterList
        {
            get { return filterList; }
            set { SetField(ref filterList, value, nameof(FilterList)); }
        }
        private string selectedFilter;
        public string SelectedFilter
        {
            get { return selectedFilter; }
            set { SetField(ref selectedFilter, value, nameof(SelectedFilter)); }
        }
       
        /// <summary>
        /// Constructor
        /// </summary>
        public OpenFileDlgWindow()
        {
            InitializeComponent();
            DataContext = this;

            FileList = new ObservableCollection<FileName>();
            FilterList = new ObservableCollection<string>();
            FilterList.Add("*.log");
            FilterList.Add("*.txt");
            FilterList.Add("*.log|*txt");
            FilterList.Add("*.*");

            SelectedFilter = FilterList[0];
            var itemFactory = new ItemFactory(SelectedFilter);
            fileSystemTreeView.ItemsSource = Environment.GetLogicalDrives().Select(itemFactory.CreateItem);
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        /// <summary>
        /// Button_Click_Clear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            FileList.Clear();
        }
        
        /// <summary>
        /// Button_Click_Refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Refresh(object sender, RoutedEventArgs e)
        {
            var itemFactory = new ItemFactory(SelectedFilter);
            fileSystemTreeView.ItemsSource = Environment.GetLogicalDrives().Select(itemFactory.CreateItem);
        }

        /// <summary>
        /// Button_Click_Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            OFDWReturnCode = OpenFileDlgWindowReturnCode.Camcel;
            CancelClick?.Invoke(null, null);
            Hide();
        }

        /// <summary>
        /// Button_Click_Ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Ok(object sender, RoutedEventArgs e)
        {
            OFDWReturnCode = OpenFileDlgWindowReturnCode.Ok;
            OkClick?.Invoke(null, null);
            Hide();
            SaveFileList();
        }

        #endregion
        /******************************/
        /*      Menu Events           */
        /******************************/
        #region Menu Events

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
            ButtonOk.Focus();
            LoadFileList();
        }

        /// <summary>
        /// FileSystemTreeView_SelectedItemChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileSystemTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            FileAttributes attr = FileAttributes.Directory;

            try
            {
                attr = File.GetAttributes(((Item)fileSystemTreeView.SelectedItem).LongName);
            }
            catch(IOException){ return; }

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                return;

            var item = FileList.SingleOrDefault(x => x.LongName == ((Item)fileSystemTreeView.SelectedItem).LongName);
            if (item == null)
                FileList.Add(new FileName { SchortName = ((Item)fileSystemTreeView.SelectedItem).ShortName, LongName = ((Item)fileSystemTreeView.SelectedItem).LongName });
            else
            {
                SelectedFile = item;
                return;
            }

            SelectedFile = FileList[FileList.Count - 1];
        }

        /// <summary>
        /// ComboBox_SelectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChangedFiles(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        /// <summary>
        /// ComboBox_SelectionChangedFilter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChangedFilter(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Button_Click_Refresh(sender, null);
        }

        /// <summary>
        /// MetroWindow_Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        /// <summary>
        /// MetroWindow_Closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Closed(object sender, EventArgs e)
        {
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// LoadFileList
        /// </summary>
        private void LoadFileList()
        {
            LST.LoadClass<RecentFiles>(ref _theRrecentFiles, LIST_FILE_NAME);
            FileList = _theRrecentFiles.FileList;if (FileList == null) FileList = new ObservableCollection<FileName>();
            SelectedFile = _theRrecentFiles.SelectedFile; if (SelectedFile == null) SelectedFile = new FileName();
            SelectedFile = FileList.SingleOrDefault(x => x.LongName == SelectedFile.LongName);
        }

        /// <summary>
        /// SaveFileList
        /// </summary>
        private void SaveFileList()
        {
            _theRrecentFiles.FileList = FileList;
            _theRrecentFiles.SelectedFile = SelectedFile;
            LST.SaveClass<RecentFiles>(_theRrecentFiles, LIST_FILE_NAME);
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

    #region Help Classes
    public class RecentFiles
    {
        public ObservableCollection<FileName> FileList { get; set; }
        public FileName SelectedFile { get; set; }
    }

    public class FileName
    {
        public string SchortName { get; set; }
        public string LongName { get; set; }
    }
    #endregion
}
