using System.Collections.Generic;
using WPFLogFilter.Model;

namespace WPFLogFilter.DialogWrapperFolder
{
    /// <summary>
    /// Used as an interface for the DialogWrapper class which is used to get the list of file paths from the dialog menu.
    /// </summary>
    public interface IDialogWrapper
    {
        /// <summary>
        /// This method is used to get the list of file paths from the dialog menu.
        /// </summary>
        /// <returns>List of log file paths</returns>
        List<string> GetPaths();
    }
}
