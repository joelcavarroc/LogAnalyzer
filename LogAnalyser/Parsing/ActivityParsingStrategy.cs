using System.Text.RegularExpressions;
using LogAnalyzer.Model;

namespace LogAnalyzer.Parsing
{
    // ReSharper disable once UnusedMember.Global
    public class ActivityParsingStrategy : RegexParsingStrategyBase
    {
        /// <summary>
        ///     Gets the task delta regex.
        /// </summary>
        private static readonly Regex TagRegEx = new Regex(
            @"^#(?<task>\p{L}\S+):(?<comment>.*)$",
            RegexOptions.Compiled);

        protected override Regex RegEx => ActivityParsingStrategy.TagRegEx;

        public override string StartPattern => "#";

        protected override void HandlePatternMatch(Match match, ParsingContext parsingContext)
        {
            string task = match.Groups["task"].Value;
            string comment = match.Groups["comment"].Value;
            TaggedEntry taggedEntry = new TaggedEntry(task, comment);

            parsingContext.TaggedEntries.Add(taggedEntry, parsingContext.LineNumber);
        }
    }
}