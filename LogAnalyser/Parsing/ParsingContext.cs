using System;
using System.Collections.Generic;

namespace LogAnalyzer.Parsing
{
    public class ParsingContext
    {
        public string LineText { get; set; }

        public int LineNumber { get; set; }

        public DateTime? CurrentDateTime { get; set; }

        public List<ParserErrorInfo> Errors { get; } = new List<ParserErrorInfo>();

        public TaskEntry LastTaskEntry { get; set; }

        public Dictionary<TaskEntry, int> TaskEntries { get; }= new Dictionary<TaskEntry, int>();

    }
}