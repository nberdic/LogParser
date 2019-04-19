using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;

namespace WPFLogFilter.ParsingFactoryStrategyFolder.ParsingStrategyFolder
{
    public class FullParsingStrategy : IParsingStrategy
    {
        public List<LogModel> Parse(string[] lines)
        {
            List<LogModel> tempList = new List<LogModel>();

            /*
            Parallel.For (0, lines.Length, (x) =>
            {
                string[] arrayOfParts = lines[x].Split('|');
                if ((lines[x] != "") && (arrayOfParts.Count() == 6))
                {
                    int id = x + 1;
                    DateTime dateTime = DateTime.Parse(arrayOfParts[0] + " " + arrayOfParts[1]);
                    string threadId = arrayOfParts[2];
                    string logLevel = arrayOfParts[3];
                    int eventId = int.Parse(arrayOfParts[4]);
                    string text = arrayOfParts[5].Trim();

                    tempList.Add(new LogModel(id, dateTime, threadId, logLevel, eventId, text));
                }
                else
                {
                    tempList.Add(new LogModel(x + 1, DateTime.MinValue, "", "", -1, lines[x]));
                }
            });
            */

           
            for (int x = 0; x < lines.Length; x++)
            {
                string[] arrayOfParts = lines[x].Split('|');
                if ((lines[x] != "") && (arrayOfParts.Count() == 6))
                {
                    int id = x + 1;
                    DateTime dateTime = DateTime.Parse(arrayOfParts[0] + " " + arrayOfParts[1]);
                    string threadId = arrayOfParts[2];
                    string logLevel = arrayOfParts[3];
                    int eventId = int.Parse(arrayOfParts[4]);
                    string text = arrayOfParts[5].Trim();

                    tempList.Add(new LogModel(id, dateTime, threadId, logLevel, eventId, text));
                }
                else
                {
                    tempList.Add(new LogModel(x+1, DateTime.MinValue, "", "", -1, lines[x]));
                }
            }
            

            return tempList;
        }
    }
}

