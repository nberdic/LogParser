using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;

namespace WPFLogFilter.ParsingFactoryStrategyFolder.ParsingStrategyFolder
{
    public interface IParsingStrategy
    {
        List<LogModel> Parse(string[] lines);
    }
}
