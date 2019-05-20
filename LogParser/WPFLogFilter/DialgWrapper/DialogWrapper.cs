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
                    File.SetAttributes(file, FileAttributes.Normal);

                    using (FileStream logFileStream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader logFileReader = new StreamReader(logFileStream))
                        {
                            List<string> listOfStrings = new List<string>();
                          
                            while (!logFileReader.EndOfStream)
                            {
                                listOfStrings.Add(logFileReader.ReadLine());
                            }

                            tempList.Add(new FileModel { FilePath = file, FileData = listOfStrings.ToArray() });
                        }
                    }
                }
            }
            return tempList;
        }
    }
}
