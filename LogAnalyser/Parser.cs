// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Parser.cs" company="JCS">
//   JCSCopyright
// </copyright>
// <summary>
//   The parser.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LogAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     The parser.
    /// </summary>
    public sealed class Parser : IDisposable
    {
        #region Constants

        /// <summary>
        ///     The start of line.
        /// </summary>
        private readonly string[] StartOfLine = new [] { "%", "#" };

        #endregion

        #region Fields

        /// <summary>
        ///     The errors.
        /// </summary>
        private readonly List<ParserErrorInfo> errors;

        /// <summary>
        ///     The read only errors.
        /// </summary>
        private readonly IEnumerable<ParserErrorInfo> readOnlyErrors;

        /// <summary>
        ///     The reg exes.
        /// </summary>
        private readonly Tuple<Regex, Action<Match, int, string>>[] regExes;

        /// <summary>
        ///     The stream reader.
        /// </summary>
        private readonly StreamReader streamReader;

        /// <summary>
        ///     The task entries.
        /// </summary>
        private readonly Dictionary<TaskEntry, int> taskEntries = new Dictionary<TaskEntry, int>();

        /// <summary>
        ///     The current date.
        /// </summary>
        private DateTime? currentDate;

        /// <summary>
        ///     The last task entry.
        /// </summary>
        private TaskEntry lastTaskEntry;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        public Parser(string filename)
            : this()
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(filename));

            this.streamReader = new StreamReader(filename);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class.
        /// </summary>
        /// <param name="streamReader">
        /// The stream reader.
        /// </param>
        public Parser(StreamReader streamReader)
            : this()
        {
            Contract.Requires(!(streamReader is null));

            this.streamReader = streamReader;
        }

        /// <summary>
        ///     Prevents a default instance of the <see cref="Parser" /> class from being created.
        /// </summary>
        private Parser()
        {
            this.regExes = new[]
                               {
                                   new Tuple<Regex, Action<Match, int, string>>(
                                       DateStartRegEx, 
                                       this.HandleDateStartRegularExpression), 
                                   new Tuple<Regex, Action<Match, int, string>>(TaskRegex, this.HandleTaskRegex),
                                   new Tuple<Regex, Action<Match, int, string>>(
                                       TaskDeltaRegex, 
                                       this.HandleTaskDeltaRegex
                                       ),
                                   new Tuple<Regex, Action<Match, int, string>>(TagRegEx, this.HandleTagRegEx)
                               };

            this.errors = new List<ParserErrorInfo>();
            this.readOnlyErrors = new ReadOnlyCollection<ParserErrorInfo>(this.errors);
        }

        private void HandleTagRegEx(Match arg1, int arg2, string arg3)
        {
            
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the date start reg ex.
        /// </summary>
        public static Regex DateStartRegEx
        {
            get
            {
                return new Regex(@"^%\s+(?<date>\d{4}-\d{2}-\d{2})\s*$");
            }
        }

        /// <summary>
        ///     Gets the task delta regex.
        /// </summary>
        public static Regex TaskDeltaRegex
        {
            get
            {
                return new Regex(@"^%\s+(?<task>\p{L}\S+)\s+(?<sign>[+|-])(?<duration>\d{1,2}:\d{2})\s*$");
            }
        }

        /// <summary>
        ///     Gets the task regex.
        /// </summary>
        public static Regex TaskRegex
        {
            get
            {
                return new Regex(
                    @"^%\s+(?<task>\p{L}\S+)\s+(?<startTime>\d{1,2}:\d{2})\s+(?<endTime>\d{1,2}:\d{2})\s*$");
            }
        }

        /// <summary>
        ///     Gets the task delta regex.
        /// </summary>
        public static Regex TagRegEx
        {
            get
            {
                return new Regex(@"^#(?<task>\p{L}\S+):(?<comment>.*)$");
            }
        }


        /// <summary>
        ///     Gets the parsing errors.
        /// </summary>
        public IEnumerable<ParserErrorInfo> Errors
        {
            get
            {
                return this.readOnlyErrors;
            }
        }

        /// <summary>
        ///     Gets the task entries.
        /// </summary>
        public IEnumerable<TaskEntry> TaskEntries
        {
            get
            {
                return this.taskEntries.Keys;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            if (this.streamReader != null)
            {
                this.streamReader.Dispose();
            }
        }

        public int GetTaskEntryLine(TaskEntry taskEntry)
        {
            return this.taskEntries[taskEntry];
        }

        /// <summary>
        ///     Parse the current filename.
        /// </summary>
        public void Parse()
        {
            string line;
            int lineNumber = 1;
            while ((line = this.streamReader.ReadLine()) != null)
            {
                this.ParseLine(line, lineNumber++);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The handle date start reg ex.
        /// </summary>
        /// <param name="match">
        /// The match.
        /// </param>
        /// <param name="lineNumber">
        /// The line Number.
        /// </param>
        /// <param name="lineText">
        /// The line Text.
        /// </param>
        private void HandleDateStartRegularExpression(Match match, int lineNumber, string lineText)
        {
            DateTime dateTime;
            if (DateTime.TryParseExact(
                match.Groups["date"].Value, 
                "yyyy-MM-dd", 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out dateTime))
            {
                this.currentDate = dateTime;
                this.lastTaskEntry = null;
            }
            else
            {
                this.errors.Add(new ParserErrorInfo(lineNumber, lineText, ErrorType.InvalidDate));
            }
        }

        /// <summary>
        /// The handle task delta regex.
        /// </summary>
        /// <param name="match">
        /// The match.
        /// </param>
        /// <param name="lineNumber">
        /// The line Number.
        /// </param>
        /// <param name="lineText">
        /// The line Text.
        /// </param>
        private void HandleTaskDeltaRegex(Match match, int lineNumber, string lineText)
        {
            if (!this.currentDate.HasValue)
            {
                this.errors.Add(new ParserErrorInfo(lineNumber, lineText, ErrorType.NoDateDefined));
                return;
            }

            string task = match.Groups["task"].Value;
            string sign = match.Groups["sign"].Value;
            string delta = match.Groups["duration"].Value;
            if (sign.Equals("-"))
            {
                delta = "-" + delta;
            }

            TimeSpan timespan;

            if (TimeSpan.TryParse(delta, CultureInfo.InvariantCulture, out timespan))
            {
                this.taskEntries.Add(new TaskEntryByDelta(this.currentDate.Value, task, timespan), lineNumber);
            }
            else
            {
                this.errors.Add(new ParserErrorInfo(lineNumber, lineText, ErrorType.InvalidDelta));
            }
        }

        /// <summary>
        /// The handle task regex.
        /// </summary>
        /// <param name="match">
        /// The match.
        /// </param>
        /// <param name="lineNumber">
        /// The line Number.
        /// </param>
        /// <param name="lineText">
        /// The line Text.
        /// </param>
        private void HandleTaskRegex(Match match, int lineNumber, string lineText)
        {
            if (!this.currentDate.HasValue)
            {
                this.errors.Add(new ParserErrorInfo(lineNumber, lineText, ErrorType.NoDateDefined));
                return;
            }

            string task = match.Groups["task"].Value;
            DateTime startTime = DateTime.ParseExact(
                match.Groups["startTime"].Value, 
                "H:mm", 
                CultureInfo.InvariantCulture);
            DateTime endTime = DateTime.ParseExact(match.Groups["endTime"].Value, "H:mm", CultureInfo.InvariantCulture);
            TaskStartEndEntry taskStartEndEntry = new TaskStartEndEntry(
                this.currentDate.Value, 
                task, 
                startTime, 
                endTime);

            this.taskEntries.Add(taskStartEndEntry, lineNumber);

            this.lastTaskEntry = taskStartEndEntry;
        }

        /// <summary>
        /// Parse a line.
        /// </summary>
        /// <param name="line">
        ///     The line to parse.
        /// </param>
        /// <param name="lineNumber">
        ///     The line Number.
        /// </param>
        private void ParseLine(string line, int lineNumber)
        {
            foreach (string startChar in StartOfLine)
            {
                if (line.StartsWith(startChar))
                {
                    bool success = false;
                    foreach (var t in this.regExes)
                    {
                        Match match = t.Item1.Match(line);
                        if (match.Success)
                        {
                            t.Item2.Invoke(match, lineNumber, line);
                            success = true;
                            break;
                        }
                    }

                    if (!success)
                    {
                        this.errors.Add(new ParserErrorInfo(lineNumber, line, ErrorType.InvalidLine));
                        Trace.WriteLine(string.Format("Invalid line {0}: {1}", lineNumber, line));
                    }

                    return;
                }
            }

            if (this.lastTaskEntry != null)
            {
                this.lastTaskEntry.Comments.Add(line);
            }
        }

        #endregion
    }
}