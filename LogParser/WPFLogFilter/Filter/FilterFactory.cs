using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLogFilter.Filter
{
    public class FilterFactory : IFilterFactory
    {
        public IFilter Create(int choice)
        {
            switch (choice)
            {
                case 2:
                    return new DateTimeFilter();
                case 3:
                    return new ThreadIdFilter();
                case 4:
                    return new LogLevelFilter();
                case 5:
                    return new EventIdFilter();
                case 6:
                    return new RegexFilter();
                default:
                    return null;
            }
        }
    }
}
