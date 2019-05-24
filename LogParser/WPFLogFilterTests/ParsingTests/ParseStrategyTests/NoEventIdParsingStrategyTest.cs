using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFLogFilter.Model;
using WPFLogFilter.Parsing.ParseStrategy;

namespace WPFLogFilterTests.ParsingTests.ParseStrategyTests
{
   
    [TestClass]
    public class NoEventIdParsingStrategyTest
    {
        List<IModel> listOfResults;
        string[] inputString;
        IParsingStrategy parsingStrategy;

        [TestInitialize]
        public void Initiliaze()
        {
            listOfResults = new List<IModel>();
            parsingStrategy = new NoEventIdParsingStrategy();
        }

        [TestMethod]
        public void NoEventIdStrategyMakeAListOfObjectsOutOfStrings_RegularString_ListCount1()
        {
            //Arrange
            inputString = new string[] { "2019-03-11|13:37:17.132|[00000006]|INFO |Initializing Threadpool: Compass thread pool" };

            //Act
            listOfResults = parsingStrategy.Parse(inputString);

            //Assert
            Assert.AreEqual(1, listOfResults.Count);
        }

        [TestMethod]
        public void NoEventIdStrategyMakeAListOfObjectsOutOfStrings_IncorrectDateString_ListCount1()
        {
            //Arrange
            inputString = new string[] { "??|13:??:17.132|[00000006]|INFO |Initializing Threadpool: Compass thread pool" };

            //Act
            listOfResults = parsingStrategy.Parse(inputString);

            //Assert
            Assert.AreEqual(1, listOfResults.Count);

            //Used TryParse Instead of Parse
        }

        [TestMethod]
        public void NoEventIdStrategyMakeAListOfObjectsOutOfStrings_IncorrectThreadIdString_ListCount1()
        {
            //Arrange
            inputString = new string[] { "2019-03-11|13:37:17.132|-?2]|INFO |Initializing Threadpool: Compass thread pool" };

            //Act
            listOfResults = parsingStrategy.Parse(inputString);

            //Assert
            Assert.AreEqual(1, listOfResults.Count);
        }

        [TestMethod]
        public void NoEventIdStrategyMakeAListOfObjectsOutOfStrings_IncorrectLogLevelString_ListCount1()
        {
            //Arrange
            inputString = new string[] { "2019-03-11|13:37:17.132|[00000006]|-?32323 |Initializing Threadpool: Compass thread pool" };

            //Act
            listOfResults = parsingStrategy.Parse(inputString);

            //Assert
            Assert.AreEqual(1, listOfResults.Count);
        }

        [TestMethod]
        public void NoEventIdStrategyMakeAListOfObjectsOutOfStrings_IncorrectTextString_ListCount1()
        {
            //Arrange
            inputString = new string[] { "2019-03-11|13:37:17.132|[00000006]|INFO |?}RER#~---" };

            //Act
            listOfResults = parsingStrategy.Parse(inputString);

            //Assert
            Assert.AreEqual(1, listOfResults.Count);
        }
    }
}
