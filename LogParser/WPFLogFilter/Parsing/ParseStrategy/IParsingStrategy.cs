using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;

namespace WPFLogFilter.Parsing.ParseStrategy
{
    /// <summary>
    /// This interface is used for the ParsingStrategies.
    /// </summary>
    public interface IParsingStrategy
    {
        /// <summary>
        /// This class is used to help us parse the lines of the logs and put them into the correct format.
        /// </summary>
        List<LogModel> Parse(string[] lines);
    }
}
