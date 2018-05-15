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

    using Microsoft.Practices.Prism.Mvvm;

    /// <summary>
    ///     The task view model.
    /// </summary>
    public class TaskViewModel : BindableBase
    {
        private TimeSpan duration;

        private float normalizedDuration;

        private string taskCode;

        private int count;

        #region Public Properties

        /// <summary>
        ///     Gets or sets the duration.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return this.duration;
            }
            set
            {
                this.SetProperty(ref this.duration, value);
            }
        }

        /// <summary>
        ///     Gets or sets the normalized duration.
        /// </summary>
        public float NormalizedDuration
        {
            get
            {
                return this.normalizedDuration;
            }
            set
            {
                this.SetProperty(ref this.normalizedDuration, value);
            }
        }

        /// <summary>
        ///     Gets or sets the task code.
        /// </summary>
        public string TaskCode
        {
            get
            {
                return this.taskCode;
            }
            set
            {
                this.SetProperty(ref this.taskCode, value);
            }
        }

        public int Count
        {
            get
            {
                return this.count;
            }
            set
            {
                this.SetProperty(ref this.count, value);
            }
        }

        #endregion
    }
}