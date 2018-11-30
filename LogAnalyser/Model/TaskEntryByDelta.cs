// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskEntryByDelta.cs" company="JCS">
//   JCSCopyright
// </copyright>
// <summary>
//   The task entry by delta.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace LogAnalyzer.Model
{
    /// <summary>
    /// The task entry by delta.
    /// </summary>
    public class TaskEntryByDelta : TaskEntry
    {
        #region Fields

        /// <summary>
        /// The duration.
        /// </summary>
        private readonly TimeSpan duration;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskEntryByDelta"/> class.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <param name="task">
        /// The task.
        /// </param>
        /// <param name="duration">
        /// The duration.
        /// </param>
        public TaskEntryByDelta(DateTime date, string task, TimeSpan duration)
            : base(date, task)
        {
            this.duration = duration;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the duration.
        /// </summary>
        public override TimeSpan Duration
        {
            get
            {
                return this.duration;
            }
        }

        #endregion
    }
}