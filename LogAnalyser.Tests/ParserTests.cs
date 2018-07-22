// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParserTests.cs" company="JCS">
//   copyright
// </copyright>
// <summary>
//   The parser tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using LogAnalyzer.Parsing;

namespace LogAnalyzer.Tests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     The parser tests.
    /// </summary>
    [TestClass]
    public class ParserTests
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The parse.
        /// </summary>
        [TestMethod]
        public void Parse()
        {
        }

        /// <summary>
        ///     The parse task delta line.
        /// </summary>
        [TestMethod]
        public void ParseTaskDeltaLine()
        {
            Match match = TaskDeltaParsingStrategy.TaskDeltaRegex.Match("% DUMMY +0:05");

            Assert.IsTrue(match.Success);
        }

        /// <summary>
        ///     The parse task delta line 2.
        /// </summary>
        [TestMethod]
        public void ParseTaskDeltaLine2()
        {
            Match match = TaskDeltaParsingStrategy.TaskDeltaRegex.Match("% DUMMY -0:05");

            Assert.IsTrue(match.Success);
        }



        /// <summary>
        /// The parse with one day hold date.
        /// </summary>
        [TestMethod]
        public void ParseWithOneDayHoldDate()
        {
            const string s = "% 2014-06-07";
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            using (StreamReader streamReader = new StreamReader(memoryStream))
            using (Parser parser = new Parser(streamReader))
            {
                parser.Parse();

                Assert.AreEqual(0, parser.TaskEntries.Count());
            }
        }

        /// <summary>
        /// The parse with task before any date has error.
        /// </summary>
        [TestMethod]
        public void ParseWithTaskBeforeAnyDateHasError()
        {
            const string s = "% TASKCODE 12:34 13:13";
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            using (StreamReader streamReader = new StreamReader(memoryStream))
            using (Parser parser = new Parser(streamReader))
            {
                parser.Parse();

                Assert.AreEqual(0, parser.TaskEntries.Count());
                Assert.AreEqual(1, parser.Errors.Count());
                Assert.AreEqual(1, parser.Errors.First().LineNumber);
                Assert.AreEqual(s, parser.Errors.First().LineText);
                Assert.AreEqual(ErrorType.NoDateDefined, parser.Errors.First().ErrorType);
            }
        }

        /// <summary>
        /// The parse with task hold time.
        /// </summary>
        [TestMethod]
        public void ParseWithTaskHoldTime()
        {
            const string s = "% 2014-06-07\n" + "% TASKCODE 12:34 13:13";
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            using (StreamReader streamReader = new StreamReader(memoryStream))
            using (Parser parser = new Parser(streamReader))
            {
                parser.Parse();

                Assert.AreEqual(1, parser.TaskEntries.GroupBy(t => t.Date).Count());

                var taskEntry = parser.TaskEntries.First();

                Assert.AreEqual(new TimeSpan(0, 0, 39, 0), taskEntry.Duration);
                Assert.AreEqual("TASKCODE", taskEntry.Task);
            }
        }

        /// <summary>
        /// The parse with task hold time.
        /// </summary>
        [TestMethod]
        public void ParseWithTaskDateTest()
        {
            const string s = "% 2014-06-07\n" + "% TASKCODE 12:34 13:13";
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            using (StreamReader streamReader = new StreamReader(memoryStream))
            using (Parser parser = new Parser(streamReader))
            {
                parser.Parse();

                Assert.AreEqual(new DateTime(2014, 06, 07), parser.TaskEntries.First().Date);
            }
        }

        /// <summary>
        /// The parse with task hold time.
        /// </summary>
        [TestMethod]
        public void ParseWithTaskPositiveDeltaHoldTime()
        {
            const string s = "% 2014-06-07\n" + "% TASKCODE +0:05";
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            using (StreamReader streamReader = new StreamReader(memoryStream))
            using (Parser parser = new Parser(streamReader))
            {
                parser.Parse();

                Assert.AreEqual(1, parser.TaskEntries.GroupBy(t => t.Date).Count());

                var taskEntry = parser.TaskEntries.First();

                Assert.AreEqual(new TimeSpan(0, 0, 5, 0), taskEntry.Duration);
                Assert.AreEqual("TASKCODE", taskEntry.Task);
            }
        }

        /// <summary>
        /// The parse with task hold time.
        /// </summary>
        [TestMethod]
        public void ParseWithTaskNegativeDeltaHoldTime()
        {
            const string s = "% 2014-06-07\n" + "% TASKCODE -0:05";
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            using (StreamReader streamReader = new StreamReader(memoryStream))
            using (Parser parser = new Parser(streamReader))
            {
                parser.Parse();

                Assert.AreEqual(1, parser.TaskEntries.Count());

                var taskEntry = parser.TaskEntries.First();

                Assert.AreEqual(new TimeSpan(0, 0, -5, 0), taskEntry.Duration);
                Assert.AreEqual("TASKCODE", taskEntry.Task);
            }
        }

        /// <summary>
        /// The parse with task hold time.
        /// </summary>
        [TestMethod]
        [DeploymentItem("DataFiles\\log-2014-02.txt", "DataFiles")]
        public void ParseFileTests1()
        {
            using (Parser parser = new Parser("DataFiles\\log-2014-02.txt"))
            {
                parser.Parse();

                Assert.AreEqual(19, parser.TaskEntries.GroupBy(t => t.Date).Count());
                foreach (var error in parser.Errors)
                {
                    Trace.WriteLine(error);
                }
            }
        }

        /// <summary>
        /// The parse with task hold time.
        /// </summary>
        [TestMethod]
        [DeploymentItem("DataFiles\\log-2012-11.txt", "DataFiles")]
        public void ParseFileTests2()
        {
            using (Parser parser = new Parser("DataFiles\\log-2012-11.txt"))
            {
                parser.Parse();

                Assert.AreEqual(14, parser.TaskEntries.GroupBy(t => t.Date).Count());
                foreach (var error in parser.Errors)
                {
                    Trace.WriteLine(error);
                }
            }
        }

        #endregion
    }
}