using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using WPFLogFilter.Model;

namespace WPFLogFilter.Filter
{
    public class RegexFilter : IFilter
    {
        public ObservableCollection<LogModel> Filter(ObservableCollection<LogModel> list, string searchText)
        {
            int notCaseSensitive = 0;

            if (searchText.Equals(string.Empty))
            {
                return list;
            }

            if (searchText[searchText.Length - 1].Equals('¢'))
            {
                searchText = searchText.Remove(searchText.Length - 1);
                notCaseSensitive = 1;
            }

            if (IsRegexValid(searchText))
            {
                list = new ObservableCollection<LogModel>(list.Where(x => Regex.Match(x.Text, searchText).Success));
            }
            else
            {
                if (notCaseSensitive == 0)
                {
                    list = new ObservableCollection<LogModel>(list.Where(x => x.Text.Contains(searchText)));
                }
                else
                {
                    list = new ObservableCollection<LogModel>(list.Where(x => x.Text.ToLower().Contains(searchText.ToLower())));
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
