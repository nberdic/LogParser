using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLogFilter.Model
{
    /// <summary>
    /// This class is used to store log file path and all text they contain.
    /// </summary>
    public class FileModel
    {
        /// <summary>
        /// This property contains the log file paths.
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// This property contains all of the text that the log contains.
        /// </summary>
        public string[] FileData { get; set; }
    }
}
