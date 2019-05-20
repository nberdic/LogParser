using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WPFLogFilter.Model;

namespace WPFLogFilter.Filter
{
    public class TextFilter : IFilter
    {
        public ObservableCollection<LogModel> Filter(ObservableCollection<LogModel> list, string searchText)
        {
            int notCaseSensitive = 0;

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

            if (searchText[searchText.Length - 1].Equals('¢'))
            {
                searchText = searchText.Remove(searchText.Length - 1);
                notCaseSensitive = 1;
            }

            if (notCaseSensitive == 0)
            {
                list = new ObservableCollection<LogModel>(list.Where(x => x.Text.Contains(searchText)));

                foreach (var model in list)
                {
                    string[] stringSeparator = new string[] { searchText };
                    var result = model.Text.Split(stringSeparator, StringSplitOptions.None);

                    model.FirstText = result[0];
                    model.HighLightedText = searchText;

                    result = result.Skip(1).ToArray();
                    model.LastText = string.Join("", result);
                }
            }
            else
            {
                list = new ObservableCollection<LogModel>(list.Where(x => x.Text.ToLower().Contains(searchText.ToLower())));

                foreach (var model in list)
                {
                    string[] stringSeparator = new string[] { searchText };
                    var result = Regex.Split(model.Text, searchText, RegexOptions.IgnoreCase);

                    model.FirstText = result[0];
                    model.HighLightedText = model.Text.Substring(model.FirstText.Length, searchText.Length);

                    result = result.Skip(1).ToArray();
                    model.LastText = string.Join("", result);
                }
            }
            return list;
        }
    }
}
