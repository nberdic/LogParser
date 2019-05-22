using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLogFilter.Filter
{
    /// <summary>
    /// This interface is used by the FilterFactory class.
    /// </summary>
    public interface IFilterFactory
    {
        /// <summary>
        /// This method lets us chose which filter we need based on the choice parameter
        /// </summary>
        /// <param name="choice">Choose one of the possible filters</param>
        /// <returns></returns>
        IFilter Create(int choice);
    }
}
