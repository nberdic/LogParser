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
    /// <summary>
    /// This class is used to filter the text column.
    /// </summary>
    public class TextFilter : IFilter<IModel>
    {
        /// <summary>
        /// This method filters the text column, with the search textbox criteria.
        /// </summary>
        /// <param name="list">List of log objects</param>
        /// <param name="searchText">Textbox search criteria</param>
        /// <returns></returns>
        public IEnumerable<IModel> Filter(IEnumerable<IModel> list, string searchText)
        {
            //The method splits the text into 3 categories, so the 2nd or the middle text can be highlighted.
            bool CaseSensitive = true;
            
            // If the search field is empty, reset to default
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

            //The ¢ symbol at the end of the searchText indicates that the case sensitivity is ON.
            if (searchText[searchText.Length - 1].Equals('¢'))
            {
                searchText = searchText.Remove(searchText.Length - 1);
                CaseSensitive = false;
            }

            //This is the filtering with the case sensitivity on, also it puts the text that needs to be highlighted into the HighLightedText.
            if (CaseSensitive)
            {
                list = list.Where(x => x.TextFull.Contains(searchText));

                foreach (var model in list)
                {
                    string[] stringSeparator = new string[] { searchText };
                    var result = model.TextFull.Split(stringSeparator, StringSplitOptions.None);

                    model.TextFirstPart = result[0];
                    model.TextHighlightedPart = searchText;

                    result = result.Skip(1).ToArray();
                    model.TextSecondPart = string.Join("", result);
                }
            }

            //This is the filtering with the case sensitivity off, also it puts the text that needs to be highlighted into the HighLightedText.
            else
            {
                list = list.Where(x => x.TextFull.ToLower().Contains(searchText.ToLower()));

                foreach (var model in list)
                {
                    string[] stringSeparator = new string[] { searchText };
                    var result = Regex.Split(model.TextFull, searchText, RegexOptions.IgnoreCase);

                    model.TextFirstPart = result[0];
                    model.TextHighlightedPart = model.TextFull.Substring(model.TextFirstPart.Length, searchText.Length);

                    result = result.Skip(1).ToArray();
                    model.TextSecondPart = string.Join("", result);
                }
            }
            return list;
        }
    }
}
