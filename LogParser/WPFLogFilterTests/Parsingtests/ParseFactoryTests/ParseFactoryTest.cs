using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFLogFilter.Parsing.ParseStrategy;
using WPFLogFilter.Parsing.ParsingFactory;

namespace WPFLogFilterTests.ParsingTests.ParseFactoryTests
{
    [TestClass]
    public class ParseFactoryTest
    {
        IParsingStrategy parsingStrategy;
        IParsingFactory parsingFactory;

        [TestInitialize]

        public void Initialize()
        {
            parsingFactory = new ParsingFactory();
        }

        [TestMethod]
        public void ParsingFactoryCorrectTypeBasedOnString_FullParsingStrategyString_FullParsingType()
        {
            //Arrange
            string[] fullParsingStrategyLine = new string[] { "2019 - 01 - 08 | 13:19:55.657 |[00000001] | INFO | 1 | ----Telexis.TSU.AutoUpdaterService version 1.36.2.32763----" };

            //Act
            parsingStrategy = parsingFactory.Create(fullParsingStrategyLine);

            //Assert
            Assert.AreEqual(typeof(AFCParsingStrategy), parsingStrategy.GetType());
        }

        [TestMethod]
        public void ParsingFactoryCorrectTypeBasedOnString_NoEventIdParsingStrategyString_NoEventIdParsingType()
        {
            //Arrange
            string[] noEventIdStrategyLine = new string[] { "2019-03-11|13:37:17.132|[00000006]|INFO |Initializing Threadpool: Compass thread pool" };

            //Act
            parsingStrategy = parsingFactory.Create(noEventIdStrategyLine);

            //Assert
            Assert.AreEqual(typeof(NoEventIdParsingStrategy), parsingStrategy.GetType());
        }

        [TestMethod]
        public void ParsingFactoryCorrectTypeBasedOnString_NoThreadIdParsingStrategyString_NoThreadIdParsingType()
        {
            //Arrange
            string[] noThreadIdStrategyLine = new string[] { "2019-01-08|13:20:05.907|INFO |1|---- Telexis.TSU.ShellUI version 1.36.2.32763 ----" };

            //Act
            parsingStrategy = parsingFactory.Create(noThreadIdStrategyLine);

            //Assert
            Assert.AreEqual(typeof(NoThreadIdParsingStrategy), parsingStrategy.GetType());
        }

        [TestMethod]
        public void ParsingFactoryCorrectTypeBasedOnString_StringOnlyParsingStrategyString_StringOnlyParsingType()
        {
            //Arrange
            string[] stringOnlyStrategyLine = new string[] { "random stuff" };

            //Act
            parsingStrategy = parsingFactory.Create(stringOnlyStrategyLine);

            //Assert
            Assert.AreEqual(typeof(StringOnlyParsingStrategy), parsingStrategy.GetType());
        }

        [TestMethod]
        public void ParsingFactoryCorrectTypeBasedOnString_EmptyString_StringOnlyParsingType()
        {
            //Arrange
            string[] emptyStringLine = new string[] { "" };

            //Act
            parsingStrategy = parsingFactory.Create(emptyStringLine);

            //Assert
            Assert.AreEqual(typeof(StringOnlyParsingStrategy), parsingStrategy.GetType());
        }

        [TestMethod]
        public void ParsingFactoryCorrectTypeBasedOnString_NullString_StringOnlyParsingTypeNull()
        {
            //Arrange
            string[] nullLine = null;

            //Act
            parsingStrategy = parsingFactory.Create(nullLine);

            //Assert
            Assert.AreEqual(typeof(StringOnlyParsingStrategy), parsingStrategy.GetType());
        }
    }
}
