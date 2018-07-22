using System.Text.RegularExpressions;
using LogAnalyzer.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogAnalyzer.Tests.Parsing
{
    [TestClass]
    public class DateParsingStrategyTests
    {
        /// <summary>
        ///     The parse date line with invalid year.
        /// </summary>
        [TestMethod]
        public void ParseDateLineWithInvalidYear()
        {
            Match match = DateParsingStrategy.DateStartRegEx.Match("% 14-06-07");

            Assert.IsFalse(match.Success);
        }

        /// <summary>
        ///     The parse date line with missing day.
        /// </summary>
        [TestMethod]
        public void ParseDateLineWithMissingDay()
        {
            Match match = DateParsingStrategy.DateStartRegEx.Match("% 2014-06");

            Assert.IsFalse(match.Success);
        }

        /// <summary>
        ///     The parse date line with task with start and end fails.
        /// </summary>
        [TestMethod]
        public void ParseDateLineWithTaskWithStartAndEndFails()
        {
            Match match = DateParsingStrategy.DateStartRegEx.Match("% DUMMY 12:01 9:01");

            Assert.IsFalse(match.Success);
        }

        /// <summary>
        ///     The parse date line with valid string test.
        /// </summary>
        [TestMethod]
        public void ParseDateLineWithValidStringTest()
        {
            Match match = DateParsingStrategy.DateStartRegEx.Match("% 2014-06-07");

            Assert.IsTrue(match.Success);
            Assert.AreEqual("2014-06-07", match.Groups["date"].Value);
        }
    }
}
