using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LogAnalyzer.Parsing
{
    public class TaskParsingStrategy : RegexParsingStrategyBase
    {
        protected override Regex RegEx => TaskParsingStrategy.TaskRegex;

        public override string StartPattern => "%";

        protected override void HandlePatternMatch(Match match, ParsingContext parsingContext)
        {
            if (!parsingContext.CurrentDateTime.HasValue)
            {
                parsingContext.Errors.Add(new ParserErrorInfo(parsingContext.LineNumber, parsingContext.LineText, ErrorType.NoDateDefined));
                return;
            }

            string task = match.Groups["task"].Value;
            DateTime startTime = DateTime.ParseExact(
                match.Groups["startTime"].Value,
                "H:mm",
                CultureInfo.InvariantCulture);
            DateTime endTime = DateTime.ParseExact(match.Groups["endTime"].Value, "H:mm", CultureInfo.InvariantCulture);
            TaskStartEndEntry taskStartEndEntry = new TaskStartEndEntry(
                parsingContext.CurrentDateTime.Value,
                task,
                startTime,
                endTime);

            parsingContext.TaskEntries.Add(taskStartEndEntry, parsingContext.LineNumber);
            parsingContext.LastTaskEntry = taskStartEndEntry;
        }

        /// <summary>
        ///     Gets the task regex.
        /// </summary>
        public static readonly Regex TaskRegex = new Regex(
            @"^%\s+(?<task>\p{L}\S+)\s+(?<startTime>\d{1,2}:\d{2})\s+(?<endTime>\d{1,2}:\d{2})\s*$",
            RegexOptions.Compiled);

 }
}