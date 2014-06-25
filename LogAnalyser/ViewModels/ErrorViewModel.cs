// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorViewModel.cs" company="JCS">
//   JCSCopyright
// </copyright>
// <summary>
//   The error view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LogAnalyzer.ViewModels
{
    /// <summary>
    ///     The error view model.
    /// </summary>
    public class ErrorViewModel
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the error text.
        /// </summary>
        public string ErrorText { get; set; }

        /// <summary>
        ///     Gets or sets the line.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        ///     Gets or sets the line text.
        /// </summary>
        public string LineText { get; set; }

        #endregion
    }
}