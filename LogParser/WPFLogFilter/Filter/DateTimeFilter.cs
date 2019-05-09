using System;
using System.Collections.ObjectModel;
using System.Linq;
using WPFLogFilter.Model;

namespace WPFLogFilter.Filter
{
    public class DateTimeFilter : IFilter
    {
        public ObservableCollection<LogModel> Filter(ObservableCollection<LogModel> list, string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                if ((search.Length == 17) || (search.Length == 18 && search.EndsWith("¢")))
                {
                    int noDateSensitive = 0;
                    string[] times = search.Split('¥');

                    if (times[1].EndsWith("¢"))
                    {
                        noDateSensitive = 1;
                        times[1] = times[1].Remove(times[1].Length - 1);
                        search = search.Remove(search.Length - 1);
                    }

                    DateTime date = DateTime.MinValue;

                    foreach (var item in list)
                    {
                        if (item.DateTime != DateTime.MinValue)
                        {
                            date = item.DateTime;
                            break;
                        }
                    }

                    if ((date != DateTime.MinValue) && (DateTime.TryParse(date.ToShortDateString() + " " + times[0], out DateTime dt1))
                        && (DateTime.TryParse(date.ToShortDateString() + " " + times[1], out DateTime dt2)))
                    {
                        if (noDateSensitive == 1)
                        {
                            dt2 = dt2.AddSeconds(1);
                            var query = (from item in list
                                         where (((item.DateTime > DateTime.MinValue && item.DateTime <= dt2) && item.DateTime >= dt1) || item.DateTime == DateTime.MinValue)
                                         select item).ToList();

                            return new ObservableCollection<LogModel>(query);
                        }
                        else
                        {
                            dt2 = dt2.AddSeconds(1);
                            var query = (from item in list
                                         where (item.DateTime <= dt2) && (item.DateTime >= dt1)
                                         select item).ToList();

                            return new ObservableCollection<LogModel>(query);
                        }
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
            else
            {
                return list;
            }
        }
    }
}
