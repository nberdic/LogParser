using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;

namespace WPFLogFilter.Filter
{
    public class DateTimeFilter : IFilter
    {
        public ObservableCollection<LogModel> Filter(ObservableCollection<LogModel> list, string search)
        {
            string[] times = search.Split('+');

            if (search.Length == 17)
            {
                DateTime date = DateTime.MinValue;

                foreach (var item in list)
                {
                    if (!item.DateTime.ToString().Equals("01-01-0001 00:00:00"))
                    {
                        date = item.DateTime;
                    }
                }

                if ((date != DateTime.MinValue) && (DateTime.TryParse(date.ToShortDateString() + " " + times[0], out DateTime dt1))
                    && (DateTime.TryParse(date.ToShortDateString() + " " + times[1], out DateTime dt2)))
                {
                    dt2 = dt2.AddSeconds(1);
                    var query = (from item in list
                                 where (item.DateTime <= dt2) && (item.DateTime >= dt1)
                                 select item).ToList();

                    return new ObservableCollection<LogModel>(query);
                }

                else
                {
                    return list;
                }
            }

            else
            {
                return list;
            }



        }
    }
}
