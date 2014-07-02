// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="JCS">
//   JCSCopyright
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace LogAnalyzer
{
    using System.ComponentModel;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Input;

    using LogAnalyzer.ViewModels;

    /// <summary>
    ///     Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow
    {
        #region Fields

        /// <summary>
        ///     The view model.
        /// </summary>
        private readonly MainWindowViewModel viewModel;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.DataContext = this.viewModel = App.Container.GetExportedValue<MainWindowViewModel>();
        }

        #endregion

        #region Methods

        private void CanExecuteCloseHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        ///     The control_ on mouse double click.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.viewModel.SelectedError != null)
            {
                int line = this.viewModel.SelectedError.Line - 1;
                int firstCharacterIndex = this.LogTextBox.GetCharacterIndexFromLineIndex(line);

                this.LogTextBox.Select(firstCharacterIndex, this.LogTextBox.GetLineLength(line));

                this.LogTextBox.Focus();
                this.LogTextBox.ScrollToLine(line);
            }
        }

        private void TasksGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.TasksGrid.SelectedItem != null)
            {
                TaskViewModel taskViewModel = (TaskViewModel)this.TasksGrid.SelectedItem;
                string regExString = string.Format(@"^%\s+{0}\s+.*$", taskViewModel.TaskCode);

                Regex regex = new Regex(regExString, RegexOptions.Multiline);
                string currentlySelectedText = this.LogTextBox.SelectedText;
                Match match = null;

                // If the current match is selected, then select the next one.
                if (regex.Match(currentlySelectedText).Success)
                {
                    match = regex.Match(
                        this.LogTextBox.Text,
                        this.LogTextBox.SelectionStart + this.LogTextBox.SelectionLength);
                }

                if (match == null || !match.Success)
                {
                    match = regex.Match(this.LogTextBox.Text);
                }

                if (match.Success)
                {
                    int line = this.LogTextBox.GetLineIndexFromCharacterIndex(match.Index);

                    this.LogTextBox.Select(match.Index, this.LogTextBox.GetLineLength(line));

                    this.LogTextBox.Focus();
                    this.LogTextBox.ScrollToLine(line);
                }
            }
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            if (this.viewModel.IsModified)
            {
                MessageBoxResult res = MessageBox.Show(
                    this,
                    "The file is modified, do you want to save it? ",
                    "Closing...",
                    MessageBoxButton.YesNoCancel);
                switch (res)
                {
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.Yes:
                        this.viewModel.SaveCommand.Execute(null);
                        break;
                }
            }
        }

        private void WorkedDaysMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.WorkedDays.SelectedItem != null)
            {
                WorkDayViewModel workDayViewModel = (WorkDayViewModel)this.WorkedDays.SelectedItem;
                string regExString = string.Format(
                    @"^%\s+{0}-{1:D2}-{2:D2}\s*$",
                    workDayViewModel.Date.Year,
                    workDayViewModel.Date.Month,
                    workDayViewModel.Date.Day);

                Regex regex = new Regex(regExString, RegexOptions.Multiline);

                Match match = regex.Match(this.LogTextBox.Text);
                if (match.Success)
                {
                    int line = this.LogTextBox.GetLineIndexFromCharacterIndex(match.Index);

                    this.LogTextBox.Select(match.Index, this.LogTextBox.GetLineLength(line));

                    this.LogTextBox.Focus();
                    this.LogTextBox.ScrollToLine(line);
                }
            }
        }

        #endregion
    }
}