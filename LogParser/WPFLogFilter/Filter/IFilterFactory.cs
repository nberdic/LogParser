using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLogFilter.Filter
{
    public interface IFilterFactory
    {
        IFilter Create(int choice);
    }
}
