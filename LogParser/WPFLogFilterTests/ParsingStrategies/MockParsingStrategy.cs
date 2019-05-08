﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;
using WPFLogFilter.Parsing.ParseStrategy;

namespace WPFLogFilterTests.ParsingStrategies
{
    public class MockParsingStrategy : IParsingStrategy
    {
        public List<LogModel> Parse(string[] lines)
        {
            return new List<LogModel>
            {
                new LogModel()
            };
        }
    }
}
