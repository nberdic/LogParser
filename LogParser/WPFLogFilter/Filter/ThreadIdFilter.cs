using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;

namespace WPFLogFilter.Filter
{
    /// <summary>
    /// This class is used to filter the ThreadId column.
    /// </summary>
    public class ThreadIdFilter : IFilter
    {
        /// <summary>
        /// This method is used to filter the ThreadId column.
        /// </summary>
        /// <param name="list">List of log objects</param>
        /// <param name="search">ThreadId textbox criteria</param>
        /// <returns></returns>
        public ObservableCollection<LogModel> Filter(ObservableCollection<LogModel> list, string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                list = new ObservableCollection<LogModel>(list.Where(x => x.ThreadId.Contains(search.Trim())));
            }
            return list;
        }
    }
}
