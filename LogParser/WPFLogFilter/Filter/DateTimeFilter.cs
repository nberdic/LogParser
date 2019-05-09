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
            search = search.Trim();

            if (string.IsNullOrWhiteSpace(search))
            {
                return list;
            }
                if ((search.Equals("¢") || (search.Equals("¥"))))
                {
                    return list;
                }
                
                int noDateSensitive = 0;
                string[] times = search.Split('¥');

                if (times[1].EndsWith("¢"))
                {
                    noDateSensitive = 1;
                    times[1] = times[1].Remove(times[1].Length - 1);
                    search = search.Remove(search.Length - 1);
                }

                if (!times[0].Equals("00:00:00"))
                {
                    if (!DateTime.TryParse(times[0], out DateTime temp1))
                    {
                        return list;
                    }
                    else
                    {
                        times[0] = temp1.ToString("HH:mm:ss");
                    }
                }

                if (!times[1].Equals("23:59:59"))
                {
                    if (!DateTime.TryParse(times[1], out DateTime temp2))
                    {
                        return list;
                    }
                    else
                    {
                        times[1] = temp2.ToString("HH:mm:ss");
                    }
                }

                if ((times[0].Length == 8) || (times[1].Length == 8))
                {
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
    }
}
