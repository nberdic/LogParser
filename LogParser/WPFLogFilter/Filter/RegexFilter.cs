using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WPFLogFilter.Model;

namespace WPFLogFilter.Filter
{
    /// <summary>
    /// This class is used to filter the text column.
    /// </summary>
    public class RegexFilter : IFilter
    {
        /// <summary>
        /// This method filters the text using the Regular expression
        /// <param name="list">List of Log objects</param>
        /// <param name="searchText">Search criteria</param>
        /// <returns></returns>
        public ObservableCollection<LogModel> Filter(ObservableCollection<LogModel> list, string searchText)
        {
            //If the search field is empty, reset to default
            if (string.IsNullOrWhiteSpace(searchText))
            {
                foreach (var model in list)
                {
                    model.FirstText = model.Text;
                    model.HighLightedText = string.Empty;
                    model.LastText = string.Empty;
                }
                return list;
            }

            //Use regex and try to filter text, if there is an error, don't filter, and return the list.
            try
            {
                list = new ObservableCollection<LogModel>(list.Where(x => Regex.Match(x.Text, searchText).Success));
            }
            catch (Exception)
            {
                return list;
            }

            //highlight the result and not the regex search string because regex could be ([A-Z])\w+.
            foreach (var model in list)
            {
                MatchCollection coll = Regex.Matches(model.Text, searchText);
                string[] stringSeparator = new string[] { coll[0].Value };
                var result = model.Text.Split(stringSeparator, StringSplitOptions.None);

                model.FirstText = result[0];
                model.HighLightedText = coll[0].Value;

                result = result.Skip(1).ToArray();
                model.LastText = string.Join("", result);
            }
            return list;
        }
    }
}
