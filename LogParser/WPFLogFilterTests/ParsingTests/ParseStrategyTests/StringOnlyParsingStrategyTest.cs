using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFLogFilter.Model;
using WPFLogFilter.Parsing.ParseStrategy;

namespace WPFLogFilterTests.ParsingTests.ParseStrategyTests
{
   
    [TestClass]
    public class StringOnlyParsingStrategyTest
    {
        List<IModel> listOfResults;
        string[] inputString;
        IParsingStrategy parsingStrategy;

        [TestInitialize]
        public void Initiliaze()
        {
            listOfResults = new List<IModel>();
            parsingStrategy = new StringOnlyParsingStrategy();
        }

        [TestMethod]
        public void StringOnlyStrategyMakeAListOfObjectsOutOfStrings_RegularString_ListCount1()
        {
            //Arrange
            inputString = new string[] { "2019 - 01 - 08 | 13:19:55.657 |[00000001] | INFO | 1 | ----Telexis.TSU.AutoUpdaterService version 1.36.2.32763----"};

            //Act
            listOfResults = parsingStrategy.Parse(inputString);

            //Assert
            Assert.AreEqual(1, listOfResults.Count);
        }

        [TestMethod]
        public void StringOnlyStrategyMakeAListOfObjectsOutOfStrings_IncorrectTextString_ListCount1()
        {
            //Arrange
            inputString = new string[] { "2019 - 01 - 08 | 13:19:55.657 |[00000001] | INFO | 1 | /REWREWRW~~~~$?}|~~~~[]" };

            //Act
            listOfResults = parsingStrategy.Parse(inputString);

            //Assert
            Assert.AreEqual(1, listOfResults.Count);
        }
    }
}
