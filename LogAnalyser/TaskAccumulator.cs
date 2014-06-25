// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskAccumulator.cs" company="JCS">
//   JCSCopyright
// </copyright>
// <summary>
//   The task accumulator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LogAnalyzer
{
    using System;

    /// <summary>
    /// The task accumulator.
    /// </summary>
    public class TaskAccumulator
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskAccumulator"/> class.
        /// </summary>
        /// <param name="taskCode">
        /// The task code.
        /// </param>
        public TaskAccumulator(string taskCode)
        {
            this.TaskCode = taskCode;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the normalized total duration.
        /// </summary>
        public float NormalizedTotalDuration { get; set; }

        /// <summary>
        /// Gets or sets the task code.
        /// </summary>
        public string TaskCode { get; set; }

        /// <summary>
        /// Gets or sets the total duration.
        /// </summary>
        public TimeSpan TotalDuration { get; set; }

        #endregion
    }
}