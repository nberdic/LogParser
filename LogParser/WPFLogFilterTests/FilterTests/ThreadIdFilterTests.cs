using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFLogFilter.Model;
using System.Collections.ObjectModel;
using WPFLogFilter.Filter;

namespace WPFLogFilterTests.FilterTests
{
    [TestClass]
    public class ThreadIdFilterTests
    {
        ObservableCollection<IModel> listOfLines;
        IFilter filter;
        string search;

        [TestInitialize]
        public void Initiliaze()
        {
            listOfLines = new ObservableCollection<IModel> {
             new IModel { Id = 1, DateTime = new DateTime(2019, 1, 8, 8, 30, 52), ThreadId = "[00000001]", LogLevel = "Info", EventId = 1, Text = "Something" },
             new IModel { Id = 1, DateTime = new DateTime(2019, 1, 8, 23, 00, 55), ThreadId = "[00000005]", LogLevel = "Info", EventId = 1, Text = "Something2" },
             new IModel { Id = 1, DateTime = new DateTime(2019, 1, 8, 9, 30, 52), ThreadId = "[000000AA]", LogLevel = "Info", EventId = 1, Text = "Something3" }};

            filter = new ThreadIdFilter();
        }

        [TestMethod]
        public void ThreadIdFilter_DefaultSearchString_ReturnListWithAll3Objects()
        {
            //Arrange
            search = "000000";

            //Act
            listOfLines = filter.Filter(listOfLines,search);

            //Assert
            Assert.AreEqual(3,listOfLines.Count);
        }

        [TestMethod]
        public void ThreadIdFilter_EmptySearchString_ReturnListWithAll3Objects()
        {
            //Arrange
            search = "";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(3, listOfLines.Count);
        }

        [TestMethod]
        public void ThreadIdFilter_LimitedSearchString_ReturnListWith1Object()
        {
            //Arrange
            search = "000000AA";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(1, listOfLines.Count);
        }

        [TestMethod]
        public void ThreadIdFilter_LimitedSearchStringWithSpace_ReturnListWith1Object()
        {
            //Arrange
            search = "  000000AA";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(1, listOfLines.Count);
        }

    }
}
