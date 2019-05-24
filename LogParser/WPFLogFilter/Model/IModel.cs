using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLogFilter.Model
{
    public interface IModel
    {
        int Id { get; set; }
        string TextFull { get; set; }
        string TextFirstPart { get; set; }
        string TextHighlightedPart { get; set; }
        string TextSecondPart { get; set; }
        bool IsValid { get; set; }
    }
}
