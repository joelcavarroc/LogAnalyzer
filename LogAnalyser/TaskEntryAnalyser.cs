// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskEntryAnalyzer.cs" company="JCS">
//   JCSCopyright
// </copyright>
// <summary>
//   The task entry Analyzer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LogAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     The task entry Analyzer.
    /// </summary>
    public class TaskEntryAnalyzer
    {
        #region Fields

        /// <summary>
        ///     The task entries.
        /// </summary>
        private readonly IEnumerable<TaskEntry> taskEntries;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskEntryAnalyzer"/> class.
        /// </summary>
        /// <param name="taskEntries">
        /// The task entries.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public TaskEntryAnalyzer(IEnumerable<TaskEntry> taskEntries)
        {
            if (taskEntries == null)
            {
                throw new ArgumentNullException("taskEntries");
            }

            this.taskEntries = taskEntries;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The Analyze by day.
        /// </summary>
        /// <returns>
        ///     The <see cref="List" />.
        /// </returns>
        public List<WorkDay> AnalyzeByDay()
        {
            IEnumerable<IGrouping<DateTime, TaskEntry>> groupedByDate = this.taskEntries.GroupBy(t => t.Date);

            List<WorkDay> tasksPerDay = new List<WorkDay>();

            foreach (IGrouping<DateTime, TaskEntry> taskGroup in groupedByDate)
            {
                WorkDay workDay = new WorkDay(taskGroup.Key)
                                      {
                                          TotalDuration =
                                              taskGroup.Aggregate(
                                                  new TimeSpan(), 
                                                  (current, taskEntry) =>
                                                  current + taskEntry.Duration)
                                      };
                tasksPerDay.Add(workDay);
            }

            return tasksPerDay;
        }

        /// <summary>
        ///     The Analyze by task.
        /// </summary>
        /// <returns>
        ///     The <see cref="List" />.
        /// </returns>
        public List<TaskAccumulator> AnalyzeByTask()
        {
            IEnumerable<IGrouping<string, TaskEntry>> groupedByTask = this.taskEntries.GroupBy(t => t.Task);

            IDictionary<DateTime, WorkDay> workDays = this.AnalyzeByDay().ToDictionary(wd => wd.Date);

            List<TaskAccumulator> taskAccumulators = new List<TaskAccumulator>();
            foreach (IGrouping<string, TaskEntry> entriesByTask in groupedByTask)
            {
                TaskAccumulator taskAccumulator = new TaskAccumulator(entriesByTask.Key);
                foreach (TaskEntry taskEntry in entriesByTask)
                {
                    WorkDay workDay = workDays[taskEntry.Date];

                    float normalizedDuration = ((float)taskEntry.Duration.Ticks / workDay.TotalDuration.Ticks) * 8;

                    taskAccumulator.NormalizedTotalDuration += normalizedDuration / 8;
                    taskAccumulator.TotalDuration += taskEntry.Duration;
                }

                taskAccumulators.Add(taskAccumulator);
            }

            return taskAccumulators;
        }

        #endregion
    }
}