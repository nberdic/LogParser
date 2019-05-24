using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLogFilter.Model
{
    public class CompleteTSUModel : IStandardModel, IThreadIdModel, IEventIdModel
    {
        public CompleteTSUModel(int id, DateTime dateTime, string threadId, string logLevel, int eventId, string text, bool isValid)
        {
            Id = id;
            DateTime = dateTime;
            ThreadId = threadId;
            LogLevel = logLevel;
            EventId = eventId;
            TextFull = text;
            TextFirstPart = text;
            IsValid = isValid;
        }

        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string ThreadId { get; set; }
        public string LogLevel { get; set; }
        public int EventId { get; set; }
        public string TextFull { get; set; }
        public string TextFirstPart { get; set; }
        public string TextHighlightedPart { get; set; }
        public string TextSecondPart { get; set; }
        public bool IsValid { get; set; }
    }
}
