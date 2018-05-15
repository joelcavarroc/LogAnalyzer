// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="JCS">
//   JCSCopyright
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace LogAnalyzer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Odbc;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Microsoft.Practices.Prism.Mvvm;

    /// <summary>
    ///     The main window view model.
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        #region Fields

        /// <summary>
        ///     The errors.
        /// </summary>
        private readonly ObservableCollection<ErrorViewModel> errors = new ObservableCollection<ErrorViewModel>();

        /// <summary>
        ///     The tasks.
        /// </summary>
        private readonly ObservableCollection<TaskViewModel> tasks = new ObservableCollection<TaskViewModel>();

        /// <summary>
        ///     The work days view models.
        /// </summary>
        private readonly ObservableCollection<WorkDayViewModel> workDays = new ObservableCollection<WorkDayViewModel>();

        private string filename;

        private string lastSavedText;

        /// <summary>
        ///     The log text.
        /// </summary>
        private string logText;

        /// <summary>
        ///     The selected error.
        /// </summary>
        private ErrorViewModel selectedError;

        private SaveCommand saveCommand;

        private double normalizedTotalDuration;

        private TaskViewModel selectedTask;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the errors.
        /// </summary>
        public ObservableCollection<ErrorViewModel> Errors
        {
            get
            {
                return this.errors;
            }
        }

        public string Filename
        {
            get
            {
                return this.filename;
            }
            set
            {
                this.SetProperty(ref this.filename, value);
            }
        }

        public bool IsModified
        {
            get
            {
                if (string.IsNullOrEmpty(this.LastSavedText))
                {
                    return !string.IsNullOrEmpty(this.LogText);
                }

                return !this.LastSavedText.Equals(this.LogText, StringComparison.CurrentCulture);
            }
        }

        public string LastSavedText
        {
            get
            {
                return this.lastSavedText;
            }
            set
            {
                this.SetProperty(ref this.lastSavedText, value);
                this.OnPropertyChanged(() => this.IsModified);
                this.OnPropertyChanged(() => this.ModificationIndicator);
            }
        }

        public LoadCommand LoadCommand { get; set; }

        /// <summary>
        ///     Gets or sets the log text.
        /// </summary>
        public string LogText
        {
            get
            {
                return this.logText;
            }
            set
            {
                this.SetProperty(ref this.logText, value);
                this.OnPropertyChanged(() => this.IsModified);
                this.OnPropertyChanged(() => this.ModificationIndicator);
                this.AnalyzeLog();
            }
        }

        public string ModificationIndicator
        {
            get
            {
                if (this.IsModified)
                {
                    return "*";
                }
                return string.Empty;
            }
        }

        public SaveCommand SaveCommand
        {
            get
            {
                return this.saveCommand;
            }
            set
            {
                this.saveCommand = value;
            }
        }

        /// <summary>
        ///     The selected error.
        /// </summary>
        public ErrorViewModel SelectedError
        {
            get
            {
                return this.selectedError;
            }
            set
            {
                this.SetProperty(ref this.selectedError, value);
            }
        }

        /// <summary>
        ///     Gets the tasks.
        /// </summary>
        public ObservableCollection<TaskViewModel> Tasks
        {
            get
            {
                return this.tasks;
            }
        }

        /// <summary>
        ///     Gets the work days view models.
        /// </summary>
        public ObservableCollection<WorkDayViewModel> WorkDays
        {
            get
            {
                return this.workDays;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The Analyze log.
        /// </summary>
        private void AnalyzeLog()
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(this.logText)))
            using (StreamReader streamReader = new StreamReader(memoryStream))
            using (Parser parser = new Parser(streamReader))
            {
                parser.Parse();
                TaskEntryAnalyzer taskEntryAnalyzer = new TaskEntryAnalyzer(parser.TaskEntries);

                this.UpdateWorkDays(taskEntryAnalyzer);
                this.UpdateTasks(taskEntryAnalyzer);


                this.UpdateErrors(parser, taskEntryAnalyzer);
            }
        }

        /// <summary>
        ///     The update errors.
        /// </summary>
        /// <param name="parser">
        ///     The parser.
        /// </param>
        /// <param name="analyzer"></param>
        private void UpdateErrors(Parser parser, TaskEntryAnalyzer analyzer)
        {
            this.errors.Clear();
            foreach (ParserErrorInfo error in parser.Errors)
            {
                this.errors.Add(
                    new ErrorViewModel
                    {
                        Line = error.LineNumber,
                        LineText = error.LineText,
                        ErrorText = error.ErrorType.ToString()
                    });
            }

            List<AnalyzerError> analyzerErrors = analyzer.Validate();

            if (analyzerErrors.Count > 0)
            {
                string[] lines = this.LogText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (AnalyzerError error in analyzerErrors)
                {
                    TaskEntry[] taskEntriesInError = error.TaskEntries;
                    int lineInError = parser.GetTaskEntryLine(taskEntriesInError[taskEntriesInError.GetUpperBound(0)]);
                    StringBuilder stringBuilder = new StringBuilder();

                    foreach (TaskEntry taskEntry in taskEntriesInError)
                    {
                        int lineNumber = parser.GetTaskEntryLine(taskEntry);
                        string line = lines[lineNumber];
                        stringBuilder.AppendFormat("{0} ({1})", line, lineNumber);
                    }

                    this.errors.Add(
                        new ErrorViewModel { ErrorText = error.ErrorType.ToString(), Line = lineInError, LineText = "" });
                }
            }
        }

        public double NormalizedTotalDuration
        {
            get
            {
                return this.normalizedTotalDuration;
            }
            internal set
            {
                this.SetProperty(ref this.normalizedTotalDuration, value);
            }
        }

        public TaskViewModel SelectedTask
        {
            get
            {
                return this.selectedTask;
            }
            set
            {
                this.SetProperty(ref this.selectedTask, value);
            }

        }

        /// <summary>
        ///     The update tasks.
        /// </summary>
        /// <param name="taskEntryAnalyzer">
        ///     The task entry Analyzer.
        /// </param>
        private void UpdateTasks(TaskEntryAnalyzer taskEntryAnalyzer)
        {
            Dictionary<string, TaskViewModel> taskEntries = this.Tasks.ToDictionary(t => t.TaskCode);

            double totalDuration = 0;
            foreach (TaskAccumulator task in taskEntryAnalyzer.AnalyzeByTask().OrderBy(t => t.TaskCode))
            {
                TaskViewModel taskViewModel;

                if (!taskEntries.TryGetValue(task.TaskCode, out taskViewModel))
                {
                    taskViewModel = new TaskViewModel();
                    this.Tasks.Add(taskViewModel);
                }
                else
                {
                    taskEntries.Remove(task.TaskCode);
                }

                taskViewModel.TaskCode = task.TaskCode;
                taskViewModel.Duration = task.TotalDuration;
                taskViewModel.NormalizedDuration = task.NormalizedTotalDuration;
                taskViewModel.Count = task.Count;

                totalDuration += task.NormalizedTotalDuration;
            }
            foreach (TaskViewModel task in taskEntries.Values)
            {
                this.Tasks.Remove(task);
            }

            this.NormalizedTotalDuration = totalDuration;
        }

        /// <summary>
        ///     The update work days.
        /// </summary>
        /// <param name="taskEntryAnalyzer">
        ///     The task entry Analyzer.
        /// </param>
        private void UpdateWorkDays(TaskEntryAnalyzer taskEntryAnalyzer)
        {
            this.WorkDays.Clear();
            TimeSpan totalTimeSpan = new TimeSpan();
            TimeSpan normalTime = new TimeSpan();
            foreach (WorkDay workDay in taskEntryAnalyzer.AnalyzeByDay().OrderBy(wd => wd.Date))
            {
                normalTime += new TimeSpan(0, 8, 0, 0);
                totalTimeSpan += workDay.TotalDuration;

                this.WorkDays.Add(
                    new WorkDayViewModel
                    {
                        Date = workDay.Date,
                        WorkedTime = workDay.TotalDuration,
                        TotalTime = totalTimeSpan - normalTime
                    });
            }
        }

        #endregion
    }
}