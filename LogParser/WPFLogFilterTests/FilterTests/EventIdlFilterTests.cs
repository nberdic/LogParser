using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFLogFilter.Model;
using System.Collections.ObjectModel;
using WPFLogFilter.Filter;

namespace WPFLogFilterTests.FilterTests
{
    [TestClass]
    public class EventIdFilterTests
    {
        ObservableCollection<LogModel> listOfLines;
        IFilter filter;
        string search;

        [TestInitialize]
        public void Initiliaze()
        {
            listOfLines = new ObservableCollection<LogModel> {
             new LogModel { Id = 1, DateTime = new DateTime(2019, 1, 8, 8, 30, 52), ThreadId = "[00000001]", LogLevel = "Info", EventId = 10, Text = "Something" },
             new LogModel { Id = 1, DateTime = new DateTime(2019, 1, 8, 23, 00, 55), ThreadId = "[00000005]", LogLevel = "SomethingLog", EventId = 7200, Text = "Something2" },
             new LogModel { Id = 1, DateTime = new DateTime(2019, 1, 8, 9, 30, 52), ThreadId = "[000000AA]", LogLevel = "Info3", EventId = 0, Text = "Something3" }};

            filter = new EventIdFilter();
        }

        [TestMethod]
        public void EventIdFilter_DefaultSearchString_ReturnListWithAll3Objects()
        {
            //Arrange
            search = "0";

            //Act
            listOfLines = filter.Filter(listOfLines,search);

            //Assert
            Assert.AreEqual(3,listOfLines.Count);
        }

        [TestMethod]
        public void EventIdFilter_LimitedSearchString_ReturnListWith1Object()
        {
            //Arrange
            search = "7200";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(1, listOfLines.Count);
        }

        [TestMethod]
        public void EventIdFilter_TextInsteadOfNumberSearchString_ReturnListWith0Objects()
        {
            //Arrange
            search = "WARNING";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(0, listOfLines.Count);
        }

        [TestMethod]
        public void EventIdFilter_EmptySearchString_ReturnListWithAll3Objects()
        {
            //Arrange
            search = "";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(3, listOfLines.Count);
        }

        [TestMethod]
        public void EventIdFilter_DefaultStringWithSpace_ReturnListWith1Object()
        {
            //Arrange
            search = " 7200";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(1, listOfLines.Count);
            //Trim is activated
        }
    }
}
