using System.Collections;


namespace WPFLogFilter.DialogWrapperFolder
{
    public interface IDialogWrapper
    {
        string[] GetLines(ref string fileName);
    }
}
