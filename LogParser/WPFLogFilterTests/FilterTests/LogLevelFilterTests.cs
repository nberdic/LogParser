using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFLogFilter.Model;
using System.Collections.ObjectModel;
using WPFLogFilter.Filter;

namespace WPFLogFilterTests.FilterTests
{
    [TestClass]
    public class LogLevelFilterTests
    {
        ObservableCollection<LogModel> listOfLines;
        IFilter filter;
        string search;

        [TestInitialize]
        public void Initiliaze()
        {
            listOfLines = new ObservableCollection<LogModel> {
             new LogModel { Id = 1, DateTime = new DateTime(2019, 1, 8, 8, 30, 52), ThreadId = "[00000001]", LogLevel = "Info", EventId = 1, Text = "Something" },
             new LogModel { Id = 1, DateTime = new DateTime(2019, 1, 8, 23, 00, 55), ThreadId = "[00000005]", LogLevel = "SomethingLog", EventId = 1, Text = "Something2" },
             new LogModel { Id = 1, DateTime = new DateTime(2019, 1, 8, 9, 30, 52), ThreadId = "[000000AA]", LogLevel = "Info3", EventId = 1, Text = "Something3" }};

            filter = new LogLevelFilter();
        }

        [TestMethod]
        public void LogLevelFilter_DefaultSearchString_ReturnListWithAll3Objects()
        {
            //Arrange
            search = "ALL";

            //Act
            listOfLines = filter.Filter(listOfLines,search);

            //Assert
            Assert.AreEqual(3,listOfLines.Count);
        }

        [TestMethod]
        public void LogLevelFilter_LimitedSearchString_ReturnListWith2Objects()
        {
            //Arrange
            search = "Info";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(2, listOfLines.Count);
        }

        [TestMethod]
        public void LogLevelFilter_EmptySearchString_ReturnListWithAll3Objects()
        {
            //Arrange
            search = "";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(3, listOfLines.Count);
        }

        [TestMethod]
        public void LogLevelFilter_LimitedSearchStringWithSpace_ReturnListWith2Objects()
        {
            //Arrange
            search = "  Info";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(2, listOfLines.Count);
        }
    }
}
