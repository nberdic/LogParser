using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;
using WPFLogFilter.Parsing.ParseStrategy;
using WPFLogFilter.ParsingFactoryStrategyFolder.ParsingStrategyFolder;

namespace WPFLogFilter.ParsingFactoryStrategyFolder.ParsingFactoryFolder
{
    public class ParsingFactory: IParsingFactory
    {
        public IParsingStrategy Create(string line)
        {
            string[] lineParse = line.Split('|');

            if (lineParse.Length == 6)
            {
                return new FullParsingStrategy();
            }
            else if ((lineParse.Length == 5))
            {
                if (lineParse[2].Length!=10)
                {
                    return new NoThreadIdParsingStrategy();
                }
                else
                {
                    return new NoEventIdParsingStrategy();
                }
            }
            else
            {
                return new StringOnlyParsingStrategy();
            }
        }
    }
}
