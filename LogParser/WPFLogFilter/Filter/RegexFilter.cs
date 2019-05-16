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

            // searchText = searchText.Trim();

            if (searchText[searchText.Length - 1].Equals('¢'))
            {
                searchText = searchText.Remove(searchText.Length - 1);
                notCaseSensitive = 1;
            }

            if (IsRegexValid(searchText))
            {
                list = new ObservableCollection<LogModel>(list.Where(x => Regex.Match(x.Text, searchText).Success));

                foreach (var model in list)
                {
                    string[] stringSeparator = new string[] { searchText };
                    var result = model.Text.Split(stringSeparator, StringSplitOptions.None);

                    model.FirstText = result[0];
                    model.HighLightedText = searchText;

                    StringBuilder sb = new StringBuilder();
                    for (int x = 1; x < result.Count(); x++)
                    {
                        sb.Append(result[x]);
                    }
                    model.LastText = sb.ToString();
                }
            }
            else
            {
                if (notCaseSensitive == 0)
                {
                    list = new ObservableCollection<LogModel>(list.Where(x => x.Text.Contains(searchText)));

                    foreach (var model in list)
                    {
                        string[] stringSeparator = new string[] { searchText };
                        var result = model.Text.Split(stringSeparator, StringSplitOptions.None);

                        model.FirstText = result[0];
                        model.HighLightedText = searchText;

                        StringBuilder sb = new StringBuilder();
                        for (int x = 1; x < result.Count(); x++)
                        {
                            sb.Append(result[x]);
                        }
                        model.LastText = sb.ToString();
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
                        model.HighLightedText = model.Text.Substring(model.FirstText.Length,searchText.Length);

                        StringBuilder sb = new StringBuilder();
                        for (int x = 1; x < result.Count(); x++)
                        {
                            sb.Append(result[x]);
                        }
                        model.LastText = sb.ToString();
                    }
                }
            }
            return list;
        }

        private static bool IsRegexValid(string pattern)
        {
            if ((pattern != null) && (pattern.Trim().Length > 0))
            {
                try
                {
                    Regex.Match("", pattern);
                }
                catch (ArgumentException)
                {
                    // BAD PATTERN: Syntax error
                    return false;
                }

                if (pattern.All(ch => Char.IsLetterOrDigit(ch)))
                {
                    return false;
                }

                if (((!Char.IsLetterOrDigit(pattern[0])) && (!Char.IsLetterOrDigit(pattern[pattern.Length - 1]))))

                {
                    if (pattern.Any(ch => Char.IsLetterOrDigit(ch)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if ((pattern.Equals(pattern.Trim())) && (pattern.StartsWith(".")))

                {
                    return false;
                }

                if (pattern.StartsWith(".") || pattern.EndsWith("."))
                {
                    return false;
                }
            }
            else
            {
                //BAD PATTERN: Pattern is null or blank
                return false;
            }
            return true;
        }

    }
}
