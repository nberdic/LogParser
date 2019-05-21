using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using WPFLogFilter.AsmblyWrapper;
using WPFLogFilter.DialogWrapperFolder;
using WPFLogFilter.Filter;
using WPFLogFilter.Model;
using WPFLogFilter.Observables;
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

        private string _titleVersion;
        private string _tabFileName;
        private int _tabSelectIndex;
        private bool _tabVisibility = false;
        private double _windowHeight = 700;

        public MainViewModel(IDialogWrapper iDialogWrapper, IAssemblyWrapper iAssemblyWrapper, IParsingFactory iParsingFactory, IFilterFactory iFilterFactory, ILog iLog)
        {
            _dialogWrapper = iDialogWrapper ?? throw new ArgumentNullException(nameof(iDialogWrapper));
            _assemblyWrapper = iAssemblyWrapper ?? throw new ArgumentNullException(nameof(iAssemblyWrapper));
            _parsingFactory = iParsingFactory ?? throw new ArgumentNullException(nameof(iParsingFactory));
            _filterFactory = iFilterFactory ?? throw new ArgumentNullException(nameof(iFilterFactory));
            _iLog = iLog ?? throw new ArgumentNullException(nameof(iLog));

            _listLoadLine = new ObservableCollection<LogModel>();
            Tabs = new ObservableCollection<ITab>();

            GetVersion();

            ClickMenuCommand = new RelayCommand(SelectLogFile);
            CloseTabCommand = new RelayCommand<ITab>(CloseTab);
            ClickOpenNotepadCommand = new RelayCommand(OpenNotepad);
            ExitCommand = new RelayCommand(ExitApplication);
            DropInFileCommand = new RelayCommand<DragEventArgs>(OpenDroppedFiles);
            ChangeSizeWindowCommand = new RelayCommand<EventArgs>(ChangeSizeWindow);
            CloseWindowCommand = new RelayCommand(CloseWindow);
        }

        public RelayCommand ClickMenuCommand { get; set; }
        public RelayCommand ClickOpenNotepadCommand { get; set; }
        public RelayCommand<ITab> CloseTabCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }

        public RelayCommand CloseWindowCommand { get; set; }

        public RelayCommand<DragEventArgs> DropInFileCommand { get; set; }
        public RelayCommand<EventArgs> ChangeSizeWindowCommand { get; set; }

        public ObservableCollection<ITab> Tabs { get; set; }

        public string TitleVersion
        {
            get => _titleVersion;
            set
            {
                Set(ref _titleVersion, value);
            }
        }

        public string TabFileName
        {
            get => _tabFileName;
            set
            {
                Set(ref _tabFileName, value);
            }
        }

        public int TabSelectIndex
        {
            get => _tabSelectIndex;
            set
            {
                Set(ref _tabSelectIndex, value);
            }
        }

        public bool TabVisibility
        {
            get => _tabVisibility;
            set
            {
                Set(ref _tabVisibility, value);
            }
        }

        public double WindowHeight
        {
            get => _windowHeight;
            set
            {
                Set(ref _windowHeight, value);
                Messenger.Default.Send(_windowHeight);
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
                Tabs.Remove(selectedTab);
                _iLog.Info("File closed: " + ((TabViewModel)selectedTab).TabFileName);
                if (Tabs.Count == 0)
                {
                    TabVisibility = false;
                }
            }
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
            _iLog.Info("User has exited the application");
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
    }
}
