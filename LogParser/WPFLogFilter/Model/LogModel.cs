using System;

namespace WPFLogFilter.Model
{
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

        public LogModel() { }

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

        public int Id { get => _id; set => _id = value; }
        public DateTime DateTime { get => _dateTime; set => _dateTime = value; }
        public string ThreadId { get => _threadId; set => _threadId = value; }
        public string LogLevel { get => _logLevel; set => _logLevel = value; }
        public int EventId { get => _eventId; set => _eventId = value; }
        public string Text { get => _text; set => _text = value; }
        public string FirstText { get => _firstText; set => _firstText = value; }
        public string HighLightedText { get => _highLightedText; set => _highLightedText = value; }
        public string LastText { get => _lastText; set => _lastText = value; }
    }
}
