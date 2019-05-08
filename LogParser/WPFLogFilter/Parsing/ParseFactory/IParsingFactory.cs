using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Parsing.ParseStrategy;

namespace WPFLogFilter.Parsing.ParsingFactory
{
    public interface IParsingFactory
    {
        IParsingStrategy Create(string[] lines);
    }
}
