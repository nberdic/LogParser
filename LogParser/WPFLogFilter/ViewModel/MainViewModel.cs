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

        public MainViewModel(IDialogWrapper dialogWrapper, IAssemblyWrapper assemblyWrapper, IParsingFactory iParsingFactory, IFilterFactory iFilterFactory, ILog iLog)
        {
            _dialogWrapper = dialogWrapper ?? throw new ArgumentNullException(nameof(dialogWrapper));
            _assemblyWrapper = assemblyWrapper ?? throw new ArgumentNullException(nameof(assemblyWrapper));
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
            PopulateList(_dialogWrapper.GetLines());
        }

        private void PopulateList(List<FileModel> listFileInfo)
        {
            if (listFileInfo != null)
            {
                foreach (FileModel file in listFileInfo)
                {
                    _parsingStrategy = _parsingFactory.Create(file.FileData);
                    var parsingCollection = _parsingStrategy.Parse(file.FileData);
                    if (parsingCollection == null)
                    {
                        return;
                    }
                    _listLoadLine = new ObservableCollection<LogModel>(_parsingStrategy.Parse(file.FileData));
                    Tabs.Add(new TabViewModel(_listLoadLine, _parsingStrategy, _filterFactory, _iLog, file.FilePath)
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
            List<FileModel> temp = _dialogWrapper.GetLines();
            if (temp != null)
            {
                foreach (FileModel file in temp)
                {
                    Process.Start("notepad.exe", file.FilePath);
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

            List<FileModel> tempList = new List<FileModel>();

            foreach (string file in filePathList)
            {
                File.SetAttributes(file, FileAttributes.Normal);

                using (FileStream logFileStream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader logFileReader = new StreamReader(logFileStream))
                    {
                        List<string> listOfStrings = new List<string>();

                        while (!logFileReader.EndOfStream)
                        {
                            listOfStrings.Add(logFileReader.ReadLine());
                        }

                        tempList.Add(new FileModel { FilePath = file, FileData = listOfStrings.ToArray() });
                    }
                }
            }
            PopulateList(tempList);
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
