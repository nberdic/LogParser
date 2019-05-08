using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;

namespace WPFLogFilter.Parsing.ParseStrategy
{
    public class StringOnlyParsingStrategy : IParsingStrategy
    {
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
