using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using WPFLogFilter.Model;

namespace WPFLogFilter.DialogWrapperFolder
{
    public class DialogWrapper : IDialogWrapper
    {
        public List<FileModel> GetLines()
        {
            List<FileModel> tempList = new List<FileModel>();
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = ".txt";
            openFileDlg.Multiselect = true;
            openFileDlg.Filter = "Text documents (.txt)(.log)|*.txt;*.log";
            if (openFileDlg.ShowDialog() == true)
            {
                foreach (string file in openFileDlg.FileNames)
                {
                    tempList.Add(new FileModel { FilePath = file, FileData = File.ReadAllLines(file) });
                }
                return tempList;
            }
            return null;
        }
    }
}
