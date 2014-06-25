// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkDayViewModel.cs" company="JCS">
//   JCSCopyright
// </copyright>
// <summary>
//   The work day view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LogAnalyzer.ViewModels
{
    using System;

    /// <summary>
    ///     The work day view model.
    /// </summary>
    public class WorkDayViewModel
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Gets or sets the total time.
        /// </summary>
        public TimeSpan TotalTime { get; set; }

        /// <summary>
        ///     Gets or sets the worked time.
        /// </summary>
        public TimeSpan WorkedTime { get; set; }

        #endregion
    }
}