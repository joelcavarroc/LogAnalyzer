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
    using System.Collections.Generic;

    /// <summary>
    ///     The task entry.
    /// </summary>
    public abstract class TaskEntry
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
        protected TaskEntry(DateTime date, string task)
        {
            this.Date = date;
            this.Task = task.ToUpper();
            this.Comments = new List<string>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the comments.
        /// </summary>
        public List<string> Comments { get; private set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Gets the duration.
        /// </summary>
        public abstract TimeSpan Duration { get; }

        /// <summary>
        ///     Gets the task.
        /// </summary>
        public string Task { get; private set; }

        #endregion
    }
}