using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using WPFLogFilter.DialogWrapperFolder;
using WPFLogFilter.Filter;
using WPFLogFilter.Model;
using WPFLogFilter.ParsingFactoryStrategyFolder.ParsingFactoryFolder;
using WPFLogFilter.ParsingFactoryStrategyFolder.ParsingStrategyFolder;
using WPFLogFilter.Tabs;

namespace WPFLogFilter.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<LogModel> _listLoadLine;

        private IDialogWrapper _dialogWrapper;
        private IParsingFactory _parsingFactory;
        private IParsingStrategy _parsingStrategy;
        private IFilterFactory _filterFactory;

        private string _titleVersion;
        private string _tabFileName;
        private int _tabSelectIndex;
        private bool _tabVisibility = false;

        public MainViewModel(IDialogWrapper dialogWrapper, IParsingFactory iParsingFactory, IFilterFactory iFilterFactory)
        {
            _dialogWrapper = dialogWrapper;
            _parsingFactory = iParsingFactory;
            _filterFactory = iFilterFactory;

            _listLoadLine = new ObservableCollection<LogModel>();
            Tabs = new ObservableCollection<ITab>();

            GetVersion();

            ClickMenuCommand = new RelayCommand(SelectLogFile);
            CloseTabCommand = new RelayCommand<ITab>(CloseTab);
            ClickOpenNotepadCommand = new RelayCommand(OpenNotepad);
            ExitCommand = new RelayCommand(ExitApplication);
            DropInFileCommand = new RelayCommand<DragEventArgs>(OpenDroppedFiles);
        }

        public RelayCommand ClickMenuCommand { get; set; }
        public RelayCommand ClickOpenNotepadCommand { get; set; }
        public RelayCommand<ITab> CloseTabCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand<DragEventArgs> DropInFileCommand { get; set; }

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

        public void SelectLogFile()
        {
            PopulateList(_dialogWrapper.GetLines());
        }

        public void PopulateList(List<FileModel> listFileInfo)
        {
            if (listFileInfo != null)
            {
                foreach (FileModel file in listFileInfo)
                {
                    _parsingStrategy = _parsingFactory.Create(file.FileData);
                    _listLoadLine = new ObservableCollection<LogModel>(_parsingStrategy.Parse(file.FileData));
                    Tabs.Add(new TabViewModel(_listLoadLine, _parsingStrategy, _filterFactory, file.FilePath));
                    GetTabIndex();
                }
            }
        }

        public void GetVersion()
        {
            string version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            TitleVersion = "LogParser v" + version;
        }

        public void GetTabIndex()
        {
            TabSelectIndex = Tabs.Count() - 1;
            TabVisibility = true;
        }

        public void OpenNotepad()
        {
            List<FileModel> temp = _dialogWrapper.GetLines();
            if (temp!=null)
            {
                foreach (FileModel file in temp)
                {
                    Process.Start("notepad.exe", file.FilePath);
                }
            }
        }

        public void CloseTab(ITab selectedTab)
        {
            Tabs.Remove(selectedTab);
            if (Tabs.Count == 0)
            {
                TabVisibility = false;
            }
        }

        public void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        public void OpenDroppedFiles(DragEventArgs e)
        {
            string[] filePathList= (string[])e.Data.GetData(DataFormats.FileDrop, false);

            List<FileModel> tempList = new List<FileModel>();

            foreach (string file in filePathList)
            {
                tempList.Add(new FileModel { FilePath = file, FileData = File.ReadAllLines(file) });
            }

            PopulateList(tempList);
        }
    }
}
