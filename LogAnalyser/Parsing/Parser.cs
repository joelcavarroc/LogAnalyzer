// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Parser.cs" company="JCS">
//   JCSCopyright
// </copyright>
// <summary>
//   The parser.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LogAnalyzer.Parsing
{
    /// <summary>
    ///     The parser.
    /// </summary>
    public sealed class Parser : IDisposable
    {
        #region Fields

        /// <summary>
        ///     The errors.
        /// </summary>
        public List<ParserErrorInfo> Errors { get; private set; }

        /// <summary>
        ///     The stream reader.
        /// </summary>
        private readonly StreamReader streamReader;

        /// <summary>
        ///     The task entries.
        /// </summary>
        private Dictionary<TaskEntry, int> taskEntries = new Dictionary<TaskEntry, int>();

        private List<IParsingStrategy> parsingStrategies;

        private ILookup<string, IParsingStrategy> parsingStrategiesLookup;
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
            this.LoadStrategies();

            this.Errors = new List<ParserErrorInfo>();
        }

        private void LoadStrategies()
        {
            this.parsingStrategies = Assembly.GetExecutingAssembly().DefinedTypes
                .Where(t => typeof(IParsingStrategy).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IParsingStrategy>().ToList();

            this.parsingStrategiesLookup = this.parsingStrategies.ToLookup(ps => ps.StartPattern);
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the task entries.
        /// </summary>
        public IEnumerable<TaskEntry> TaskEntries
        {
            get { return this.taskEntries.Keys; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            this.streamReader?.Dispose();
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
            ParsingContext parsingContext = new ParsingContext();

            string line;
            int lineNumber = 1;
            while ((line = this.streamReader.ReadLine()) != null)
            {
                parsingContext.LineNumber = lineNumber++;
                parsingContext.LineText = line;

                this.ParseLine(parsingContext);
            }

            this.taskEntries = parsingContext.TaskEntries;
            this.Errors = parsingContext.Errors;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Parse a line.
        /// </summary>
        /// <param name="parsingContext">The parsing context</param>
        private void ParseLine(ParsingContext parsingContext)
        {
            foreach(IGrouping<string, IParsingStrategy> parsingStrategiesGroup in this.parsingStrategiesLookup)
            {
                if (parsingContext.LineText.StartsWith(parsingStrategiesGroup.Key))
                {
                    bool success = false;
                    foreach (IParsingStrategy parsingStategy in parsingStrategiesGroup)
                    {
                        success = parsingStategy.ParseLine(parsingContext);
                        if (success) break;
                    }

                    if (!success)
                    {
                        parsingContext.Errors.Add(new ParserErrorInfo(parsingContext.LineNumber, parsingContext.LineText, ErrorType.InvalidLine));
                        Trace.WriteLine($"Invalid line {parsingContext.LineNumber}: {parsingContext.LineText}");
                    }

                    return;
                }
            }

            if (parsingContext.LastTaskEntry != null)
            {
                parsingContext.LastTaskEntry.Comments.Add(parsingContext.LineText);
            }
            else
            {
                parsingContext.Errors.Add(new ParserErrorInfo(parsingContext.LineNumber, parsingContext.LineText, ErrorType.CommentFoundBeforeTask));
                Trace.WriteLine($"Comment found before task {parsingContext.LineNumber}: {parsingContext.LineText}");
            }
        }

        #endregion
    }
}