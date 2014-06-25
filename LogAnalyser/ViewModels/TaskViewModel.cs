// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskViewModel.cs" company="JCS">
//   JCSCopyright
// </copyright>
// <summary>
//   The task view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LogAnalyzer.ViewModels
{
    using System;

    /// <summary>
    ///     The task view model.
    /// </summary>
    public class TaskViewModel
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the duration.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        ///     Gets or sets the normalized duration.
        /// </summary>
        public float NormalizedDuration { get; set; }

        /// <summary>
        ///     Gets or sets the task code.
        /// </summary>
        public string TaskCode { get; set; }

        #endregion
    }
}