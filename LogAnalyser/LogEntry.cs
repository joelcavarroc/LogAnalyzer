

using System.Collections.Generic;

namespace LogAnalyzer
{
    public abstract class LogEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskEntry"/> class.
        /// </summary>
        /// <param name="date">
        /// </param>
        /// <param name="task">
        /// The task.
        /// </param>
        protected LogEntry(string task)
        {
            this.Task = task.ToUpper();
            this.Comments = new List<string>();
        }

        /// <summary>
        ///     Gets the task.
        /// </summary>
        public string Task { get; private set; }

        /// <summary>
        ///     Gets the comments.
        /// </summary>
        public List<string> Comments { get; private set; }
    }
}
