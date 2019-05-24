using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLogFilter.Model
{
    public class NoEventIdTSUModel : IStandardModel, IThreadIdModel
    {
        public NoEventIdTSUModel(int id, DateTime dateTime, string threadId, string logLevel, string text, bool isValid)
        {
            Id = id;
            DateTime = dateTime;
            ThreadId = threadId;
            LogLevel = logLevel;
            TextFull = text;
            TextFirstPart = text;
            IsValid = isValid;
        }

        public int Id { get; set; }
        public DateTime DateTime { get; set; }

        public string ThreadId { get; set; }
        public string LogLevel { get; set; }
        public string TextFull { get; set; }
        public string TextFirstPart { get; set; }
        public string TextHighlightedPart { get; set; }
        public string TextSecondPart { get; set; }
        public bool IsValid { get; set; }

    }

}
