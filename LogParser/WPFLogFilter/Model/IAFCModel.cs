using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLogFilter.Model
{
    public interface IAFCModel
    {
        string HardwareId { get; set; }
        string Service { get; set; }
    }
}
