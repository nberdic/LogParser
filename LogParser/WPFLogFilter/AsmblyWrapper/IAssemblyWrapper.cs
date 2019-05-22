using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLogFilter.AsmblyWrapper
{
    /// <summary>
    /// Used as an interface for the AssemplyWrapper class which is used to get the current Assembly version.
    /// </summary>
    public interface IAssemblyWrapper
    {
        /// <summary>
        /// This method accesses the Assemlby and extracts the version, so it can be displayed as the application title.
        /// </summary>
        /// <returns>Returns the version number</returns>
        string GetVersion();
    }
}
