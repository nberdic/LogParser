using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;

namespace WPFLogFilter.Filter
{
    public class LogLevelFilter : IFilter
    {
        public ObservableCollection<LogModel> Filter(ObservableCollection<LogModel> list, string search)
        {
            if ((list != null) && (!search.Equals("ALL")))
            {
                list = new ObservableCollection<LogModel>(list.Where(x => x.LogLevel.Trim().Contains(search)));
            }

            return list;
        }
    }
}
