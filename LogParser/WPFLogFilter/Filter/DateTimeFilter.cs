using System;
using System.Collections.ObjectModel;
using System.Linq;
using WPFLogFilter.Model;

namespace WPFLogFilter.Filter
{
    /// <summary>
    /// this class is used to filter the log lines with the beginning and end dates.
    /// </summary>
    public class DateTimeFilter : IFilter
    {
        /// <summary>
        /// This method is used to filter the log lines using the in-between-two-dates criteria.
        /// </summary>
        /// <param name="list">List of log objects</param>
        /// <param name="search">Two Dates combined into a string and used as criteria for filtering</param>
        /// <returns></returns>
        public ObservableCollection<LogModel> Filter(ObservableCollection<LogModel> list, string search)
        {
            search = search.Trim();

            if (string.IsNullOrWhiteSpace(search))
            {
                return list;
            }

            //The ¥ symbol is used to indicate the end of first date and the beginning of the second date from search string, and ¢ symbol is for checkbox toggle 
            //so the filter shows results that have DateTime.MinValue

            //if string contains only symbols that means that it doesn't have a single date, so return list
            if ((search.Equals("¢") || (search.Equals("¥"))))
            {
                return list;
            }

            int noDateSensitive = 0;
            string[] times = search.Split('¥');

            //¢ at the end means show DateTime.Minvalue, we toggle a check, and remove the symbol so we can convert 2 strings into 2 dates
            if (times[1].EndsWith("¢"))
            {
                noDateSensitive = 1;
                times[1] = times[1].Remove(times[1].Length - 1);
                search = search.Remove(search.Length - 1);
            }

            //00:00:00 is the default value for the first datetime filter, that means the value wasn't change and it doesn't require a string to date conversion.
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

            //23:59:59 is the default value for the second datetime filter, that means the value wasn't change and it doesn't require a string to date conversion.
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

            //Length = 8 is the correct format needed to convert our strings to datetime, we don't filter if this criteria isn't satisfied
            if ((times[0].Length == 8) || (times[1].Length == 8))
            {
                DateTime date = DateTime.MinValue;

                //Because we only get Time from our filter strings, we take the date from the list that we loaded
                foreach (var item in list)
                {
                    if (item.DateTime != DateTime.MinValue)
                    {
                        date = item.DateTime;
                        break;
                    }
                }

                //we attach the time from strings to the date taken from a list we loaded
                if ((date != DateTime.MinValue) && (DateTime.TryParse(date.ToShortDateString() + " " + times[0], out DateTime dt1))
                    && (DateTime.TryParse(date.ToShortDateString() + " " + times[1], out DateTime dt2)))
                {
                    //if the symbol ¢ was in the string, that means that we need all of the results + the results with DateTime.MinValue
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
