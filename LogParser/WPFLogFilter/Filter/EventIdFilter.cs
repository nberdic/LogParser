using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;

namespace WPFLogFilter.Filter
{
    public class EventIdFilter : IFilter
    {
        public ObservableCollection<LogModel> Filter(ObservableCollection<LogModel> list, string search)
        {
            if (!search.Equals(string.Empty))
            {
                list = new ObservableCollection<LogModel>(list.Where(x => x.EventId.ToString().Contains(search)));
            }
            return list;
        }
    }
}
