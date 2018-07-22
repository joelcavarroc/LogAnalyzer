// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="JCS">
//   JCSCopyright
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace LogAnalyzer.Parsing
{
    internal interface IParsingStrategy
    {
        /// <summary>
        /// Define the start of line pattern
        /// </summary>
        string StartPattern { get; }


        bool ParseLine(ParsingContext parsingContext);
    }
}
