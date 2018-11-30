// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="JCS">
//   JCSCopyright
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using LogAnalyzer.Analysing;
using LogAnalyzer.Model;
using LogAnalyzer.Parsing;
using Microsoft.Practices.Prism;

namespace LogAnalyzer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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

        private double normalizedTotalDuration;

        private TaskViewModel selectedTask;
        private FileSystemWatcher fileSystemWatcher;
        private Task analyzeTask;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the errors.
        /// </summary>
        public ObservableCollection<ErrorViewModel> Errors { get; } = new ObservableCollection<ErrorViewModel>();

        public string Filename
        {
            get
            {
                return this.filename;
            }
            set
            {
                this.SetProperty(ref this.filename, value);
                if (fileSystemWatcher != null)
                {
                    fileSystemWatcher.Changed -= FileSystemWatcher_Changed;
                    fileSystemWatcher.Dispose();
                    fileSystemWatcher = new FileSystemWatcher(value);
                    fileSystemWatcher.Changed += FileSystemWatcher_Changed;
                }
            }
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
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

                bool? isTaskCompleted = this.analyzeTask?.IsCompleted;
                if (isTaskCompleted.HasValue && !isTaskCompleted.Value)
                {
                    this.cancellationTokenSource.Cancel();
                }

                this.analyzeTask = Task.Run(
                        async delegate
                        {
                            await Task.Delay(TimeSpan.FromSeconds(0.1));
                            // Execute the update on the Application main dispatcher as it does not multithreaded update.
                            Application.Current.Dispatcher.Invoke(this.AnalyzeLog);
                        },
                        this.cancellationTokenSource.Token);
                
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

        public SaveCommand SaveCommand { get; set; }

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
        public ObservableCollection<TaskViewModel> Tasks { get; } = new ObservableCollection<TaskViewModel>();

        /// <summary>
        ///     Gets the work days view models.
        /// </summary>
        public ObservableCollection<WorkDayViewModel> WorkDays { get; } = new ObservableCollection<WorkDayViewModel>();

        public ObservableCollection<TagTypeViewModel> TagTypes { get; } = new ObservableCollection<TagTypeViewModel>();

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

                TaggedEntryAnalyser taggedEntryAnalyser = new TaggedEntryAnalyser();
                this.UpdateTaggedEntries(taggedEntryAnalyser.Analyse(parser.TaggedEntries.Keys));

                this.UpdateErrors(parser, taskEntryAnalyzer);
            }
        }

        private void UpdateTaggedEntries(List<TagTypeViewModel> tagTypes)
        {
            this.TagTypes.Clear();

            this.TagTypes.AddRange(tagTypes.OrderBy(t => t.Name));
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
            this.Errors.Clear();
            foreach (ParserErrorInfo error in parser.Errors)
            {
                this.Errors.Add(
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

                    this.Errors.Add(
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
                if (!taskEntries.TryGetValue(task.TaskCode, out TaskViewModel taskViewModel))
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