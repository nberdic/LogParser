using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFLogFilter.DialogWrapperFolder;
using WPFLogFilter.Enums;
using WPFLogFilter.Filter;
using WPFLogFilter.Model;
using WPFLogFilter.Parsing.ParseStrategy;
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

        private string _titleFileName;
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
        }

        public RelayCommand ClickMenuCommand { get; set; }

        public RelayCommand<ITab> CloseTabCommand { get; set; }

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

        public bool TabVisibility { get => _tabVisibility;
            set
            {
                Set(ref _tabVisibility, value);
            }
        }

       

        public void SelectLogFile()
        {
            PopulateList(_dialogWrapper.GetLines(ref _titleFileName));
        }

        public void PopulateList(string[] lines)
        {
            if (lines != null)
            {
                _parsingStrategy = _parsingFactory.Create(lines);
                _listLoadLine = new ObservableCollection<LogModel>(_parsingStrategy.Parse(lines));

                Tabs.Add(new TabViewModel(_listLoadLine, _parsingStrategy, _filterFactory, _titleFileName));
                GetTabIndex();
            }
        }

        public void GetVersion()
        {
            string version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            TitleVersion = "LogParser v" + version;
        }

        public void GetTabIndex()
        {
            TabSelectIndex = Tabs.Count()-1;
            TabVisibility = true;
        }

        public void CloseTab(ITab selectedTab)
        {
            Tabs.Remove(selectedTab);
            if (Tabs.Count==0)
            {
                TabVisibility = false;
            }
        }

    }
}
