using System;
using System.IO;


namespace WPFLogFilter.Observables
{
    /// <summary>
    /// 
    /// </summary>
    public class FileWatcher : IDisposable
    {
        private FileSystemWatcher _fileWatcher;
        private bool isDisposed = false;

        /// <summary>
        /// Callback action which will be invoked when opened file is modified.
        /// </summary>
        public Action<string> OnFileModified { get; set; }

        public FileWatcher()
        {
            _fileWatcher = new FileSystemWatcher();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    if (_fileWatcher !=null)
                    {
                        _fileWatcher.Dispose();
                    }
                }
                isDisposed = true;
            }
        }

        public void Watch(string filePath)
        {
            if (_fileWatcher!=null)
            {
                _fileWatcher.Path = Path.GetDirectoryName(filePath);
                _fileWatcher.Filter = Path.GetFileName(filePath);
                _fileWatcher.NotifyFilter = NotifyFilters.Size | NotifyFilters.LastWrite;
                _fileWatcher.Changed += new FileSystemEventHandler(ChangeEvent);
                _fileWatcher.EnableRaisingEvents = true;
            }
        }

        private void ChangeEvent(object sender, FileSystemEventArgs e)
        {
            OnFileModified?.Invoke(e.FullPath);
        }

    }
}
