using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WPFLogFilter.AsmblyWrapper;
using WPFLogFilter.DialogWrapperFolder;
using WPFLogFilter.Filter;
using WPFLogFilter.Model;
using WPFLogFilter.Parsing.ParseStrategy;
using WPFLogFilter.Parsing.ParsingFactory;
using WPFLogFilter.Tabs;

namespace WPFLogFilter.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<LogModel> _listLoadLine;

        private IDialogWrapper _dialogWrapper;
        private IAssemblyWrapper _assemblyWrapper;
        private IParsingFactory _parsingFactory;
        private IParsingStrategy _parsingStrategy;
        private IFilterFactory _filterFactory;
        private ILog _iLog;
        private List<ITab> _closedTabsList;

        private string _titleVersion;
        private int _tabSelectIndex;
        private bool _tabVisibility = false;
        private bool _showOpenClosedTab = false;
        private double _windowHeight = 700;

        /// <summary>
        /// Constructor for the MainViewModel
        /// </summary>
        /// <param name="iDialogWrapper">Interface for the log-file-open-selection-dialog</param>
        /// <param name="iAssemblyWrapper">Interface for the get-application-version class</param>
        /// <param name="iParsingFactory">Interface for determining which parsing strategy we need to use</param>
        /// <param name="iFilterFactory">Interface for the filters we need to use in the TabViewModel</param>
        /// <param name="iLog">Interface for the LogParse's log file</param>
        public MainViewModel(IDialogWrapper iDialogWrapper, IAssemblyWrapper iAssemblyWrapper, IParsingFactory iParsingFactory, IFilterFactory iFilterFactory, ILog iLog)
        {
            _dialogWrapper = iDialogWrapper ?? throw new ArgumentNullException(nameof(iDialogWrapper));
            _assemblyWrapper = iAssemblyWrapper ?? throw new ArgumentNullException(nameof(iAssemblyWrapper));
            _parsingFactory = iParsingFactory ?? throw new ArgumentNullException(nameof(iParsingFactory));
            _filterFactory = iFilterFactory ?? throw new ArgumentNullException(nameof(iFilterFactory));
            _iLog = iLog ?? throw new ArgumentNullException(nameof(iLog));
            _listLoadLine = new ObservableCollection<LogModel>();
            _closedTabsList = new List<ITab>();

            Tabs = new ObservableCollection<ITab>();

            ClickMenuCommand = new RelayCommand(SelectLogFile);
            OpenClosedTabMenuCommand = new RelayCommand(ReOpenLastClosedTab);
            CloseTabCommand = new RelayCommand<ITab>(CloseTab);
            ClickOpenNotepadCommand = new RelayCommand(OpenNotepad);
            ExitCommand = new RelayCommand(ExitApplication);
            DropInFileCommand = new RelayCommand<DragEventArgs>(OpenDroppedFiles);
            ChangeSizeWindowCommand = new RelayCommand<EventArgs>(ChangeSizeWindow);
            CloseWindowCommand = new RelayCommand(CloseWindow);
            LastClosedTabOpenEventCommand = new RelayCommand(ReOpenLastClosedTabEvent);
            TabMouseClickCommand = new RelayCommand<MouseEventArgs>(CloseTabMiddleButton);

            GetVersion();
        }

        /// <summary>
        /// Command used to open up a dialog menu where we select the files.
        /// </summary>
        public RelayCommand ClickMenuCommand { get; set; }

        /// <summary>
        /// Command which is used to open up the last closed tab.
        /// </summary>
        public RelayCommand OpenClosedTabMenuCommand { get; set; }

        /// <summary>
        /// Command used to open up a dialog menu where we select the files which are opened in Notepad.
        /// </summary>
        public RelayCommand ClickOpenNotepadCommand { get; set; }
        /// <summary>
        /// Command which is used to remove the current TabViewModel from Tabs list, therefore closing it.
        /// </summary>
        public RelayCommand<ITab> CloseTabCommand { get; set; }
        /// <summary>
        /// Command which is used to Exit the LogParser application.
        /// </summary>
        public RelayCommand ExitCommand { get; set; }
        /// <summary>
        /// Command which is used to catch the event that closes the application.
        /// </summary>
        public RelayCommand CloseWindowCommand { get; set; }
        /// <summary>
        /// Command which is used to catch the fileDrop events so they can be opened and loaded.
        /// </summary>
        public RelayCommand<DragEventArgs> DropInFileCommand { get; set; }
        /// <summary>
        /// Command which is used to catch the window resize event so the tabViewModels resize too.
        /// </summary>
        public RelayCommand<EventArgs> ChangeSizeWindowCommand { get; set; }

        /// <summary>
        /// Command triggered by an event which is used to open up the last closed tab.
        /// </summary>
        public RelayCommand LastClosedTabOpenEventCommand { get; set; }

        /// <summary>
        /// Command triggered by a middle mouse button click on a tab window.
        /// </summary>
        public RelayCommand<MouseEventArgs> TabMouseClickCommand { get; set; }

        /// <summary>
        /// A list of interfaces for TabViewModels, which gets an additional tab every time we load a log file.
        /// </summary>
        public ObservableCollection<ITab> Tabs { get; set; }

        /// <summary>
        /// Property for Main Window Title version
        /// </summary>
        public string TitleVersion
        {
            get => _titleVersion;
            set
            {
                Set(ref _titleVersion, value);
            }
        }

        /// <summary>
        /// Property which is used to tell MainWindows.xaml's TabControl to select the last tab that was loaded.
        /// </summary>
        public int TabSelectIndex
        {
            get => _tabSelectIndex;
            set
            {
                Set(ref _tabSelectIndex, value);
            }
        }

        /// <summary>
        /// Property which is used to hide the UserControl window until a file is loaded.
        /// </summary>
        public bool TabVisibility
        {
            get => _tabVisibility;
            set
            {
                Set(ref _tabVisibility, value);
            }
        }

        /// <summary>
        /// Property which is used to note the current window height, so it can be sent to the TabViewModel's User Control's datagrid, so it resizes with the height of the main window.
        /// </summary>
        public double WindowHeight
        {
            get => _windowHeight;
            set
            {
                Set(ref _windowHeight, value);
                Messenger.Default.Send(_windowHeight);
            }
        }

        /// <summary>
        /// Property which is used to toggle visibility of the Reopen closed tab menu option.
        /// </summary>
        public bool ShowOpenClosedTab
        {
            get => _showOpenClosedTab;
            set
            {
                Set(ref _showOpenClosedTab, value);
            }
        }

        private void SelectLogFile()
        {
            GenerateTabs(_dialogWrapper.GetPaths());
        }

        private void GenerateTabs(List<string> listOfPaths)
        {
            if (listOfPaths != null)
            {
                foreach (var path in listOfPaths)
                {
                    Tabs.Add(new TabViewModel(_parsingFactory, _parsingStrategy, _filterFactory, _iLog, path)
                    {
                        ScrollViewHeight = WindowHeight
                    });

                    GetTabIndex();
                }
            }
        }

        private void GetVersion()
        {
            TitleVersion = "LogParser v" + _assemblyWrapper.GetVersion();
        }

        private void GetTabIndex()
        {
            TabSelectIndex = Tabs.Count() - 1;
            TabVisibility = true;
        }

        private void OpenNotepad()
        {
            List<string> temp = _dialogWrapper.GetPaths();
            if (temp != null)
            {
                foreach (var path in temp)
                {
                    Process.Start("notepad.exe", path);
                }
            }
        }

        private void CloseTab(ITab selectedTab)
        {
            if (selectedTab != null)
            {
                _closedTabsList.Add(selectedTab);
                Tabs.Remove(selectedTab);
                _iLog.Info("File closed: " + selectedTab.TabFileName);
                if (Tabs.Count == 0)
                {
                    TabVisibility = false;
                }
                ShowOpenClosedTab = true;
            }
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        private void OpenDroppedFiles(DragEventArgs e)
        {
            string[] filePathList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            List<string> tempList = new List<string>();

            foreach (var path in filePathList)
            {
                tempList.Add(path);
            }
            GenerateTabs(tempList);
        }

        private void ChangeSizeWindow(EventArgs e)
        {
            WindowHeight = ((SizeChangedEventArgs)e).NewSize.Height;
        }

        private void CloseWindow()
        {
            _iLog.Info("User has exited the application");
        }

        private void ReOpenLastClosedTabEvent()
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl)) && (Keyboard.IsKeyDown(Key.LeftShift)) && (Keyboard.IsKeyDown(Key.T)))
            {
                ReOpenLastClosedTab();
            }
        }

        private void ReOpenLastClosedTab()
        {
            if (_closedTabsList.Count != 0)
            {
                Tabs.Add(_closedTabsList.Last());
                _closedTabsList.Remove(_closedTabsList.Last());
                if (_closedTabsList.Count == 0)
                {
                    ShowOpenClosedTab = false;
                }
                GetTabIndex();
            }
        }

        private void CloseTabMiddleButton(MouseEventArgs e)
        {
           
        }
    }
}
