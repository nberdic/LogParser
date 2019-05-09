using System.Collections.ObjectModel;
using System.Linq;
using WPFLogFilter.Model;

namespace WPFLogFilter.Filter
{
    public class EventIdFilter : IFilter
    {
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
