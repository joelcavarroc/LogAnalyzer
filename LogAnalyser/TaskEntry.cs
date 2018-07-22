// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskEntry.cs" company="JCS">
//   JCSCopyright
// </copyright>
// <summary>
//   The task entry.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LogAnalyzer
{
    using System;

    /// <summary>
    ///     The task entry.
    /// </summary>
    public abstract class TaskEntry : LogEntry
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskEntry"/> class.
        /// </summary>
        /// <param name="date">
        /// </param>
        /// <param name="task">
        /// The task.
        /// </param>
        protected TaskEntry(DateTime date, string task) : base(task)
        {
            this.Date = date;
        }

        #endregion

        #region Public Properties

         /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Gets the duration.
        /// </summary>
        public abstract TimeSpan Duration { get; }

        #endregion
    }
}