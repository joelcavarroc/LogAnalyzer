// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveCommand.cs" company="JCS">
//   JCSCopyright
// </copyright>
// <summary>
//   Defines the SaveCommand type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LogAnalyzer.ViewModels
{
    using System;
    using System.IO;
    using System.Text;
    using System.Windows.Input;

    /// <summary>
    /// The save command.
    /// </summary>
    public class SaveCommand : ICommand
    {
        /// <summary>
        /// The main window view model.
        /// </summary>
        private readonly MainWindowViewModel mainWindowViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveCommand"/> class.
        /// </summary>
        /// <param name="mainWindowViewModel">
        /// The main window view model.
        /// </param>
        public SaveCommand(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            this.mainWindowViewModel.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "IsModified")
                    {
                        if (this.CanExecuteChanged != null)
                        {
                            this.CanExecuteChanged(this, EventArgs.Empty);
                        }
                    }
                };
        }

        /// <summary>
        /// The can execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return this.mainWindowViewModel.IsModified && !string.IsNullOrEmpty(this.mainWindowViewModel.Filename);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            using (StreamWriter streamWriter = new StreamWriter(this.mainWindowViewModel.Filename, false, Encoding.UTF8))
            {
                streamWriter.Write(this.mainWindowViewModel.LogText);

                this.mainWindowViewModel.LastSavedText = this.mainWindowViewModel.LogText;
            }
        }
    }
}