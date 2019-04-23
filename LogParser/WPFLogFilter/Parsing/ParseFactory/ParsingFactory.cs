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
    public class ParsingFactory : IParsingFactory
    {
        public IParsingStrategy Create(string[] lines)
        {
            int counterFull = 0;
            int counterNoThread = 0;
            int counterNoEvent = 0;

            for (int x = 0; x < lines.Length; x++)
            {
                string[] lineParse = lines[x].Split('|');

                switch (lineParse.Length)
                {
                    case 6:
                        counterFull++;
                        break;
                    case 5:

                        if (lineParse[2].Length != 10)
                        {
                            counterNoThread++;
                        }
                        else
                        {
                            counterNoEvent++;
                        }

                        break;
                    default:
                        break;
                }
            }

            if ((counterFull != 0) && (counterFull >= counterNoEvent) && (counterFull >= counterNoThread))
            {
                return new FullParsingStrategy();
            }
            else if ((counterNoEvent > counterFull) && (counterNoEvent >= counterNoThread))
            {
                return new NoEventIdParsingStrategy();
            }
            else if ((counterNoThread > counterFull) && (counterNoThread > counterNoEvent))
            {
                return new NoThreadIdParsingStrategy();
            }
            else
            {
                return new StringOnlyParsingStrategy();
            }
        }
    }
}
