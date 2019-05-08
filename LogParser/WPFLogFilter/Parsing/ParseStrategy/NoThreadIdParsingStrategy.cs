﻿using System;
using System.Collections.Generic;
using System.Linq;
using WPFLogFilter.Model;

namespace WPFLogFilter.Parsing.ParseStrategy
{
    public class NoThreadIdParsingStrategy : IParsingStrategy
    {
        public List<LogModel> Parse(string[] lines)
        {
            List<LogModel> tempList = new List<LogModel>();
            DateTime dateTime;

            for (int x = 0; x < lines.Length; x++)
            {
                string[] arrayOfParts = lines[x].Split('|');
                if ((lines[x] != "") && (arrayOfParts.Count() == 5))
                {
                    int id = x + 1;
                    int eventId;
                    if (!DateTime.TryParse(arrayOfParts[0] + " " + arrayOfParts[1], out dateTime))
                    {
                        dateTime = DateTime.MinValue;
                    }

                    string threadId = "";
                    string logLevel = arrayOfParts[2];
                    if (!int.TryParse(arrayOfParts[3], out eventId))
                    {
                        eventId = -1;
                    }
                    string text = arrayOfParts[4].Trim();

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
