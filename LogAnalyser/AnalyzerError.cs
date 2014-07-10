namespace LogAnalyzer
{
    public class AnalyzerError
    {
        #region Constructors and Destructors

        public AnalyzerError(AnalyzerErrorType errorType, params TaskEntry[] taskEntries)
        {
            this.TaskEntries = taskEntries;
            this.ErrorType = errorType;
        }

        #endregion

        #region Public Properties

        public AnalyzerErrorType ErrorType { get; private set; }

        public TaskEntry[] TaskEntries { get; private set; }

        #endregion
    }
}