using System;

namespace WPFLogFilter.Model
{
    /// <summary>
    /// This class is used to model all the columns inside log files.
    /// </summary>
    public class LogModel
    {
        private int _id;
        private DateTime _dateTime;
        private string _threadId;
        private string _logLevel;
        private int _eventId;
        private string _text;
        private string _firstText;
        private string _highLightedText=string.Empty;
        private string _lastText=string.Empty;

        /// <summary>
        /// LogModel constructor.
        /// </summary>
        public LogModel() { }

        /// <summary>
        /// LogModel constructor which transfers the line string into one of the three text containters, so if the search occurs the text can be highlighted.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateTime"></param>
        /// <param name="threadId"></param>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="text"></param>
        public LogModel(int id, DateTime dateTime, string threadId, string logLevel, int eventId, string text)
        {
            Id = id;
            DateTime = dateTime;
            ThreadId = threadId;
            LogLevel = logLevel;
            EventId = eventId;
            Text = text;
            FirstText = text;
    }

        /// <summary>
        /// This property is used to store the assigned ID number of each line of string.
        /// </summary>
        public int Id { get => _id; set => _id = value; }

        /// <summary>
        /// This property is used to store the converted value of the first two columns of each log line.
        /// </summary>
        public DateTime DateTime { get => _dateTime; set => _dateTime = value; }
        /// <summary>
        /// This property is used to store the value from the ThreadId column.
        /// </summary>
        public string ThreadId { get => _threadId; set => _threadId = value; }

        /// <summary>
        /// This property is used to store the value from the LogLevel column.
        /// </summary>
        public string LogLevel { get => _logLevel; set => _logLevel = value; }

        /// <summary>
        /// This property is used to store the value from the EventID column.
        /// </summary>
        public int EventId { get => _eventId; set => _eventId = value; }

        /// <summary>
        /// This property is used to store the value from the Text column.
        /// </summary>
        public string Text { get => _text; set => _text = value; }

        /// <summary>
        /// This property is used to store the value from the whole Text column.
        /// Later it's used to store the value of the text column for the first part of the non-highlighted text.
        /// </summary>
        public string FirstText { get => _firstText; set => _firstText = value; }

        /// <summary>
        /// This property is used to store the value of the highlighted part of the text column.
        /// </summary>
        public string HighLightedText { get => _highLightedText; set => _highLightedText = value; }

        /// <summary>
        /// This property is used to store the value of the second part of the non-highlighted part of the text column.
        /// </summary>
        public string LastText { get => _lastText; set => _lastText = value; }

    }
}
