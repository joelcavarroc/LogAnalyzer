using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LogAnalyzer.Parsing
{
    public class DateParsingStrategy : RegexParsingStrategyBase
    {
        /// <summary>
        ///     Gets the date start reg ex.
        /// </summary>
        public static readonly Regex DateStartRegEx = new Regex(
            @"^%\s+(?<date>\d{4}-\d{2}-\d{2})\s*$",
            RegexOptions.Compiled);

        protected override Regex RegEx => DateParsingStrategy.DateStartRegEx;

        public override string StartPattern => "%";

        protected override void HandlePatternMatch(Match match, ParsingContext parsingContext)
        {
            if (DateTime.TryParseExact(
                match.Groups["date"].Value,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime dateTime))
            {
                parsingContext.CurrentDateTime = dateTime;
                parsingContext.LastTaskEntry = null;
            }
            else
            {
                parsingContext.Errors.Add(new ParserErrorInfo(parsingContext.LineNumber, parsingContext.LineText, ErrorType.InvalidDate));
            }
        }
    }
}