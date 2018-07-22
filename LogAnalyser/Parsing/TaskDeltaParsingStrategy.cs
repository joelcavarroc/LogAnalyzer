using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LogAnalyzer.Parsing
{
    public class TaskDeltaParsingStrategy : RegexParsingStrategyBase
    {
        /// <summary>
        ///     Gets the task delta regex.
        /// </summary>
        public static readonly Regex TaskDeltaRegex = new Regex(
            @"^%\s+(?<task>\p{L}\S+)\s+(?<sign>[+|-])(?<duration>\d{1,2}:\d{2})\s*$",
            RegexOptions.Compiled);

        protected override Regex RegEx => TaskDeltaParsingStrategy.TaskDeltaRegex;

        public override string StartPattern => "%";

        protected override void HandlePatternMatch(Match match, ParsingContext parsingContext)
        {
            if (!parsingContext.CurrentDateTime.HasValue)
            {
                parsingContext.Errors.Add(
                    new ParserErrorInfo(
                        parsingContext.LineNumber,
                        parsingContext.LineText,
                        ErrorType.NoDateDefined));
                return;
            }

            string task = match.Groups["task"].Value;
            string sign = match.Groups["sign"].Value;
            string delta = match.Groups["duration"].Value;
            if (sign.Equals("-"))
            {
                delta = "-" + delta;
            }

            if (TimeSpan.TryParse(delta, CultureInfo.InvariantCulture, out TimeSpan timespan))
            {
                parsingContext.TaskEntries.Add(
                    new TaskEntryByDelta(
                        parsingContext.CurrentDateTime.Value,
                        task,
                        timespan),
                    parsingContext.LineNumber);
            }
            else
            {
                parsingContext.Errors.Add(
                    new ParserErrorInfo(
                        parsingContext.LineNumber,
                        parsingContext.LineText,
                        ErrorType.InvalidDelta));
            }
        }
    }
}