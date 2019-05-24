using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLogFilter.Model;

namespace WPFLogFilter.Filter
{
    /// <summary>
    /// This is the interface for the filter classes.  
    /// </summary>
    public interface IFilter<T> where T : IModel
    {
        /// <summary>
        /// This method is used to filter the log list by multiple criteria.
        /// </summary>
        /// <param name="list">List of log objects</param>
        /// <param name="search">Search criteria</param>
        /// <returns></returns>
        IEnumerable<T> Filter(IEnumerable<T> list, string search);
    }
}
