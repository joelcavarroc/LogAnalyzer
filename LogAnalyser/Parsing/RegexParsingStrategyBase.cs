using System.Text.RegularExpressions;

namespace LogAnalyzer.Parsing
{
    public abstract class RegexParsingStrategyBase : IParsingStrategy
    {
        protected abstract Regex RegEx { get; }

        public abstract string StartPattern { get; }

        public bool ParseLine(ParsingContext parsingContext)
        {
            Match match = this.RegEx.Match(parsingContext.LineText);
            if (!match.Success)
            {
                return false;
            }

            this.HandlePatternMatch(match, parsingContext);

            return true;
        }

        protected abstract void HandlePatternMatch(Match match, ParsingContext parsingContext);
    }
}