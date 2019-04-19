using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFLogFilter.DialogWrapperFolder;
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
        private IDialogWrapper _dialogWrapper;
        private IParsingFactory _parsingFactory;
        private IFilterFactory _filterfactory;
        private ObservableCollection<ObservableCollection<LogModel>> listFilters;
        private string fileName;
        private string nameAndVersion;

        private string stopWatch = "";

        IParsingStrategy parsingStrategy;

        private LogLevelEnum logLevelValues;

        private bool idTrigger = false;
        private bool dateTimeTrigger = false;
        private bool threadIdTrigger = false;
        private bool logLevelTrigger = false;
        private bool eventIdTrigger = false;
        private bool textTrigger = false;
        private bool caseSensitiveCheckBox = true;
        private bool openFileTrigger = false;

        private ObservableCollection<LogModel> idList;
        private ObservableCollection<LogModel> dateTimeList;
        private ObservableCollection<LogModel> threadIdList;
        private ObservableCollection<LogModel> logLevelList;
        private ObservableCollection<LogModel> eventIdList;
        private ObservableCollection<LogModel> regexList;

        private string logTextSearch = "";
        private string idNumberSearch = "";
        private string dateTimeSearch1 = "";
        private string dateTimeSearch2 = "";

        private string threadIdSearch = "";
        private string logLevelSearch = "";
        private string eventIdSearch = "";

        private ObservableCollection<LogModel> backup;

        public MainViewModel(IDialogWrapper dialogWrapper, IParsingFactory iParsingFactory, IFilterFactory iFilterFactory)
        {
            _dialogWrapper = dialogWrapper;
            _parsingFactory = iParsingFactory;
            _filterfactory = iFilterFactory;

            ListLoadLine = new ObservableCollection<LogModel>();
            ListFilters = new ObservableCollection<ObservableCollection<LogModel>>();

            idList = new ObservableCollection<LogModel>();
            eventIdList = new ObservableCollection<LogModel>();
            dateTimeList = new ObservableCollection<LogModel>();
            threadIdList = new ObservableCollection<LogModel>();
            logLevelList = new ObservableCollection<LogModel>();
            eventIdList = new ObservableCollection<LogModel>();
            regexList = new ObservableCollection<LogModel>();

            ClickMenuCommand = new RelayCommand(MenuItem);
            OpenNotepadCommand = new RelayCommand(OpenNotepad);
        }

        public RelayCommand ClickMenuCommand { get; set; }
        public RelayCommand OpenNotepadCommand { get; set; }

        public ObservableCollection<LogModel> ListLoadLine
        {
            get { return _listLoadLine; }
            set
            {
                Set(ref _listLoadLine, value);

                if (ListLoadLine.Count != 0)
                {
                    TriggerType();
                }
            }
        }

        public enum LogLevelEnum
        {
            ALL,
            DEBUG,
            TRACE,
            INFO,
            WARN,
            ERROR,
            FATAL,
            ALERT
        }

        public IList<LogLevelEnum> LogLevelEnumList
        {
            get
            {
                return Enum.GetValues(typeof(LogLevelEnum)).Cast<LogLevelEnum>().ToList();
            }
        }

        public bool CaseSensitiveCheckBox
        {
            get => caseSensitiveCheckBox; set
            {
                Set(ref caseSensitiveCheckBox, value);
                IFilter filterFactory = _filterfactory.Create(6);
                if (caseSensitiveCheckBox == false)
                {
                    ListLoadLine = AddRemoveFilter(filterFactory.Filter(backup, LogTextSearch + "¢"), 6);

                    // ListLoadLine = filterFactory.Filter(backup, LogTextSearch + "¢");
                }
                else
                {
                    ListLoadLine = AddRemoveFilter(filterFactory.Filter(backup, LogTextSearch), 6);
                    // ListLoadLine = filterFactory.Filter(backup, LogTextSearch);
                }
            }
        }

        public bool IdTrigger
        {
            get => idTrigger; set
            {
                Set(ref idTrigger, value);
            }
        }
        public bool DateTimeTrigger
        {
            get => dateTimeTrigger; set
            {
                Set(ref dateTimeTrigger, value);
            }
        }
        public bool ThreadIdTrigger
        {
            get => threadIdTrigger; set
            {
                Set(ref threadIdTrigger, value);
            }
        }
        public bool LogLevelTrigger
        {
            get => logLevelTrigger; set
            {
                Set(ref logLevelTrigger, value);
            }
        }
        public bool EventIdTrigger
        {
            get => eventIdTrigger; set
            {
                Set(ref eventIdTrigger, value);
            }
        }

        public bool TextTrigger
        {
            get => textTrigger; set
            {
                Set(ref textTrigger, value);
            }
        }

        public string LogTextSearch
        {
            get => logTextSearch;
            set
            {
                Set(ref logTextSearch, value);
                IFilter filterFactory = _filterfactory.Create(6);
                if (caseSensitiveCheckBox == false)
                {
                    ListLoadLine = AddRemoveFilter(filterFactory.Filter(backup, LogTextSearch + "¢"), 6);

                    // ListLoadLine = filterFactory.Filter(backup, LogTextSearch + "¢");
                }
                else
                {
                    ListLoadLine = AddRemoveFilter(filterFactory.Filter(backup, LogTextSearch), 6);
                    // ListLoadLine = filterFactory.Filter(backup, LogTextSearch);
                }

            }
        }
        public string IdNumberSearch
        {
            get => idNumberSearch; set
            {
                Set(ref idNumberSearch, value);
                IFilter filterFactory = _filterfactory.Create(1);
                //   ListLoadLine = filterFactory.Filter(backup, idNumberSearch);

                ListLoadLine = AddRemoveFilter(filterFactory.Filter(backup, idNumberSearch), 1);
                // ListFilters.Add(filterFactory.Filter(backup, idNumberSearch));
            }
        }
        public string DateTimeSearch1
        {
            get => dateTimeSearch1; set
            {
                Set(ref dateTimeSearch1, value);
                IFilter filterFactory = _filterfactory.Create(2);
                ListLoadLine = AddRemoveFilter(filterFactory.Filter(backup, DateTimeSearch1 + "+" + DateTimeSearch2), 2);
                //ListLoadLine = filterFactory.Filter(backup, dateTimeSearch1 + "+" + dateTimeSearch2);
            }
        }

        public string DateTimeSearch2
        {
            get => dateTimeSearch2; set
            {
                Set(ref dateTimeSearch2, value);
                IFilter filterFactory = _filterfactory.Create(2);

                ListLoadLine = AddRemoveFilter(filterFactory.Filter(backup, DateTimeSearch1 + "+" + DateTimeSearch2), 2);
                //  ListLoadLine = filterFactory.Filter(backup, dateTimeSearch1 + "+" + dateTimeSearch2);
            }
        }

        public string ThreadIdSearch
        {
            get => threadIdSearch; set
            {
                Set(ref threadIdSearch, value);
                IFilter filterFactory = _filterfactory.Create(3);
                ListLoadLine = AddRemoveFilter(filterFactory.Filter(backup, ThreadIdSearch), 3);
                //   ListLoadLine = filterFactory.Filter(backup, threadIdSearch);
            }
        }
        public string LogLevelSearch
        {
            get => logLevelSearch; set
            {
                Set(ref logLevelSearch, value);
                IFilter filterFactory = _filterfactory.Create(4);

                ListLoadLine = AddRemoveFilter(filterFactory.Filter(backup, LogLevelSearch), 4);

                //    ListLoadLine = AddRemoveFilter(filterFactory.Filter(backup, logLevelSearch), 4);

                //   ListLoadLine = filterFactory.Filter(backup, logLevelSearch);
            }
        }
        public string EventIdSearch
        {
            get => eventIdSearch; set
            {
                Set(ref eventIdSearch, value);
                IFilter filterFactory = _filterfactory.Create(5);

                ListLoadLine = AddRemoveFilter(filterFactory.Filter(backup, EventIdSearch), 5);

                //  ListLoadLine = AddRemoveFilter(filterFactory.Filter(backup, eventIdSearch), 5);
                //  ListLoadLine = filterFactory.Filter(backup, eventIdSearch);
            }
        }

        public LogLevelEnum LogLevelValues
        {
            get => logLevelValues; set
            {
                Set(ref logLevelValues, value);
                IFilter filterFactory = _filterfactory.Create(4);

                ListLoadLine = AddRemoveFilter(filterFactory.Filter(backup, LogLevelValues.ToString()), 5);

                //  ListLoadLine = filterFactory.Filter(backup, logLevelValues.ToString());
            }
        }

        public ObservableCollection<ObservableCollection<LogModel>> ListFilters
        {
            get => listFilters; set
            {
                Set(ref listFilters, value);
            }
        }

        public string StopWatch { get => stopWatch; set
            {
                Set(ref stopWatch, value);
            }
        }

        public bool OpenFileTrigger { get => openFileTrigger; set
            {
                Set(ref openFileTrigger, value);
            }
        }

        public string NameAndVersion { get => nameAndVersion; set
            {
                Set(ref nameAndVersion, value);
            }
        }

        public void MenuItem()
        {
            PopulateList(_dialogWrapper.GetLines(ref fileName));
        }

        public void OpenNotepad()
        {
            Process.Start("notepad.exe", fileName);
        }

        public void PopulateList(string[] lines)
        {
            if (lines != null)
            {
                for (int x = 0; x < lines.Length; x++)
                {
                    if (!string.IsNullOrEmpty(lines[x]))
                    {
                        var stopWatch = System.Diagnostics.Stopwatch.StartNew();

                        parsingStrategy = _parsingFactory.Create(lines[x]);
                        FileNameAndVersion();
                        //stopWatch.Stop();

                        //var timePassed1 = stopWatch.ElapsedMilliseconds;

                        //stopWatch.Start();
                        //  await Task.Run(() => ListLoadLine = new ObservableCollection<LogModel>(parsingStrategy.Parse(lines)));
                        ListLoadLine = new ObservableCollection<LogModel>(parsingStrategy.Parse(lines));

                        stopWatch.Stop();
                        var timePassed = stopWatch.ElapsedMilliseconds;
                        StopWatch += $"F: {timePassed} ";
                        backup = ListLoadLine;
                        break;
                    }
                }
            }
        }

        public void FileNameAndVersion()
        {
            string version= System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            string[] results = fileName.Split(new char[] { '\\', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            NameAndVersion = "v" + version + " File name: "+results[results.Length-1] ;
        }

        public void TriggerType()
        {
            IdTrigger = true;
            LogLevelTrigger = true;
            TextTrigger = true;
            ThreadIdTrigger = true;
            EventIdTrigger = true;
            DateTimeTrigger = true;
            OpenFileTrigger = true;
            if (parsingStrategy is NoThreadIdParsingStrategy)
            {
                ThreadIdTrigger = false;
            }

            if (parsingStrategy is NoEventIdParsingStrategy)
            {
                EventIdTrigger = false;
            }

            if (parsingStrategy is StringOnlyParsingStrategy)
            {
                LogLevelTrigger = false;
                ThreadIdTrigger = false;
                EventIdTrigger = false;
                DateTimeTrigger = false;
            }
        }

        public ObservableCollection<LogModel> AddRemoveFilter(ObservableCollection<LogModel> list, int caseNo)
        {
            switch (caseNo)
            {
                case 1:
                    if (idList.Count != 0)
                    {
                        ListFilters.Remove(idList);
                    }
                    idList = list;
                    break;
                case 2:
                    if (dateTimeList.Count != 0)
                    {
                        ListFilters.Remove(dateTimeList);
                    }
                    dateTimeList = list;
                    break;
                case 3:
                    if (threadIdList.Count != 0)
                    {
                        ListFilters.Remove(threadIdList);
                    }
                    threadIdList = list;
                    break;
                case 4:
                    if (logLevelList.Count != 0)
                    {
                        ListFilters.Remove(logLevelList);
                    }
                    logLevelList = list;
                    break;
                case 5:
                    if (eventIdList.Count != 0)
                    {
                        ListFilters.Remove(eventIdList);
                    }
                    eventIdList = list;
                    break;
                case 6:
                    if (regexList.Count != 0)
                    {
                        ListFilters.Remove(regexList);
                    }
                    regexList = list;
                    break;
                default:
                    break;
            }

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

    }
}
