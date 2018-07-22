using System.Text.RegularExpressions;

namespace LogAnalyzer.Parsing
{
    public class TagParsingStrategy : RegexParsingStrategyBase
    {
        /// <summary>
        ///     Gets the task delta regex.
        /// </summary>
        private static readonly Regex TagRegEx = new Regex(
            @"^#(?<task>\p{L}\S+):(?<comment>.*)$",
            RegexOptions.Compiled);

        protected override Regex RegEx => TagParsingStrategy.TagRegEx;

        public override string StartPattern => "#";

        protected override void HandlePatternMatch(Match match, ParsingContext parsingContext)
        {
        }
    }
}