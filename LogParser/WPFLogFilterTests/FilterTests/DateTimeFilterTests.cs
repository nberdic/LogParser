using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFLogFilter.Model;
using System.Collections.ObjectModel;
using Moq;
using WPFLogFilter.Filter;

namespace WPFLogFilterTests.FilterTests
{
    [TestClass]
    public class DateTimeFilterTests
    {
        ObservableCollection<LogModel> listOfLines;
        IFilter filter;
        string search;

        [TestInitialize]
        public void Initiliaze()
        {
            listOfLines = new ObservableCollection<LogModel> {
             new LogModel { Id = 1, DateTime = new DateTime(2019, 1, 8, 8, 30, 52), ThreadId = "[00000001]", LogLevel = "Info", EventId = 1, Text = "Something" },
             new LogModel { Id = 1, DateTime = new DateTime(2019, 1, 8, 23, 00, 55), ThreadId = "[00000001]", LogLevel = "Info", EventId = 1, Text = "Something2" },
             new LogModel { Id = 1, DateTime = new DateTime(2019, 1, 8, 9, 30, 52), ThreadId = "[00000001]", LogLevel = "Info", EventId = 1, Text = "Something3" },
              new LogModel { Id = 1, DateTime = DateTime.MinValue, ThreadId = "[00000001]", LogLevel = "Info", EventId = 1, Text = "Something3" }
            };

            filter = new DateTimeFilter();
        }

        [TestMethod]
        public void DateTimeFilter_DefaultSearchString_ReturnListWithAll4Objects()
        {
            //Arrange
            search = "00:00:00¥23:59:59¢";

            //Act
            listOfLines = filter.Filter(listOfLines,search);

            //Assert
            Assert.AreEqual(4,listOfLines.Count);
        }

        [TestMethod]
        public void DateTimeFilter_DefaultSearchStringWithSeparatorSymbol_ReturnListWith3Objects()
        {
            //Arrange
            search = "00:00:00¥23:59:59";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(3, listOfLines.Count);
        }

        [TestMethod]
        public void DateTimeFilter_EmptySearchString_ReturnListWithAll4Objects()
        {
            //Arrange
            search = "";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(4, listOfLines.Count);
        }

        [TestMethod]
        public void DateTimeFilter_EmptySearchStringWithShowEmptyDatesSymbol_ReturnListWithAll4Objects()
        {
            //Arrange
            search = "¢";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(4, listOfLines.Count);
        }

        [TestMethod]
        public void DateTimeFilter_EmptySearchWithSeparatorString_ReturnListWithAll4Objects()
        {
            //Arrange
            search = "¥";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(4, listOfLines.Count);
        }

        [TestMethod]
        public void DateTimeFilter_LimitedSearchString_ReturnListWith2Objects()
        {
            //Arrange
            search = "00:00:00¥13:59:59";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(2, listOfLines.Count);
        }

        [TestMethod]
        public void DateTimeFilter_LimitedSearchStringWithShowAllDatesSymbol_ReturnListWith3Objects()
        {
            //Arrange
            search = "00:00:00¥13:59:59¢";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(3, listOfLines.Count);
        }

        [TestMethod]
        public void DateTimeFilter_ReversedTimeString_ReturnListWith0Objects()
        {
            //Arrange
            search = "13:59:59¥00:00:00";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(0, listOfLines.Count);
        }

        [TestMethod]
        public void DateTimeFilter_SameTimeWindowsString_ReturnListWith1Object()
        {
            //Arrange
            search = "08:30:52¥08:30:52";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(1, listOfLines.Count);
        }

        [TestMethod]
        public void DateTimeFilter_NonValidTimeString_ReturnListWithAll4Objects()
        {
            //Arrange
            search = "0-:00:00¥23:?9:59";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(4, listOfLines.Count);
            //Don't filter if the search time isn't valid, just return list with all results
        }

        [TestMethod]
        public void DateTimeFilter_SpaceString_ReturnListWithAll4Objects()
        {
            //Arrange
            search = "  ";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(4, listOfLines.Count);
        }

        [TestMethod]
        public void DateTimeFilter_SpaceStringWithShowAllDatesSymbol_ReturnListWithAll4Objects()
        {
            //Arrange
            search = "  ¢";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(4, listOfLines.Count);
        }

        [TestMethod]
        public void DateTimeFilter_DefaultSearchStringWithSpaceAndShowAllDatesSymbol_ReturnListWithAll4Objects()
        {
            //Arrange
            search = "  00:00:00¥23:59:59¢";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(4, listOfLines.Count);
        }
    }
}
