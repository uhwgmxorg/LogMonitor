using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace LogMonitor.Views
{
    /// <summary>
    /// Interaktionslogik für MyListBoxLog.xaml
    /// </summary>
    public partial class UCLogListBox : UserControl
    {
        public event PropertyChangedEventHandler PropertyChanged;
        delegate void DMessageAdd(string message, System.Windows.Media.Brush color);

        private const int MAX_MESSGAES = 200;

        private object _lockObj = new object();
        private int _counter = 0;
        private FileSystemWatcher _watcher;

        private long _startLineTotal;
        private long _lastFileSize;

        public string LongFileName { get; set; }

        bool stopWatching;
        public bool StopWatching
        {
            get
            {
                lock (_lockObj)
                {
                    return stopWatching;
                }
            }
            set
            {
                lock (_lockObj)
                {
                    stopWatching = value;
                }
            }
        }

        string Message { get; set; }
        public FontFamily DisplayFontFamily { get; set; }
        private System.Windows.Media.Brush MessageColor { get; set; }
        public virtual Dispatcher DispatcherObject { get; protected set; }
        public ObservableCollection<ListBoxLogMessageString> MessageList { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public UCLogListBox()
        {
            InitializeComponent();
            DataContext = this;

            DispatcherObject = Dispatcher.CurrentDispatcher;
            MessageList = new ObservableCollection<ListBoxLogMessageString>();

            DisplayFontFamily = new FontFamily("Courier New");
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        #endregion
        /******************************/
        /*      Menu Events          */
        /******************************/
        #region Menu Events

        #endregion
        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        /// <summary>
        /// UserControl_Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _startLineTotal = GetTotalLinesInFile(LongFileName,ref _lastFileSize);
            // Estimate the position of the last 200 lines
            try { _lastFileSize = _lastFileSize - (_lastFileSize / _startLineTotal * MAX_MESSGAES); } catch (Exception) { }
            WatchLogFile(LongFileName);
            StopWatching = false;
            Task.Factory.StartNew(() =>
            {
                string path = Path.GetDirectoryName(LongFileName);
                string baseName = Path.GetFileName(LongFileName);
                _watcher = new System.IO.FileSystemWatcher(path, baseName);
                FileSystemEventHandler handler = new FileSystemEventHandler(FileWatcherChanged);
                _watcher.Changed += handler;
                _watcher.EnableRaisingEvents = true;
            });
        }

        /// <summary>
        /// UserControl_Unloaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            StopWatching = true;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    FileSystemEventHandler handler = new FileSystemEventHandler(FileWatcherChanged);
                    _watcher.Changed -= handler;
                    _watcher.Dispose(); // this blockes the app WA 23.12.2017
                    _watcher = null;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            });
        }

        /// <summary>
        /// FileWatcherChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileWatcherChanged(object sender, FileSystemEventArgs e)
        {
            WatchLogFile(LongFileName);
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions
        
        /// <summary>
        /// filePath
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private long GetTotalLinesInFile(string filePath, ref long fileSize)
        {
            try
            {
                using (StreamReader r = new StreamReader(filePath))
                {
                    long counts = 0;
                    fileSize = new FileInfo(filePath).Length;
                    while (r.ReadLine() != null) { counts++; }
                    return counts;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// WatchLogFile
        /// this coop tail part is from:
        /// </summary>
        private void WatchLogFile(string fileName)
        {
            int count = 0;
            long newLength = 0;
            string newFileLines = "";
            bool success = false;
            while (count < 5 && !success && !StopWatching)
            {
                try
                {
                    using (FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        newLength = stream.Length;
                        if (newLength >= _lastFileSize)
                            stream.Position = _lastFileSize;
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            newFileLines = reader.ReadToEnd();

                            string[] stringSeparators = new string[] { "\r\n" };
                            string[] lines = newFileLines.Split(stringSeparators, StringSplitOptions.None);
                            foreach (string s in lines)
                                if (!String.IsNullOrEmpty(s))
                                {
                                    if(s.CaseInsensitiveContains("Error"))
                                        ListBoxLogMessageAdd(s, System.Windows.Media.Brushes.Red);
                                    else
                                        ListBoxLogMessageAdd(s, System.Windows.Media.Brushes.LightSkyBlue);
                                }
                            _lastFileSize = newLength;
                        }
                    }
                    success = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    Thread.Sleep(50);
                }
                ++count;
            }
        }

        /// <summary>
        /// ListBoxLogMessageAdd
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        public void ListBoxLogMessageAdd(string message, System.Windows.Media.Brush color)
        {
            if (DispatcherObject.Thread != Thread.CurrentThread)
                DispatcherObject.Invoke(new DMessageAdd(ListBoxLogMessageAdd), DispatcherPriority.ApplicationIdle, message, color);
            else
            {
                MessageList.Add(new ListBoxLogMessageString(String.Format("{0} {1}", ++_counter, message), color, FontWeights.Normal, 14));
                listBox_LogMessages.SelectedIndex = listBox_LogMessages.Items.Count - 1;
                listBox_LogMessages.ScrollIntoView(listBox_LogMessages.SelectedItem);
                if (MessageList.Count > MAX_MESSGAES)
                    MessageList.RemoveAt(0);
            }
        }

        /// <summary>
        /// Clear
        /// </summary>
        public void Clear()
        {
            MessageList.Clear();
        }

        /// <summary>
        /// OnPropertyChanged
        /// </summary>
        /// <param name="p"></param>
        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
        #endregion
    }

    #region Help Classes
    public static class ExtensionMethods
    {
        /// <summary>
        /// TextReader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="lineCount"></param>
        /// <returns></returns>
        public static string[] Tail(this TextReader reader, int lineCount)
        {
            var buffer = new List<string>(lineCount);
            string line;
            for (int i = 0; i < lineCount; i++)
            {
                line = reader.ReadLine();
                if (line == null) return buffer.ToArray();
                buffer.Add(line);
            }

            int lastLine = lineCount - 1;           //The index of the last line read from the buffer.  Everything > this index was read earlier than everything <= this indes

            while (null != (line = reader.ReadLine()))
            {
                lastLine++;
                if (lastLine == lineCount) lastLine = 0;
                buffer[lastLine] = line;
            }

            if (lastLine == lineCount - 1) return buffer.ToArray();
            var retVal = new string[lineCount];
            buffer.CopyTo(lastLine + 1, retVal, 0, lineCount - lastLine - 1);
            buffer.CopyTo(0, retVal, lineCount - lastLine - 1, lastLine + 1);
            return retVal;
        }

        /// <summary>
        /// CaseInsensitiveContains
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        public static bool CaseInsensitiveContains(this string text, string value,StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return text.IndexOf(value, stringComparison) >= 0;
        }
    }

    public class ListBoxLogMessageString
    {
        private int DEFAULD_FONT_SIZE = 12;
        public string Message { get; set; }
        public System.Windows.Media.Brush MessageColor { get; set; }
        public FontWeight FontWeight { get; set; }
        public int FontSize { get; set; }
        public ListBoxLogMessageString(string message, System.Windows.Media.Brush colore)
        {
            Message = message;
            MessageColor = colore;
            FontWeight = FontWeights.Normal;
            FontSize = DEFAULD_FONT_SIZE;
        }
        public ListBoxLogMessageString(string message, System.Windows.Media.Brush colore, FontWeight fontWeight)
        {
            Message = message;
            MessageColor = colore;
            FontWeight = fontWeight;
            FontSize = DEFAULD_FONT_SIZE;
        }
        public ListBoxLogMessageString(string message, System.Windows.Media.Brush colore, FontWeight fontWeight, int fontSize)
        {
            Message = message;
            MessageColor = colore;
            FontWeight = fontWeight;
            FontSize = fontSize;
        }
    }
    #endregion
}
