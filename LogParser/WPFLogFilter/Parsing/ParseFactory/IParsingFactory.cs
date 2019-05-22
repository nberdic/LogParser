using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Parsing.ParseStrategy;

namespace WPFLogFilter.Parsing.ParsingFactory
{
    /// <summary>
    /// This interface is used for the ParsingFactory class.
    /// </summary>
    public interface IParsingFactory
    {
        /// <summary>
        /// This method is used to help us see what is the best format we need to choose to parse the log file.
        /// </summary>
        /// <param name="lines">List of log strings</param>
        /// <returns></returns>
        IParsingStrategy Create(string[] lines);
    }
}
