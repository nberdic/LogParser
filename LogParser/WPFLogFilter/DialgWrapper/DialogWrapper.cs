using Microsoft.Win32;
using System.Collections;
using System.IO;

namespace WPFLogFilter.DialogWrapperFolder
{
    public class DialogWrapper : IDialogWrapper
    {
        public string[] GetLines(ref string fileName)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = ".txt";
            openFileDlg.Multiselect = true;
            openFileDlg.Filter = "Text documents (.txt)(.log)|*.txt;*.log";
            if (openFileDlg.ShowDialog() == true)
            {
                fileName = openFileDlg.FileName;
                return File.ReadAllLines(openFileDlg.FileName); 
            }
            

            return null;
        }
    }
}
