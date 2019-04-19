using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.ParsingFactoryStrategyFolder.ParsingStrategyFolder;

namespace WPFLogFilter.ParsingFactoryStrategyFolder.ParsingFactoryFolder
{
    public interface IParsingFactory
    {
        IParsingStrategy Create(string line);
    }
}
