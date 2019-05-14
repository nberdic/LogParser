using CommonServiceLocator;
using log4net;
using System.Windows;

namespace WPFLogFilter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var logger = ServiceLocator.Current.GetInstance<ILog>();
            string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception);
            logger.Error(errorMessage);
            e.Handled = true;
        }
    }
}
