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

namespace WPFLogFilter.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<LogModel> _listLoadLine;
        private ObservableCollection<ObservableCollection<LogModel>> _listFilters;
        private ObservableCollection<LogModel> _backupList;
        private ObservableCollection<LogModel> _idList;
        private ObservableCollection<LogModel> _dateTimeList;
        private ObservableCollection<LogModel> _threadIdList;
        private ObservableCollection<LogModel> _logLevelList;
        private ObservableCollection<LogModel> _eventIdList;
        private ObservableCollection<LogModel> _regexList;

        private IDialogWrapper _dialogWrapper;
        private IParsingFactory _parsingFactory;
        private IFilterFactory _filterfactory;
        private IParsingStrategy _parsingStrategy;
        private IList<LogLevelEnum> _logLvlComboEnumList;
        private LogLevelEnum _logLevelValues;

        private bool _idIsValid = false;
        private bool _dateTimeIsValid = false;
        private bool _threadIdIsValid = false;
        private bool _logLevelIsValid = false;
        private bool _eventIdIsValid = false;
        private bool _textIsValid = false;
        private bool _caseSensitiveCheckBox = true;
        private bool _openFileIsValid = false;

        private string _titleNameAndVersion;
        private string _titleFileName = "";
        
        private string _logTextSearch = "";
        private string _dateTimeSearch1 = "";
        private string _dateTimeSearch2 = "";
        private string _threadIdSearch = "";
        private string _logLevelSearch = "";
        private string _eventIdSearch = "";

        public MainViewModel(IDialogWrapper dialogWrapper, IParsingFactory iParsingFactory, IFilterFactory iFilterFactory)
        {
            _dialogWrapper = dialogWrapper;
            _parsingFactory = iParsingFactory;
            _filterfactory = iFilterFactory;

            ListLoadLine = new ObservableCollection<LogModel>();
            ListFilters = new ObservableCollection<ObservableCollection<LogModel>>();

            _idList = new ObservableCollection<LogModel>();
            _eventIdList = new ObservableCollection<LogModel>();
            _dateTimeList = new ObservableCollection<LogModel>();
            _threadIdList = new ObservableCollection<LogModel>();
            _logLevelList = new ObservableCollection<LogModel>();
            _eventIdList = new ObservableCollection<LogModel>();
            _regexList = new ObservableCollection<LogModel>();
            _backupList = new ObservableCollection<LogModel>();
            _logLvlComboEnumList = Enum.GetValues(typeof(LogLevelEnum)).OfType<LogLevelEnum>().ToList();

            GetFileNameAndVersion();

            ClickMenuCommand = new RelayCommand(SelectLogFile);
            OpenNotepadCommand = new RelayCommand(OpenNotepad);
        }

        public RelayCommand ClickMenuCommand { get; set; }
        public RelayCommand OpenNotepadCommand { get; set; }

        public ObservableCollection<LogModel> ListLoadLine
        {
            get =>_listLoadLine;
            set
            {
                Set(ref _listLoadLine, value);
                if (ListLoadLine != null)
                {
                    if (ListLoadLine.Count != 0)
                    {
                        ToggleColumnVisibility();
                    }
                }
            }
        }

        public bool CaseSensitiveCheckBox
        {
            get => _caseSensitiveCheckBox;
            set
            {
                Set(ref _caseSensitiveCheckBox, value);
                IFilter filterFactory = _filterfactory.Create(6);
                if (!_caseSensitiveCheckBox)
                {
                    ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, LogTextSearch + "�"), 6);
                }
                else
                {
                    ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, LogTextSearch), 6);
                }
            }
        }

        public bool IdIsValid
        {
            get => _idIsValid;
            set
            {
                Set(ref _idIsValid, value);
            }
        }
        public bool DateTimeIsValid
        {
            get => _dateTimeIsValid;
            set
            {
                Set(ref _dateTimeIsValid, value);
            }
        }
        public bool ThreadIdIsValid
        {
            get => _threadIdIsValid;
            set
            {
                Set(ref _threadIdIsValid, value);
            }
        }
        public bool LogLevelIsValid
        {
            get => _logLevelIsValid;
            set
            {
                Set(ref _logLevelIsValid, value);
            }
        }
        public bool EventIdIsValid
        {
            get => _eventIdIsValid;
            set
            {
                Set(ref _eventIdIsValid, value);
            }
        }

        public bool TextIsValid
        {
            get => _textIsValid;
            set
            {
                Set(ref _textIsValid, value);
            }
        }

        public string LogTextSearch
        {
            get => _logTextSearch;
            set
            {
                Set(ref _logTextSearch, value);
                IFilter filterFactory = _filterfactory.Create(6);
                if (!_caseSensitiveCheckBox)
                {
                    ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, LogTextSearch + "�"), 6);
                }
                else
                {
                    ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, LogTextSearch), 6);
                }
            }
        }

        public string DateTimeSearch1
        {
            get => _dateTimeSearch1;
            set
            {
                Set(ref _dateTimeSearch1, value);
                IFilter filterFactory = _filterfactory.Create(2);
                ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, DateTimeSearch1 + "+" + DateTimeSearch2), 2);
            }
        }

        public string DateTimeSearch2
        {
            get => _dateTimeSearch2;
            set
            {
                Set(ref _dateTimeSearch2, value);
                IFilter filterFactory = _filterfactory.Create(2);
                ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, DateTimeSearch1 + "+" + DateTimeSearch2), 2);
            }
        }

        public string ThreadIdSearch
        {
            get => _threadIdSearch;
            set
            {
                Set(ref _threadIdSearch, value);
                IFilter filterFactory = _filterfactory.Create(3);
                ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, ThreadIdSearch), 3);
            }
        }
        public string LogLevelSearch
        {
            get => _logLevelSearch;
            set
            {
                Set(ref _logLevelSearch, value);
                IFilter filterFactory = _filterfactory.Create(4);
                ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, LogLevelSearch), 4);

            }
        }
        public string EventIdSearch
        {
            get => _eventIdSearch;
            set
            {
                Set(ref _eventIdSearch, value);
                IFilter filterFactory = _filterfactory.Create(5);
                ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, EventIdSearch), 5);
            }
        }

        public LogLevelEnum LogLevelValues
        {
            get => _logLevelValues;
            set
            {
                Set(ref _logLevelValues, value);
                IFilter filterFactory = _filterfactory.Create(4);
                ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, LogLevelValues.ToString()), 5);
            }
        }

        public ObservableCollection<ObservableCollection<LogModel>> ListFilters
        {
            get => _listFilters;
            set
            {
                Set(ref _listFilters, value);
            }
        }

        public bool OpenFileIsValid
        {
            get => _openFileIsValid;
            set
            {
                Set(ref _openFileIsValid, value);
            }
        }

        public string TitleNameAndVersion
        {
            get => _titleNameAndVersion;
            set
            {
                Set(ref _titleNameAndVersion, value);
            }
        }

        public IList<LogLevelEnum> LogLvlComboEnumList
        {
            get => _logLvlComboEnumList;
            set
            {
                Set(ref _logLvlComboEnumList, value);
            }
        }

        public void OpenNotepad()
        {
            Process.Start("notepad.exe", _titleFileName);
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
                ClearAllFilters();
                ListLoadLine = new ObservableCollection<LogModel>(_parsingStrategy.Parse(lines));
                GetFileNameAndVersion();
                _backupList = ListLoadLine;
            }
        }

        public void ClearAllFilters()
        {
            _logLevelValues = LogLevelEnum.ALL;

            _logTextSearch = "";
            _dateTimeSearch1 = "";
            _dateTimeSearch2 = "";
            _threadIdSearch = "";
            _logLevelSearch = "";
            _eventIdSearch = "";
            ListFilters.Clear();
            _idList.Clear();
            _eventIdList.Clear();
            _dateTimeList.Clear();
            _threadIdList.Clear();
            _logLevelList.Clear();
            _eventIdList.Clear();
            _regexList.Clear();
        }

        public void GetFileNameAndVersion()
        {
            string version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();

            TitleNameAndVersion = "LogParser v" + version;
            if (!string.IsNullOrEmpty(_titleFileName))
            {
                string[] results = _titleFileName.Split(new char[] { '\\', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                TitleNameAndVersion += " - " + results[results.Length - 1];
            }
        }

        public void ToggleColumnVisibility()
        {
            IdIsValid = true;
            LogLevelIsValid = true;
            TextIsValid = true;
            ThreadIdIsValid = true;
            EventIdIsValid = true;
            DateTimeIsValid = true;
            OpenFileIsValid = true;
            if (_parsingStrategy is NoThreadIdParsingStrategy)
            {
                ThreadIdIsValid = false;
            }

            if (_parsingStrategy is NoEventIdParsingStrategy)
            {
                EventIdIsValid = false;
            }

            if (_parsingStrategy is StringOnlyParsingStrategy)
            {
                LogLevelIsValid = false;
                ThreadIdIsValid = false;
                EventIdIsValid = false;
                DateTimeIsValid = false;
            }
        }

        public ObservableCollection<LogModel> AddRemoveFilter(ObservableCollection<LogModel> list, int caseNo)
        {
            switch (caseNo)
            {
                case 2:
                    if (_dateTimeList.Count != 0)
                    {
                        ListFilters.Remove(_dateTimeList);
                    }
                    _dateTimeList = list;
                    break;
                case 3:
                    if (_threadIdList.Count != 0)
                    {
                        ListFilters.Remove(_threadIdList);
                    }
                    _threadIdList = list;
                    break;
                case 4:
                    if (_logLevelList.Count != 0)
                    {
                        ListFilters.Remove(_logLevelList);
                    }
                    _logLevelList = list;
                    break;
                case 5:
                    if (_eventIdList.Count != 0)
                    {
                        ListFilters.Remove(_eventIdList);
                    }
                    _eventIdList = list;
                    break;
                case 6:
                    if (_regexList.Count != 0)
                    {
                        ListFilters.Remove(_regexList);
                    }
                    _regexList = list;
                    break;
                default:
                    break;
            }

            if (list != null)
            {
                if (list.Count != 0)
                {
                    ListFilters.Add(list);
                }
                IEnumerable<LogModel> filterResult = list;
                if (ListFilters.Count() > 1)
                {
                    for (int x = 0; x < ListFilters.Count - 1; x++)
                    {
                        if (x == 0)
                        {
                            filterResult = ListFilters[x].Intersect(ListFilters[x + 1]).ToList();
                            continue;
                        }
                        filterResult = filterResult.Intersect(ListFilters[x + 1]).ToList();
                    }
                }
                return new ObservableCollection<LogModel>(filterResult);
            }
            else
            {
                return null;
            }
        }
    }
}
