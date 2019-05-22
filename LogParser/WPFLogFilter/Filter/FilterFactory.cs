using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLogFilter.Filter
{
    /// <summary>
    /// This class is used to create a desired filter.
    /// </summary>
    public class FilterFactory : IFilterFactory
    {
        /// <summary>
        /// This method gives us the corresponding filter depending on the choice we made.
        /// </summary>
        /// <param name="choice">The number of the filter we chose</param>
        /// <returns></returns>
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
                case 7:
                    return new TextFilter();
                default:
                    return null;
            }
        }
    }
}
