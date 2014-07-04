// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadCommand.cs" company="JCS">
//   JCSCopyright
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace LogAnalyzer.ViewModels
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Text;
    using System.Windows.Input;

    /// <summary>
    ///     The load command.
    /// </summary>
    public class LoadCommand : ICommand
    {
        #region Fields

        private readonly IOpenFileDialogService openFileDialogService;

        #endregion

        #region Constructors and Destructors

        public LoadCommand(IOpenFileDialogService openFileDialogService)
        {
            Contract.Requires(openFileDialogService != null);

            this.openFileDialogService = openFileDialogService;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     The can execute changed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        #endregion

        #region Public Properties

        public MainWindowViewModel MainWindowViewModel { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The can execute.
        /// </summary>
        /// <param name="parameter">
        ///     The parameter.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        ///     The execute.
        /// </summary>
        /// <param name="parameter">
        ///     The parameter.
        /// </param>
        public void Execute(object parameter)
        {
            this.openFileDialogService.CheckFileExists = true;
            this.openFileDialogService.Multiselect = false;
            this.openFileDialogService.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (this.openFileDialogService.ShowDialog() == true)
            {
                this.MainWindowViewModel.Filename = this.openFileDialogService.FileName;

                using (Stream stream = this.openFileDialogService.OpenFile())
                using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                {
                    this.MainWindowViewModel.LogText = this.MainWindowViewModel.LastSavedText = streamReader.ReadToEnd();
                }
            }
        }

        #endregion
    }
}