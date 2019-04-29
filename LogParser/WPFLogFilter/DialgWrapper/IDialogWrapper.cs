using System.Collections.Generic;
using WPFLogFilter.Model;

namespace WPFLogFilter.DialogWrapperFolder
{
    public interface IDialogWrapper
    {
        List<FileModel> GetLines();
    }
}
