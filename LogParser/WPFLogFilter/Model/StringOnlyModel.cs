using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLogFilter.Model
{
    public class StringOnlyModel : IModel
    {
        public StringOnlyModel(string text)
        {
            TextFull = text;
            TextFirstPart = text;
            IsValid = false;
        }

        public int Id { get; set; }
        public string TextFull { get; set; }
        public string TextFirstPart { get; set; }
        public string TextHighlightedPart { get; set; }
        public string TextSecondPart { get; set; }
        public bool IsValid { get; set; }
    }
}
