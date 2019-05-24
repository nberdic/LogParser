using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WPFLogFilter.AsmblyWrapper;
using WPFLogFilter.DialogWrapperFolder;
using WPFLogFilter.Filter;
using WPFLogFilter.Model;
using WPFLogFilter.Parsing.ParseStrategy;
using WPFLogFilter.Parsing.ParsingFactory;
using WPFLogFilter.Tabs;
using WPFLogFilter.ViewModel;
using WPFLogFilterTests.ParsingStrategies;

namespace WPFLogFilterTests.ViewModelTests
{
    [TestClass]
    public class MainViewModelTests
    {
        Mock<IDialogWrapper> iDialogWrapper;
        Mock<IAssemblyWrapper> iAssemblyWrapper;
        Mock<IParsingFactory> iParsingFactory;
        Mock<IFilterFactory> iFilterFactory;
        Mock<IParsingStrategy> iParsingStrategy;
        Mock<ILog> iLog;
        MainViewModel viewModel;
        Mock<ObservableCollection<IModel>> observableCollection;

        [TestInitialize]

        public void Initialize()
        {
            iDialogWrapper = new Mock<IDialogWrapper>();
            iAssemblyWrapper = new Mock<IAssemblyWrapper>();
            iParsingFactory = new Mock<IParsingFactory>();
            iFilterFactory = new Mock<IFilterFactory>();
            iParsingStrategy = new Mock<IParsingStrategy>();
            iLog = new Mock<ILog>();
            observableCollection = new Mock<ObservableCollection<IModel>>();

            viewModel = new MainViewModel(iDialogWrapper.Object, iAssemblyWrapper.Object, iParsingFactory.Object, iFilterFactory.Object, iLog.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MainViewModelContructor_iDialogWrapperIsNull_NullReferenceExceptionError()
        {
            // Arrange

            // Act
            MainViewModel viewModelNoDialogWrapper = new MainViewModel(null, iAssemblyWrapper.Object, iParsingFactory.Object, iFilterFactory.Object, iLog.Object);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MainViewModelContructor_iAssemblyWrapperIsNull_NullReferenceExceptionError()
        {
            // Arrange

            // Act
            MainViewModel viewModelNoAssemblyWrapper = new MainViewModel(iDialogWrapper.Object, null, iParsingFactory.Object, iFilterFactory.Object, iLog.Object);

            //Assert
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MainViewModelContructor_iParsingStrategyIsNullIsNull_NullReferenceExceptionError()
        {
            // Arrange


            // Act 
            MainViewModel viewModelNoParsingStrategy = new MainViewModel(iDialogWrapper.Object, iAssemblyWrapper.Object, null, iFilterFactory.Object, iLog.Object);

            // Assert

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MainViewModelContructor_iFilterFactoryIsNullIsNull_NullReferenceExceptionError()
        {
            // Arrange


            // Act 
            MainViewModel viewModelNoFilterFactory = new MainViewModel(iDialogWrapper.Object, iAssemblyWrapper.Object, iParsingFactory.Object, null, iLog.Object);

            //Assert

        }

        [TestMethod]
        public void ClickMenuCommand_DialogWrapperReturnsNullList_VisibilityFalse()
        {
            // Arrange
            iDialogWrapper.Setup(x => x.GetPaths()).Returns((List<string>)null);

            // Act
            viewModel.ClickMenuCommand.Execute(null);

            // Assert
            Assert.IsFalse(viewModel.TabVisibility);
        }

        [TestMethod]
        public void ClickMenuCommand_DialogWrapperReturnsEmptyList_VisibilityFalse()
        {
            // Arrange
            iDialogWrapper.Setup(x => x.GetPaths()).Returns(new List<string>());

            // Act
            viewModel.ClickMenuCommand.Execute(null);

            // Assert
            Assert.IsFalse(viewModel.TabVisibility);
        }

        //[TestMethod]
        //public void ClickMenuCommand_DialogWrapperReturnsListWith3Objects_SelectedIndex2_VisibilityTrue()
        //{
        //    // Arrange
        //    iDialogWrapper.Setup(x => x.GetPaths()).Returns(new List<string>()
        //    {
                
        //    });

        //    iParsingFactory.Setup(x => x.Create(It.IsAny<string[]>())).Returns(new MockParsingStrategy());
        //    iParsingStrategy.Setup(x => x.Parse(new string[2])).Returns(new List<LogModel>()
        //    {
        //        new LogModel()
        //    });

        //    // Act
        //    viewModel.ClickMenuCommand.Execute(null);

        //    // Assert
        //    Assert.AreEqual(2, viewModel.TabSelectIndex);
        //    Assert.IsTrue(viewModel.TabVisibility);
        //}

        [TestMethod]
        public void ClickMenuCommand_ParsingFactoryMockStrategyReturnsNull_VisibilityFalse()
        {
            // Arrange
            iDialogWrapper.Setup(x => x.GetPaths()).Returns(new List<string>()
            {
                
            });

            iParsingFactory.Setup(x => x.Create(It.IsAny<string[]>())).Returns((new MockParsingStrategyIsNull()));
            iParsingStrategy.Setup(x => x.Parse(new string[2])).Returns(new List<IModel>()
            {
                new IModel()
            });

            // Act
            viewModel.ClickMenuCommand.Execute(null);

            // Assert
            Assert.IsFalse(viewModel.TabVisibility);
        }

        //[TestMethod]
        //public void ClickMenuCommand_ParsingFactoryMockStrategyReturnsEmpty_VisibilityTrue()
        //{
        //    // Arrange
        //    iDialogWrapper.Setup(x => x.GetPaths()).Returns(new List<string>()
        //    {
                
        //    });

        //    iParsingFactory.Setup(x => x.Create(It.IsAny<string[]>())).Returns(new MockParsingStrategyEmpty());
        //    iParsingStrategy.Setup(x => x.Parse(new string[2])).Returns(new List<LogModel>()
        //    {
        //        new LogModel()
        //    });

        //    // Act
        //    viewModel.ClickMenuCommand.Execute(null);

        //    // Assert
        //    Assert.IsTrue(viewModel.TabVisibility);
        //    //Even though the file is empty, show it nonetheless
        //}

        //[TestMethod]
        //public void CloseTabCommand_ListHas1Tab_ListCountIsZero()
        //{
        //    //Arrange
        //    observableCollection = new Mock<ObservableCollection<LogModel>>();
        //    var tab = new TabViewModel(iParsingFactory.Object, iParsingStrategy.Object, iFilterFactory.Object, iLog.Object, It.IsAny<string>());
        //    viewModel.Tabs.Add(tab);

        //    //Act
        //    viewModel.CloseTabCommand.Execute(tab);

        //    //Assert
        //    Assert.AreEqual(0, viewModel.Tabs.Count);
        //}

        //[TestMethod]
        //public void CloseTabCommand_TabUsedForRemoveIsNull_ListCountIs1()
        //{
        //    //Arrange
        //    var tab = new TabViewModel(iParsingFactory.Object, iParsingStrategy.Object, iFilterFactory.Object, iLog.Object, It.IsAny<string>());
        //    TabViewModel tabSentAsArgument = null;
        //    viewModel.Tabs.Add(tab);

        //    //Act
        //    viewModel.CloseTabCommand.Execute(tabSentAsArgument);

        //    //Assert
        //    Assert.AreEqual(1, viewModel.Tabs.Count);
        //}

    }
}
