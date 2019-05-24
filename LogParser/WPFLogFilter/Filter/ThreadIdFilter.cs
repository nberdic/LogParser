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
    public class ThreadIdFilter : IFilter<IThreadIdModel>
    {
        /// <summary>
        /// This method is used to filter the ThreadId column.
        /// </summary>
        /// <param name="list">List of log objects</param>
        /// <param name="search">ThreadId textbox criteria</param>
        /// <returns></returns>
        public IEnumerable<IThreadIdModel> Filter(IEnumerable<IThreadIdModel> list, string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                list = list.Where(x => x.ThreadId.Contains(search.Trim()));
            }
            return list;
        }
    }
}
