using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using WPFLogFilter.Model;

namespace WPFLogFilter.DialogWrapperFolder
{
    public class DialogWrapper : IDialogWrapper
    {
        public List<string> GetPaths()
        {
            List<string> listOfPaths = new List<string>();
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = ".txt";
            openFileDlg.Multiselect = true;
            openFileDlg.Filter = "Text documents (.txt)(.log)|*.txt;*.log";
            if (openFileDlg.ShowDialog() == true)
            {
                foreach (string file in openFileDlg.FileNames)
                {
                    listOfPaths.Add(file);
                }
            }
            return listOfPaths;
        }
    }
}
