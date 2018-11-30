// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkDay.cs" company="JCS">
//   JCSCopyright
// </copyright>
// <summary>
//   The work day.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace LogAnalyzer.Model
{
    /// <summary>
    /// The work day.
    /// </summary>
    public class WorkDay
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkDay"/> class.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        public WorkDay(DateTime date)
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
        /// Gets or sets the total duration.
        /// </summary>
        public TimeSpan TotalDuration { get; set; }

        #endregion
    }
}