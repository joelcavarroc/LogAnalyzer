// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveCommand.cs" company="JCS">
//   JCSCopyright
// </copyright>
// <summary>
//   Defines the SaveCommand type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using LogAnalyzer.Properties;

namespace LogAnalyzer.ViewModels
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Windows.Input;

    /// <summary>
    /// The save command.
    /// </summary>
    [UsedImplicitly]
    public class SaveCommand : ICommand
    {
        private readonly IStorageManager storageManager;

        /// <summary>
        /// The main window view model.
        /// </summary>
        private MainWindowViewModel mainWindowViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveCommand"/> class.
        /// </summary>
        /// <param name="storageManager"></param>
        public SaveCommand(IStorageManager storageManager)
        {
            Contract.Requires(storageManager != null);
            this.storageManager = storageManager;
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                return this.mainWindowViewModel;
            }
            set
            {
                this.mainWindowViewModel = value;
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
            if (!this.CanExecute(null))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(this.mainWindowViewModel.LastSavedText))
            {
                using (TextWriter textWriter = storageManager.GetStreamWriter(Path.ChangeExtension(this.mainWindowViewModel.Filename, Path.GetExtension(this.mainWindowViewModel.Filename) + ".bak"), false))
                {
                    textWriter.Write(this.mainWindowViewModel.LastSavedText);
                }
                
            }

            using (TextWriter textWriter = storageManager.GetStreamWriter(this.mainWindowViewModel.Filename, false))
            {
                textWriter.Write(this.mainWindowViewModel.LogText);

                this.mainWindowViewModel.LastSavedText = this.mainWindowViewModel.LogText;
            }
        }
    }
}