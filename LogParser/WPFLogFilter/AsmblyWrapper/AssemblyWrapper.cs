
namespace WPFLogFilter.AsmblyWrapper
{
    /// <summary>
    /// This class is used to get the Assemply version.
    /// </summary>
    public class AssemblyWrapper : IAssemblyWrapper
    {
        /// <summary>
        /// This method accesses the Assemlby and extracts the version, so it can be displayed as the application title.
        /// </summary>
        /// <returns>Returns the version number</returns>
        public string GetVersion()
        {
            return System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
        }
    }
}
