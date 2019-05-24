using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;

namespace WPFLogFilter.Parsing.ParseStrategy
{
    /// <summary>
    /// This class is used as the last resort for unformattable lines of the logs.
    /// </summary>
    public class StringOnlyParsingStrategy : IParsingStrategy
    {
        /// <summary>
        /// This method is used to copy the entire line to the text column.
        /// </summary>
        /// <param name="lines">List of log lines</param>
        /// <returns></returns>
        public List<IModel> Parse(string[] lines)
        {
            List<IModel> tempList = new List<IModel>();

            for (int x = 0; x < lines.Length; x++)
            {
                tempList.Add(new StringOnlyModel(lines[x]));
            }
            return tempList;
        }
    }
}
