using WPFLogFilter.Parsing.ParseStrategy;

namespace WPFLogFilter.Parsing.ParsingFactory
{
    /// <summary>
    /// This class is used to help us check what is the best format for the parsing of the log file.
    /// </summary>
    public class ParsingFactory : IParsingFactory
    {
        /// <summary>
        /// This method is used to help us see what is the best format we need to choose to parse the log file.
        /// </summary>
        /// <param name="lines">List of log strings</param>
        /// <returns></returns>
        public IParsingStrategy Create(string[] lines)
        {
            int counterFull = 0;
            int counterNoThread = 0;
            int counterNoEvent = 0;
            
            //if the log is empty return the no-format strategy.
            if (lines == null)
            {
                return new StringOnlyParsingStrategy();
            }

            foreach (string line in lines)
            {
                if (line.Length>=10)
                {
                    if (line[10].Equals('|'))
                    {
                        break;
                    }
                    else if (line[10].Equals(','))
                    {
                        return new AFCParsingStrategy();
                    }
                }
            }

            //This method load all the lines from the log and checks if the strings belong to a certain format. 
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

            //The format with the most lines will be proclaimed the default format, and all the rest will be shown with a warning.
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
