/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:WPFLogFilter"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using WPFLogFilter.AsmblyWrapper;
using WPFLogFilter.DialogWrapperFolder;
using WPFLogFilter.Filter;
using WPFLogFilter.Parsing.ParsingFactory;
//using Microsoft.Practices.ServiceLocation;

namespace WPFLogFilter.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        ///[PreferredConstructor]
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IDialogWrapper, DialogWrapper>();
            SimpleIoc.Default.Register<IAssemblyWrapper, AssemblyWrapper>();
            SimpleIoc.Default.Register<IParsingFactory, ParsingFactory>();
            SimpleIoc.Default.Register<IFilterFactory, FilterFactory>();
            

            SimpleIoc.Default.Register<TabViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                try
                {
                    return ServiceLocator.Current.GetInstance<MainViewModel>("");
                }
                catch (System.Exception)
                {

                    throw;
                }
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}