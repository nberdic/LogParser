﻿using System.Collections.ObjectModel;
using System.Linq;
using WPFLogFilter.Model;

namespace WPFLogFilter.Filter
{
    /// <summary>
    /// This class is used to filter eventId column.
    /// </summary>
    public class EventIdFilter : IFilter
    {
        /// <summary>
        /// This method filters the list of log lines with the EventId search textbox.
        /// </summary>
        /// <param name="list">List of log objects</param>
        /// <param name="search">EventId search criteria</param>
        /// <returns></returns>
        public ObservableCollection<LogModel> Filter(ObservableCollection<LogModel> list, string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                list = new ObservableCollection<LogModel>(list.Where(x => x.EventId.ToString().Contains(search.Trim()) && x.EventId !=-1));
            }
            return list;
        }
    }
}
