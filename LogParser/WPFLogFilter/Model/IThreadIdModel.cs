using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLogFilter.Model
{
    public interface IThreadIdModel : IModel
    {
        string ThreadId { get; set; }
    }
}
