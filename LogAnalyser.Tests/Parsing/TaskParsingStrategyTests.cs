using System.Text.RegularExpressions;
using LogAnalyzer.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogAnalyzer.Tests.Parsing
{
    [TestClass]
    public class TaskParsingStrategyTests
    {
        /// <summary>
        ///     The parse task line with task with start and end.
        /// </summary>
        [TestMethod]
        public void ParseTaskLineWithTaskWithStartAndEnd()
        {
            Match match = TaskParsingStrategy.TaskRegex.Match("% DUMMY 12:01 9:01");

            Assert.IsTrue(match.Success);
            Assert.AreEqual(4, match.Groups.Count);
        }

        /// <summary>
        ///     The parse task line with valid date fails.
        /// </summary>
        [TestMethod]
        public void ParseTaskLineWithValidDateFails()
        {
            Match match = TaskParsingStrategy.TaskRegex.Match("% 2014-05-06");

            Assert.IsFalse(match.Success);
        }
    }
}
