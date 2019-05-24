using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFLogFilter.Model;
using System.Collections.ObjectModel;
using WPFLogFilter.Filter;

namespace WPFLogFilterTests.FilterTests
{
    [TestClass]
    public class RegexFilterTests
    {
        ObservableCollection<IModel> listOfLines;
        IFilter filter;
        string search;

        [TestInitialize]
        public void Initiliaze()
        {
            listOfLines = new ObservableCollection<IModel> {
             new IModel { Id = 1, DateTime = new DateTime(2019, 1, 8, 8, 30, 52), ThreadId = "[00000001]", LogLevel = "Info", EventId = 1, Text = "miL__?--**" },
             new IModel { Id = 1, DateTime = new DateTime(2019, 1, 8, 23, 00, 55), ThreadId = "[00000001]", LogLevel = "Info", EventId = 1, Text = "MiLvvO2" },
             new IModel { Id = 1, DateTime = new DateTime(2019, 1, 8, 9, 30, 52), ThreadId = "[00000001]", LogLevel = "Info", EventId = 1, Text = "$$" }};

            filter = new RegexFilter();
        }

        //[TestMethod]
        //public void RegexFilter_DefaultSearchStringNotCaseSensitive_ReturnListWith2Objects()
        //{
        //    //Arrange
        //    search = "m¢";

        //    //Act
        //    listOfLines = filter.Filter(listOfLines,search);

        //    //Assert
        //    Assert.AreEqual(2,listOfLines.Count);
        //}

        [TestMethod]
        public void RegexFilter_LimitedSearchStringCaseSensitive_ReturnListWith1Object()
        {
            //Arrange
            search = "m";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(1, listOfLines.Count);
        }

        [TestMethod]
        public void RegexFilter_EmptySearchString_ReturnListWithAll3Objects()
        {
            //Arrange
            search = "";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(3, listOfLines.Count);
        }

        //[TestMethod]
        //public void RegexFilter_EmptySearchStringWithSymbol_ReturnListWithAll3Objects()
        //{
        //    //Arrange
        //    search = "¢";

        //    //Act
        //    listOfLines = filter.Filter(listOfLines, search);

        //    //Assert
        //    Assert.AreEqual(3, listOfLines.Count);
        //}

        //[TestMethod]
        //public void RegexFilter_RegexStringNotCaseSensitive_ReturnListWith1Object()
        //{
        //    //Arrange
        //    search = "[M]¢";

        //    //Act
        //    listOfLines = filter.Filter(listOfLines, search);

        //    //Assert
        //    Assert.AreEqual(1, listOfLines.Count);
        //}

        [TestMethod]
        public void RegexFilter_RegexStringCaseSensitive_ReturnListWith1Object()
        {
            //Arrange
            search = "[M]";

            //Act
            listOfLines = filter.Filter(listOfLines, search);

            //Assert
            Assert.AreEqual(1, listOfLines.Count);
        }

    }
}
