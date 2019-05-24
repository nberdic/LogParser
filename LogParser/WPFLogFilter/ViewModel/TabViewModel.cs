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
using WPFLogFilter.FileWatchers;
using WPFLogFilter.Parsing.ParseStrategy;
using WPFLogFilter.Parsing.ParsingFactory;
using WPFLogFilter.Tabs;

namespace WPFLogFilter.ViewModel
{
    public class TabViewModel : ViewModelBase, ITab
    {
        private ObservableCollection<IModel> _listLoadLine;
        private ObservableCollection<IModel> _backupList;
        private ObservableCollection<ObservableCollection<IModel>> _listFilters;

        private FileWatcher _fileWatcher;

        private IParsingFactory _parsingFactory;
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

        private string _logFilePath;
        private string _tabFileName;

        private int _filterCount;

        private string _dateTimeSearch1 = "00:00:00";
        private string _dateTimeSearch2 = "23:59:59";
        private string _threadIdSearch = "";
        private string _eventIdSearch = "";
        private string _logTextSearch = "";
        private double _scrollViewHeight = 700;

        /// <summary>
        /// Constructor for the TabViewModel
        /// </summary>
        /// <param name="iParsingFactory">Interface for determining which parsing strategy we need to use</param>
        /// <param name="iParsingStrategy">Interface for the determining how to parse a log file</param>
        /// <param name="iFilterFactory">Interface for the filters we need to use</param>
        /// <param name="iLog">Interface for the LogParse's log file</param>
        /// <param name="logFilePath">A string containing the path to the log file</param>
        public TabViewModel(IParsingFactory iParsingFactory, IParsingStrategy iParsingStrategy, ILog iLog, string logFilePath)
        {
            _fileWatcher = new FileWatcher();
            _parsingFactory = iParsingFactory;
            _parsingStrategy = iParsingStrategy;
            _iLog = iLog;
            _logFilePath = logFilePath;

            ListFilters = new ObservableCollection<ObservableCollection<IModel>>();
            _logLvlComboEnumList = Enum.GetValues(typeof(LogLevelEnum)).OfType<LogLevelEnum>().ToList();

            PopulateList(GetLines(logFilePath));

            Messenger.Default.Register<double>(this, UpdateScrollViewSize);

            ExtractFileName();
            GetLogInfo();

            _fileWatcher.OnFileModified = (s) => FileChangeEvent(s);
            _fileWatcher.Watch(logFilePath);

            NoDateCheckBoxIsValid = _listLoadLine.OfType<IStandardModel>().Any(x => x.DateTime == DateTime.MinValue);
        }

        /// <summary>
        /// Property used as a Main list that is displayed in the DataGrid.
        /// </summary>
        public ObservableCollection<IModel> ListLoadLine
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

        /// <summary>
        /// Property used to toggle the Case Sensitivity of the text search filter.
        /// </summary>
        public bool CaseSensitiveCheckBox
        {
            get => _caseSensitiveCheckBox;
            set
            {
                Set(ref _caseSensitiveCheckBox, value);
                OnChangeCreateFilter();
            }
        }

        /// <summary>
        /// Property used to toggle between the Regex and ordinary text search.
        /// </summary>
        public bool RegexSearchCheckBox
        {
            get => _regexSearchCheckBox;
            set
            {
                Set(ref _regexSearchCheckBox, value);
                OnChangeCreateFilter();
            }
        }

        /// <summary>
        /// Property used to toggle the ,,show dates with minimum value" in the DataGrid.
        /// </summary>
        public bool NoDateCheckBox
        {
            get => _noDateCheckBox;
            set
            {
                Set(ref _noDateCheckBox, value);
                OnChangeCreateFilter();
            }
        }

        /// <summary>
        /// Property used to hide/show the Id column in the DataGrid.
        /// </summary>
        public bool IdIsValid
        {
            get => _idIsValid;
            set
            {
                Set(ref _idIsValid, value);
            }
        }

        /// <summary>
        /// Property used to hide/show the Date/Time column in the DataGrid.
        /// </summary>
        public bool DateTimeIsValid
        {
            get => _dateTimeIsValid;
            set
            {
                Set(ref _dateTimeIsValid, value);
            }
        }

        /// <summary>
        /// Property used to hide/show the ThreadId column in the DataGrid.
        /// </summary>
        public bool ThreadIdIsValid
        {
            get => _threadIdIsValid;
            set
            {
                Set(ref _threadIdIsValid, value);
            }
        }

        /// <summary>
        /// Property used to hide/show the LogLevel column in the DataGrid.
        /// </summary>
        public bool LogLevelIsValid
        {
            get => _logLevelIsValid;
            set
            {
                Set(ref _logLevelIsValid, value);
            }
        }

        /// <summary>
        /// Property used to hide/show the EventId column in the DataGrid.
        /// </summary>
        public bool EventIdIsValid
        {
            get => _eventIdIsValid;
            set
            {
                Set(ref _eventIdIsValid, value);
            }
        }

        /// <summary>
        /// Property used to hide/show the Text column in the DataGrid.
        /// </summary>
        public bool TextIsValid
        {
            get => _textIsValid;
            set
            {
                Set(ref _textIsValid, value);
            }
        }

        /// <summary>
        /// Property used to hide/show the checkbox for ,,dates with minimum value".
        /// </summary>
        public bool NoDateCheckBoxIsValid
        {
            get => _noDateCheckBoxIsValid;
            set
            {
                Set(ref _noDateCheckBoxIsValid, value);
            }
        }

        /// <summary>
        /// Property used to filter the Date/Time, this is a starting parameter (From this date).
        /// </summary>
        public string DateTimeSearch1
        {
            get => _dateTimeSearch1;
            set
            {
                Set(ref _dateTimeSearch1, value);
                OnChangeCreateFilter();
            }
        }

        /// <summary>
        /// Property used to filter the Date/Time, this is a ending parameter (To this date).
        /// </summary>
        public string DateTimeSearch2
        {
            get => _dateTimeSearch2;
            set
            {
                Set(ref _dateTimeSearch2, value);
                OnChangeCreateFilter();
            }
        }

        /// <summary>
        /// Property used to filter the ThreadId.
        /// </summary>
        public string ThreadIdSearch
        {
            get => _threadIdSearch;
            set
            {
                Set(ref _threadIdSearch, value);
                OnChangeCreateFilter();
            }
        }

        /// <summary>
        /// Property that is used by the list that is used by the combobox, and that houses all the possible values of the LogLevel.
        /// </summary>
        public LogLevelEnum LogLevelValues
        {
            get => _logLevelValues;
            set
            {
                Set(ref _logLevelValues, value);
                OnChangeCreateFilter();
            }
        }

        /// <summary>
        /// Property that is used to filter the EventId.
        /// </summary>
        public string EventIdSearch
        {
            get => _eventIdSearch;
            set
            {
                Set(ref _eventIdSearch, value);
                OnChangeCreateFilter();
            }
        }

        /// <summary>
        /// Property that is used to filter the Text.
        /// </summary>
        public string LogTextSearch
        {
            get => _logTextSearch;
            set
            {
                Set(ref _logTextSearch, value);
                OnChangeCreateFilter();
            }
        }

        /// <summary>
        /// Property that is used to store all the filter lists that the log lines go through.
        /// </summary>
        public ObservableCollection<ObservableCollection<IModel>> ListFilters
        {
            get => _listFilters;
            set
            {
                Set(ref _listFilters, value);
            }
        }

        /// <summary>
        /// Property of Enum values that is used by the LogLevel combobox.
        /// </summary>
        public IList<LogLevelEnum> LogLvlComboEnumList
        {
            get => _logLvlComboEnumList;
            set
            {
                Set(ref _logLvlComboEnumList, value);
            }
        }

        /// <summary>
        /// Property used for the name of the tab, based on the file name.
        /// </summary>
        public string TabFileName
        {
            get => _tabFileName;
            set
            {
                Set(ref _tabFileName, value);
            }
        }

        /// <summary>
        /// Property used to modify the DataGrid height based on window height.
        /// </summary>
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

        private void ToggleColumnVisibility()
        {
            IdIsValid = true;
            LogLevelIsValid = true;
            TextIsValid = true;
            ThreadIdIsValid = true;
            EventIdIsValid = true;
            DateTimeIsValid = true;
            _filterCount = 5;
            if (_parsingStrategy is NoThreadIdParsingStrategy)
            {
                ThreadIdIsValid = false;
                _filterCount = 4;
            }

            if (_parsingStrategy is NoEventIdParsingStrategy)
            {
                EventIdIsValid = false;
                _filterCount = 4;
            }

            if (_parsingStrategy is StringOnlyParsingStrategy)
            {
                LogLevelIsValid = false;
                ThreadIdIsValid = false;
                EventIdIsValid = false;
                DateTimeIsValid = false;
                _filterCount = 1;
            }
        }

        //Every time the user changes any filter field, all the filter information goes through the list, filters it, then compares all the filters with each other,
        //and we get the result in the single list. Special symbols are used to tell the method that the checkboxes for ,,also show dates who have minimum value"" and
        //,,case sensivitivy is enabled''.
        private void OnChangeCreateFilter()
        {
            var filterFactory = new DateTimeFilter();
            if (_noDateCheckBox)
            {
                AddRemoveFilter(filterFactory.Filter(_backupList.OfType<IStandardModel>(), DateTimeSearch1 + "¥" + DateTimeSearch2 + "¢"));
            }
            else
            {
                AddRemoveFilter(filterFactory.Filter(_backupList.OfType<IStandardModel>(), DateTimeSearch1 + "¥" + DateTimeSearch2));
            }

            var filterFactory2 = new ThreadIdFilter();
            if (!(_parsingStrategy is NoThreadIdParsingStrategy))
            {
                AddRemoveFilter(filterFactory2.Filter(_backupList.OfType<IThreadIdModel>(), ThreadIdSearch));
            }

            var filterFactory3 = new LogLevelFilter();
            AddRemoveFilter(filterFactory3.Filter(_backupList.OfType<IStandardModel>(), LogLevelValues.ToString()));

            if (!(_parsingStrategy is NoEventIdParsingStrategy))
            {
                var filterFactory4 = new EventIdFilter();
                AddRemoveFilter(filterFactory4.Filter(_backupList.OfType<IEventIdModel>(), EventIdSearch));
            }

            if (!_regexSearchCheckBox)
            {
                var filterFactory5 = new TextFilter();
                if (_caseSensitiveCheckBox)
                {
                    AddRemoveFilter(filterFactory5.Filter(_backupList, LogTextSearch));
                }
                else
                {
                    AddRemoveFilter(filterFactory5.Filter(_backupList, LogTextSearch + "¢"));
                }
            }
            else
            {
                var filterFactory6 = new RegexFilter();
                AddRemoveFilter(filterFactory6.Filter(_backupList, LogTextSearch));
            }
        }

        //Goes through a list of filters, finds their common objects and produces a list.
        private void AddRemoveFilter(IEnumerable<IModel> list)
        {
            ListFilters.Add( new ObservableCollection<IModel>(list));

            if (ListFilters.Count == _filterCount)
            {
                IEnumerable<IModel> filterResult = null;

                for (int x = 0; x < ListFilters.Count - 1; x++)
                {
                    if (x == 0)
                    {
                        filterResult = ListFilters[x].Intersect(ListFilters[x + 1]).ToList();
                        continue;
                    }
                    filterResult = filterResult.Intersect(ListFilters[x + 1]).ToList();
                }
                ListLoadLine = new ObservableCollection<IModel>(filterResult);
                ListFilters.Clear();
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
            List<IModel> parsingCollection = _parsingStrategy.Parse(logFileData);
            if (parsingCollection == null)
            {
                return;
            }
            ListLoadLine = new ObservableCollection<IModel>(_parsingStrategy.Parse(logFileData));
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
            _listLoadLine = new ObservableCollection<IModel>(_parsingStrategy.Parse(GetLines(logFilePath)));
            _backupList = _listLoadLine;
            OnChangeCreateFilter();
        }
    }
}
