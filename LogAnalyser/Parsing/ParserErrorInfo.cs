// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParserErrorInfo.cs" company="JCS">
//   JCSCopyright
// </copyright>
// <summary>
//   The parser error info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LogAnalyzer.Parsing
{
    /// <summary>
    ///     The parser error info.
    /// </summary>
    public class ParserErrorInfo
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserErrorInfo"/> class.
        /// </summary>
        /// <param name="lineNumber">
        /// The line number.
        /// </param>
        /// <param name="lineText">
        /// The line text.
        /// </param>
        /// <param name="errorType">
        /// The error type.
        /// </param>
        public ParserErrorInfo(int lineNumber, string lineText, ErrorType errorType)
        {
            this.LineNumber = lineNumber;
            this.LineText = lineText;
            this.ErrorType = errorType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the error type.
        /// </summary>
        public ErrorType ErrorType { get; set; }

        /// <summary>
        ///     Gets or sets the line number.
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        ///     Gets or sets the line text.
        /// </summary>
        public string LineText { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The to string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Error {0} line {1} : {2}", this.ErrorType, this.LineNumber, this.LineText);
        }

        #endregion
    }
}