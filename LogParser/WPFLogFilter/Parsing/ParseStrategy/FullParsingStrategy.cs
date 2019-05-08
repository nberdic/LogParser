using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;

namespace WPFLogFilter.Parsing.ParseStrategy
{
    public class FullParsingStrategy : IParsingStrategy
    {
        public List<LogModel> Parse(string[] lines)
        {
            List<LogModel> tempList = new List<LogModel>();

            for (int x = 0; x < lines.Length; x++)
            {
                string[] arrayOfParts = lines[x].Split('|');
                if ((lines[x] != "") && (arrayOfParts.Count() == 6))
                {
                    int id = x + 1;
                    int eventId;

                    DateTime dateTime = DateTime.Parse(arrayOfParts[0] + " " + arrayOfParts[1]);
                   
                    string threadId = arrayOfParts[2];
                    string logLevel = arrayOfParts[3];
                    if (!int.TryParse(arrayOfParts[4], out eventId))
                    {
                        eventId = -1;
                    }
                   
                    string text = arrayOfParts[5].Trim();

                    tempList.Add(new LogModel(id, dateTime, threadId, logLevel, eventId, text));
                }
                else
                {
                    tempList.Add(new LogModel(x + 1, DateTime.MinValue, "", "", -1, lines[x]));
                }
            }

            return tempList;
        }
    }
}

