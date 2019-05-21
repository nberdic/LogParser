using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using WPFLogFilter.Enums;
using WPFLogFilter.Filter;
using WPFLogFilter.Model;
using WPFLogFilter.Observables;
using WPFLogFilter.Parsing.ParseStrategy;
using WPFLogFilter.Parsing.ParsingFactory;
using WPFLogFilter.Tabs;

namespace WPFLogFilter.ViewModel
{
    public class TabViewModel : ViewModelBase, ITab
    {
        private ObservableCollection<LogModel> _listLoadLine;
        private ObservableCollection<LogModel> _backupList;
        private ObservableCollection<ObservableCollection<LogModel>> _listFilters;
        private ObservableCollection<LogModel> _dateTimeList;
        private ObservableCollection<LogModel> _threadIdList;
        private ObservableCollection<LogModel> _logLevelList;
        private ObservableCollection<LogModel> _eventIdList;
        private ObservableCollection<LogModel> _regexList;
        private ObservableCollection<LogModel> _textList;

        private FileWatcher _fileWatcher;

        private IParsingFactory _parsingFactory;
        private IFilterFactory _filterfactory;
        private IParsingStrategy _parsingStrategy;
        private ILog _iLog;

        private IList<LogLevelEnum> _logLvlComboEnumList;
        private LogLevelEnum _logLevelValues;

        private bool _idIsValid = false;
        private bool _dateTimeIsValid = false;
        private bool _threadIdIsValid = false;
        private bool _logLevelIsValid = false;
        private bool _eventIdIsValid = false;
        private bool _textIsValid = false;
        private bool _noDateCheckBoxIsValid = false;
        private bool _caseSensitiveCheckBox = true;
        private bool _noDateCheckBox = true;
        private bool _regexSearchCheckBox = false;
        private bool _openFileIsValid = false;

        private string _logFilePath;
        private string _tabFileName;

        private string _dateTimeSearch1 = "00:00:00";
        private string _dateTimeSearch2 = "23:59:59";
        private string _threadIdSearch = "";
        private string _eventIdSearch = "";
        private string _logTextSearch = "";
        private double _scrollViewHeight = 700;

        public TabViewModel(IParsingFactory iParsingFactory, IParsingStrategy iStrategy, IFilterFactory iFilterFactory, ILog iLog, string logFilePath)
        {
            _fileWatcher = new FileWatcher();
            _parsingFactory = iParsingFactory;
            _filterfactory = iFilterFactory;
            _parsingStrategy = iStrategy;
            _iLog = iLog;
            _logFilePath = logFilePath;

            _eventIdList = new ObservableCollection<LogModel>();
            _dateTimeList = new ObservableCollection<LogModel>();
            _threadIdList = new ObservableCollection<LogModel>();
            _logLevelList = new ObservableCollection<LogModel>();
            _eventIdList = new ObservableCollection<LogModel>();
            _regexList = new ObservableCollection<LogModel>();
            _textList = new ObservableCollection<LogModel>();
            ListFilters = new ObservableCollection<ObservableCollection<LogModel>>();
            _logLvlComboEnumList = Enum.GetValues(typeof(LogLevelEnum)).OfType<LogLevelEnum>().ToList();

            PopulateList(GetLines(logFilePath));

            Messenger.Default.Register<double>(this, UpdateScrollViewSize);

            ExtractFileName();

            GetLogInfo();

            _fileWatcher.OnFileModified = (s) => FileChangeEvent(s);
            _fileWatcher.Watch(logFilePath);
            
            NoDateCheckBoxIsValid = _listLoadLine.Any(x => x.DateTime == DateTime.MinValue);
        }



        public ObservableCollection<LogModel> ListLoadLine
        {
            get => _listLoadLine;
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
                OnChangeCreateFilter();
            }
        }

        public bool NoDateCheckBox
        {
            get => _noDateCheckBox;
            set
            {
                Set(ref _noDateCheckBox, value);
                OnChangeCreateFilter();
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

        public string DateTimeSearch1
        {
            get => _dateTimeSearch1;
            set
            {
                Set(ref _dateTimeSearch1, value);
                OnChangeCreateFilter();
            }
        }

        public string DateTimeSearch2
        {
            get => _dateTimeSearch2;
            set
            {
                Set(ref _dateTimeSearch2, value);
                OnChangeCreateFilter();
            }
        }

        public string ThreadIdSearch
        {
            get => _threadIdSearch;
            set
            {
                Set(ref _threadIdSearch, value);
                OnChangeCreateFilter();
            }
        }

        public LogLevelEnum LogLevelValues
        {
            get => _logLevelValues;
            set
            {
                Set(ref _logLevelValues, value);
                OnChangeCreateFilter();
            }
        }

        public string EventIdSearch
        {
            get => _eventIdSearch;
            set
            {
                Set(ref _eventIdSearch, value);
                OnChangeCreateFilter();
            }
        }

        public string LogTextSearch
        {
            get => _logTextSearch;
            set
            {
                Set(ref _logTextSearch, value);
                OnChangeCreateFilter();
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

        public IList<LogLevelEnum> LogLvlComboEnumList
        {
            get => _logLvlComboEnumList;
            set
            {
                Set(ref _logLvlComboEnumList, value);
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

        public double ScrollViewHeight
        {
            get
            {
                return _scrollViewHeight - 165;
            }
            set
            {
                Set(ref _scrollViewHeight, value);
            }
        }

        public bool NoDateCheckBoxIsValid
        {
            get => _noDateCheckBoxIsValid;
            set
            {
                Set(ref _noDateCheckBoxIsValid, value);
            }
        }

        public bool RegexSearchCheckBox
        {
            get => _regexSearchCheckBox;
            set
            {
                Set(ref _regexSearchCheckBox, value);
                OnChangeCreateFilter();
            }
        }

        private void ToggleColumnVisibility()
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

        private void OnChangeCreateFilter()
        {
            IFilter filterFactory;

            filterFactory = _filterfactory.Create(2);
            if (_noDateCheckBox)
            {
                ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, DateTimeSearch1 + "¥" + DateTimeSearch2 + "¢"), 2);
            }
            else
            {
                ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, DateTimeSearch1 + "¥" + DateTimeSearch2), 2);
            }

            filterFactory = _filterfactory.Create(3);
            ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, ThreadIdSearch), 3);

            filterFactory = _filterfactory.Create(4);
            ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, LogLevelValues.ToString()), 4);

            filterFactory = _filterfactory.Create(5);
            ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, EventIdSearch), 5);

            if (!_regexSearchCheckBox)
            {
                filterFactory = _filterfactory.Create(7);
                if (!_caseSensitiveCheckBox)
                {
                    ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, LogTextSearch + "¢"), 7);
                }
                else
                {
                    ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, LogTextSearch), 7);
                }
            }
            else
            {
                filterFactory = _filterfactory.Create(6);
                ListLoadLine = AddRemoveFilter(filterFactory.Filter(_backupList, LogTextSearch), 6);
            }
        }

        private ObservableCollection<LogModel> AddRemoveFilter(ObservableCollection<LogModel> list, int caseNo)
        {
            switch (caseNo)
            {
                case 2:
                    ListFilters.Remove(_dateTimeList);
                    _dateTimeList = list;
                    break;
                case 3:
                    ListFilters.Remove(_threadIdList);
                    _threadIdList = list;
                    break;
                case 4:
                    ListFilters.Remove(_logLevelList);
                    _logLevelList = list;
                    break;
                case 5:
                    ListFilters.Remove(_eventIdList);
                    _eventIdList = list;
                    break;
                case 6:
                    ListFilters.Remove(_regexList);
                    _regexList = list;
                    ListFilters.Remove(_textList);
                    _textList = list;
                    break;
                case 7:
                    ListFilters.Remove(_textList);
                    _textList = list;
                    ListFilters.Remove(_regexList);
                    _regexList = list;
                    break;
            }

            if (list != null)
            {
                ListFilters.Add(list);

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

        private void ExtractFileName()
        {
            if (!String.IsNullOrEmpty(_logFilePath))
            {
                string[] results = _logFilePath.Split(new char[] { '\\', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                TabFileName = results[results.Length - 1];
            }
        }

        private void UpdateScrollViewSize(double windowSize)
        {
            ScrollViewHeight = windowSize;
        }

        private void GetLogInfo()
        {
            _iLog.Info("Loaded file: " + TabFileName);
            _iLog.Info(TabFileName + " has " + ListLoadLine.Count + " lines");
        }

        private void PopulateList(string[] logFileData)
        {
            _parsingStrategy = _parsingFactory.Create(logFileData);
            var parsingCollection = _parsingStrategy.Parse(logFileData);
            if (parsingCollection == null)
            {
                return;
            }
            ListLoadLine = new ObservableCollection<LogModel>(_parsingStrategy.Parse(logFileData));
            _backupList = _listLoadLine;
        }

        private string[] GetLines(string logFilePath)
        {
            using (FileStream logFileStream = File.Open(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader logFileReader = new StreamReader(logFileStream))
                {
                    List<string> listOfStrings = new List<string>();

                    while (!logFileReader.EndOfStream)
                    {
                        listOfStrings.Add(logFileReader.ReadLine());
                    }
                    return listOfStrings.ToArray();
                }
            }
        }

        private void FileChangeEvent(string logFilePath)
        {
            _listLoadLine = new ObservableCollection<LogModel>(_parsingStrategy.Parse(GetLines(logFilePath)));
            _backupList = _listLoadLine;
            OnChangeCreateFilter();
        }
    }
}
