using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLogFilter.Model
{
    public interface IStandardModel : IModel
    {
        DateTime DateTime { get; set; }
        string LogLevel { get; set; }
    }
}
