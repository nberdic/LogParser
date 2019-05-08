using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFLogFilter.Model;
using WPFLogFilter.Parsing.ParseStrategy;

namespace WPFLogFilterTests.ParsingTests.ParseStrategyTests
{
   
    [TestClass]
    public class NoThreadIdParsingStrategyTest
    {
        List<LogModel> listOfResults;
        string[] inputString;
        IParsingStrategy parsingStrategy;

        [TestInitialize]
        public void Initiliaze()
        {
            listOfResults = new List<LogModel>();
            parsingStrategy = new NoThreadIdParsingStrategy();
        }

        [TestMethod]
        public void NoThreadIdStrategyMakeAListOfObjectsOutOfStrings_RegularString_ListCount1()
        {
            //Arrange
            inputString = new string[] { "2019-01-08|13:20:05.907|INFO |1|---- Telexis.TSU.ShellUI version 1.36.2.32763 ----" };

            //Act
            listOfResults = parsingStrategy.Parse(inputString);

            //Assert
            Assert.AreEqual(1, listOfResults.Count);
        }

        [TestMethod]
        public void NoThreadIdStrategyMakeAListOfObjectsOutOfStrings_IncorrectDateString_ListCount1()
        {
            //Arrange
            inputString = new string[] { "2019--01-08|13:20:05.907|INFO |1|---- Telexis.TSU.ShellUI version 1.36.2.32763 ----" };

            //Act
            listOfResults = parsingStrategy.Parse(inputString);

            //Assert
            Assert.AreEqual(1, listOfResults.Count);

            //Used TryParse Instead of Parse
        }

        [TestMethod]
        public void NoThreadIdStrategyMakeAListOfObjectsOutOfStrings_IncorrectLogLevelString_ListCount1()
        {
            //Arrange
            inputString = new string[] { "2019-01-08|13:20:05.907|/*- |1|---- Telexis.TSU.ShellUI version 1.36.2.32763 ----" };

            //Act
            listOfResults = parsingStrategy.Parse(inputString);

            //Assert
            Assert.AreEqual(1, listOfResults.Count);
        }

        [TestMethod]
        public void NoThreadIdStrategyMakeAListOfObjectsOutOfStrings_IncorrectEventIdString_ListCount1()
        {
            //Arrange
            inputString = new string[] { "2019-01-08|13:20:05.907|INFO |-33|---- Telexis.TSU.ShellUI version 1.36.2.32763 ----" };

            //Act
            listOfResults = parsingStrategy.Parse(inputString);

            //Assert
            Assert.AreEqual(1, listOfResults.Count);
        }

        [TestMethod]
        public void NoThreadIdStrategyMakeAListOfObjectsOutOfStrings_IncorrectTextString_ListCount1()
        {
            //Arrange
            inputString = new string[] { "2019-01-08|13:20:05.907|INFO |1|-*=-0=-0=" };

            //Act
            listOfResults = parsingStrategy.Parse(inputString);

            //Assert
            Assert.AreEqual(1, listOfResults.Count);
        }
    }
}
