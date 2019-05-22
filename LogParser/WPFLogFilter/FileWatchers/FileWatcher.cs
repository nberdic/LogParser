using System;
using System.IO;


namespace WPFLogFilter.FileWatchers
{
    /// <summary>
    /// Class used to watch for file changes and trigger events.
    /// </summary>
    public class FileWatcher : IDisposable
    {
        private FileSystemWatcher _fileWatcher;
        private bool isDisposed = false;

        /// <summary>
        /// Callback action which will be invoked when opened file is modified.
        /// </summary>
        public Action<string> OnFileModified { get; set; }

        /// <summary>
        /// Constructor used to instantiate the FileSystemWatcher.
        /// </summary>
        public FileWatcher()
        {
            _fileWatcher = new FileSystemWatcher();
        }

        /// <summary>
        /// Method used to dispose of the FileSystemWatcher when it's no longer needed.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// This method watches for changes in the file and triggers the events.
        /// </summary>
        /// <param name="filePath"></param>
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

        /// <summary>
        /// This method checks if the FileSystemWatcher was disposed, if not and the criteria is met, it disposes of it.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    if (_fileWatcher != null)
                    {
                        _fileWatcher.Dispose();
                    }
                }
                isDisposed = true;
            }
        }

        private void ChangeEvent(object sender, FileSystemEventArgs e)
        {
            OnFileModified?.Invoke(e.FullPath);
        }

    }
}
