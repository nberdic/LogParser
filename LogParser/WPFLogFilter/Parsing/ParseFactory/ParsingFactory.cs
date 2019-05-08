using WPFLogFilter.Parsing.ParseStrategy;

namespace WPFLogFilter.Parsing.ParsingFactory
{
    public class ParsingFactory : IParsingFactory
    {
        public IParsingStrategy Create(string[] lines)
        {
            int counterFull = 0;
            int counterNoThread = 0;
            int counterNoEvent = 0;

            if (lines == null)
            {
                return new StringOnlyParsingStrategy();
            }

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
