using System;
using System.Collections.Generic;
using System.Linq;
using WPFLogFilter.Model;

namespace WPFLogFilter.Parsing.ParseStrategy
{
    /// <summary>
    /// This class is used to help us parse the lines of the logs and put them into the correct format.
    /// </summary>
    public class NoThreadIdParsingStrategy : IParsingStrategy
    {
        /// <summary>
        /// This method takes the lines from log and it parses them into 5 columns, or if it can't it returns the damaged columns as default values.
        /// </summary>
        /// <param name="lines">List of log lines</param>
        /// <returns></returns>
        public List<IModel> Parse(string[] lines)
        {
            List<IModel> tempList = new List<IModel>();
            DateTime dateTime;

            //Checking if the log line fits this strategy which has 5 columns
            for (int x = 0; x < lines.Length; x++)
            {
                string[] arrayOfParts = lines[x].Split('|');
                if ((lines[x] != "") && (arrayOfParts.Count() == 5))
                {
                    int id = x + 1;
                    int eventId;
                    bool isValid = true;

                    //Combines the first column , which has the date, and the second column which has the time. If it fails, the value is set to DateTime.MinValue
                    if (!DateTime.TryParse(arrayOfParts[0] + " " + arrayOfParts[1], out dateTime))
                    {
                        dateTime = DateTime.MinValue;
                        isValid = false;
                    }

                    string logLevel = arrayOfParts[2];

                    // In case a column is damaged it will replace the value with the minimum value.
                    if (!int.TryParse(arrayOfParts[3], out eventId))
                    {
                        isValid = false;
                    }
                    string text = arrayOfParts[4].Trim();

                    tempList.Add(new NoThreadIdTSUModel(id, dateTime, logLevel, eventId, text, isValid));
                }
                // In case a column can't be parsed it will be replaced with the minimum value, but still show the entire line.
                else
                {
                    tempList.Add(new StringOnlyModel(lines[x]));
                }
            }
            return tempList;
        }

    }
}
