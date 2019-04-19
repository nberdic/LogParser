﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;

namespace WPFLogFilter.ParsingFactoryStrategyFolder.ParsingStrategyFolder
{
    public class NoThreadIdParsingStrategy : IParsingStrategy
    {
        public List<LogModel> Parse(string[] lines)
        {
            List<LogModel> tempList = new List<LogModel>();

            for (int x = 0; x < lines.Length; x++)
            {
                string[] arrayOfParts = lines[x].Split('|');
                if ((lines[x] != "") && (arrayOfParts.Count() == 5))
                {
                    int id = x + 1;
                    DateTime dateTime = DateTime.Parse(arrayOfParts[0] + " " + arrayOfParts[1]);
                    string threadId = "";
                    string logLevel = arrayOfParts[2];
                    int eventId = int.Parse(arrayOfParts[3]);
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
