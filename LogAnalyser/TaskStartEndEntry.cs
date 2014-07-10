// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskStartEndEntry.cs" company="JCS">
//   JCSCopyright
// </copyright>
// <summary>
//   The task start end entry.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LogAnalyzer
{
    using System;

    /// <summary>
    ///     The task start end entry.
    /// </summary>
    public class TaskStartEndEntry : TaskEntry
    {
        #region Fields

        /// <summary>
        ///     The end time.
        /// </summary>
        private readonly DateTime endTime;

        /// <summary>
        ///     The start time.
        /// </summary>
        private readonly DateTime startTime;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskStartEndEntry"/> class.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <param name="task">
        /// The task.
        /// </param>
        /// <param name="startTime">
        /// The start time.
        /// </param>
        /// <param name="endTime">
        /// The end time.
        /// </param>
        public TaskStartEndEntry(DateTime date, string task, DateTime startTime, DateTime endTime)
            : base(date, task)
        {
            this.startTime = startTime;
            this.endTime = endTime;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the duration.
        /// </summary>
        public override TimeSpan Duration
        {
            get
            {
                return this.EndTime - this.StartTime;
            }
        }

        /// <summary>
        ///     The end time.
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return this.endTime;
            }
        }

        /// <summary>
        ///     The start time.
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return this.startTime;
            }
        }

        #endregion
    }
}