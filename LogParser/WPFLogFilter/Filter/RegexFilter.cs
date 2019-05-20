using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WPFLogFilter.Model;

namespace WPFLogFilter.Filter
{
    public class RegexFilter : IFilter
    {
        public ObservableCollection<LogModel> Filter(ObservableCollection<LogModel> list, string searchText)
        {
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

            try
            {
                list = new ObservableCollection<LogModel>(list.Where(x => Regex.Match(x.Text, searchText).Success));
            }
            catch (Exception)
            {
                return list;
            }

            //highlight the result and not the regex search string
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
