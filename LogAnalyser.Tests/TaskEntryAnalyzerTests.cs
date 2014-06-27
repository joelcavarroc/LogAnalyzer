// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskEntryAnalyzerTests.cs" company="">
//   
// </copyright>
// <summary>
//   The task entry Analyzer tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LogAnalyzer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     The task entry Analyzer tests.
    /// </summary>
    [TestClass]
    public sealed class TaskEntryAnalyzerTests
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The Analyze by day test.
        /// </summary>
        [TestMethod]
        public void AnalyzeByDayTest()
        {
            TaskEntryAnalyzer taskEntryAnalyzer =
                new TaskEntryAnalyzer(
                    new[]
                        {
                            new TaskStartEndEntry(
                                new DateTime(2014, 6, 7),
                                "taskcode",
                                new DateTime(2000, 1, 1, 10, 0, 0, 0),
                                new DateTime(2000, 1, 1, 11, 0, 0, 0))
                        });

            List<WorkDay> workDays = taskEntryAnalyzer.AnalyzeByDay();

            Assert.AreEqual(1, workDays.Count);
            Assert.AreEqual(new TimeSpan(0, 1, 0, 0), workDays[0].TotalDuration);
        }

        /// <summary>
        ///     The Analyze by day test.
        /// </summary>
        [TestMethod]
        public void AnalyzeByDayTest2()
        {
            TaskEntryAnalyzer taskEntryAnalyzer =
                new TaskEntryAnalyzer(
                    new[]
                        {
                            new TaskStartEndEntry(
                                new DateTime(2014, 6, 7),
                                "taskcode",
                                new DateTime(2000, 1, 1, 10, 0, 0, 0),
                                new DateTime(2000, 1, 1, 11, 0, 0, 0)),
                            new TaskStartEndEntry(
                                new DateTime(2014, 6, 7),
                                "taskcode",
                                new DateTime(2000, 1, 1, 11, 0, 0, 0),
                                new DateTime(2000, 1, 1, 12, 30, 0, 0))
                        });

            List<WorkDay> workDays = taskEntryAnalyzer.AnalyzeByDay();

            Assert.AreEqual(1, workDays.Count);
            Assert.AreEqual(new TimeSpan(0, 2, 30, 0), workDays[0].TotalDuration);
        }

        /// <summary>
        ///     The Analyze by day test.
        /// </summary>
        [TestMethod]
        public void AnalyzeByDayTest3()
        {
            TaskEntryAnalyzer taskEntryAnalyzer =
                new TaskEntryAnalyzer(
                    new[]
                        {
                            new TaskStartEndEntry(
                                new DateTime(2014, 6, 7),
                                "taskcode",
                                new DateTime(2000, 1, 1, 10, 0, 0, 0),
                                new DateTime(2000, 1, 1, 11, 0, 0, 0)),
                            new TaskStartEndEntry(
                                new DateTime(2014, 6, 7),
                                "taskcode",
                                new DateTime(2000, 1, 1, 11, 0, 0, 0),
                                new DateTime(2000, 1, 1, 12, 30, 0, 0)),
                            new TaskStartEndEntry(
                                new DateTime(2014, 6, 8),
                                "taskcode",
                                new DateTime(2000, 1, 1, 11, 0, 0, 0),
                                new DateTime(2000, 1, 1, 12, 30, 0, 0))
                        });

            WorkDay[] workDays = taskEntryAnalyzer.AnalyzeByDay().OrderBy(wd => wd.Date).ToArray();

            Assert.AreEqual(2, workDays.Count());
            Assert.AreEqual(new TimeSpan(0, 2, 30, 0), workDays[0].TotalDuration);
            Assert.AreEqual(new TimeSpan(0, 1, 30, 0), workDays[1].TotalDuration);
        }


        [TestMethod]
        public void AnalyzeByTaskTest()
        {
            TaskEntryAnalyzer taskEntryAnalyzer =
                new TaskEntryAnalyzer(
                    new[]
                        {
                            new TaskStartEndEntry(
                                new DateTime(2014, 6, 7),
                                "taskcode1",
                                new DateTime(2000, 1, 1, 10, 0, 0, 0),
                                new DateTime(2000, 1, 1, 11, 0, 0, 0)),
                                 new TaskStartEndEntry(
                                new DateTime(2014, 6, 7),
                                "taskcode2",
                                new DateTime(2000, 1, 1, 11, 0, 0, 0),
                                new DateTime(2000, 1, 1, 12, 0, 0, 0))
                        });
            TaskAccumulator[] taskAccumulators = taskEntryAnalyzer.AnalyzeByTask().OrderBy(t => t.TaskCode).ToArray();

            Assert.AreEqual(2, taskAccumulators.Length);

            Assert.AreEqual(0.5f, taskAccumulators[0].NormalizedTotalDuration);
        }


        [TestMethod]
        public void AnalyzeByTaskTest2()
        {
            TaskEntryAnalyzer taskEntryAnalyzer =
                new TaskEntryAnalyzer(
                    new[]
                        {
                            new TaskStartEndEntry(
                                new DateTime(2014, 6, 7),
                                "taskcode",
                                new DateTime(2000, 1, 1, 10, 0, 0, 0),
                                new DateTime(2000, 1, 1, 11, 0, 0, 0)),
                                 new TaskStartEndEntry(
                                new DateTime(2014, 6, 8),
                                "taskcode",
                                new DateTime(2000, 1, 1, 11, 0, 0, 0),
                                new DateTime(2000, 1, 1, 12, 0, 0, 0))
                        });
            TaskAccumulator[] taskAccumulators = taskEntryAnalyzer.AnalyzeByTask().OrderBy(t => t.TaskCode).ToArray();

            Assert.AreEqual(1, taskAccumulators.Length);

            Assert.AreEqual(2f, taskAccumulators[0].NormalizedTotalDuration);
        }

        #endregion
    }
}