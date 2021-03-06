﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;

namespace WPFLogFilter.Filter
{
    /// <summary>
    /// This class is used to filter the LogLevel column.
    /// </summary>
    public class LogLevelFilter : IFilter
    {
        /// <summary>
        /// This method is used to filter the LogLevel column based on the field we selected in the combobox.
        /// </summary>
        /// <param name="list">List of log objects</param>
        /// <param name="search">Combobox choice converted to string</param>
        /// <returns></returns>
        public ObservableCollection<LogModel> Filter(ObservableCollection<LogModel> list, string search)
        {
            if ((list != null) && (!search.Equals("ALL")))
            {
                list = new ObservableCollection<LogModel>(list.Where(x => x.LogLevel.Trim().Contains(search.Trim())));
            }
            return list;
        }
    }
}
