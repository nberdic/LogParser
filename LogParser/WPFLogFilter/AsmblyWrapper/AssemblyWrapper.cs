
namespace WPFLogFilter.AsmblyWrapper
{
    public class AssemblyWrapper : IAssemblyWrapper
    {
        public string GetVersion()
        {
            return System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
        }
    }
}
