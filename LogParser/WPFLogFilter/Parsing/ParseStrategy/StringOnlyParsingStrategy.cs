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
        /// This method is used to copy the entire line to the text column and give the LogModel object default values.
        /// </summary>
        /// <param name="lines">List of log lines</param>
        /// <returns></returns>
        public List<LogModel> Parse(string[] lines)
        {
            List<LogModel> tempList = new List<LogModel>();

            for (int x = 0; x < lines.Length; x++)
            {
                tempList.Add(new LogModel(x + 1, DateTime.MinValue, "", "", -1, lines[x]));
            }
            return tempList;
        }
    }
}
