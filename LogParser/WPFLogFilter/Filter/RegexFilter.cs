using System;
using System.Collections.Generic;
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
    public class RegexFilter : IFilter<IModel>
    {
        /// <summary>
        /// This method filters the text using the Regular expression
        /// <param name="list">List of Log objects</param>
        /// <param name="searchText">Search criteria</param>
        /// <returns></returns>
        public IEnumerable<IModel> Filter(IEnumerable<IModel> list, string searchText)
        {
            //If the search field is empty, reset to default
            if (string.IsNullOrWhiteSpace(searchText))
            {
                foreach (var model in list)
                {
                    model.TextFirstPart = model.TextFull;
                    model.TextHighlightedPart = string.Empty;
                    model.TextSecondPart = string.Empty;
                }
                return list;
            }

            //Use regex and try to filter text, if there is an error, don't filter, and return the list and reset to default.
            try
            {
                list = list.Where(x => Regex.Match(x.TextFull, searchText).Success);
            }
            catch (Exception)
            {
                foreach (var model in list)
                {
                    model.TextFirstPart = model.TextFull;
                    model.TextHighlightedPart = string.Empty;
                    model.TextSecondPart = string.Empty;
                }
                return list;
            }

            //highlight the result and not the regex search string because regex could be ([A-Z])\w+.
            foreach (var model in list)
            {
                MatchCollection coll = Regex.Matches(model.TextFull, searchText);
                string[] stringSeparator = new string[] { coll[0].Value };
                var result = model.TextFull.Split(stringSeparator, StringSplitOptions.None);

                model.TextFirstPart = result[0];
                model.TextHighlightedPart = coll[0].Value;

                result = result.Skip(1).ToArray();
                model.TextSecondPart = string.Join("", result);
            }
            return list;
        }
    }
}
