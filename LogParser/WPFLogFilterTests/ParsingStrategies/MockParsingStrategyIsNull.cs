using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;
using WPFLogFilter.Parsing.ParseStrategy;

namespace WPFLogFilterTests.ParsingStrategies
{
    public class MockParsingStrategyIsNull : IParsingStrategy
    {
        public List<IModel> Parse(string[] lines)
        {
            return null;
        }
    }
}
