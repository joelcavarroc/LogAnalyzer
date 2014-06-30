// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadCommand.cs" company="JCS">
//   JCSCopyright
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace LogAnalyzer.ViewModels
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Windows.Input;

    /// <summary>
    /// The load command.
    /// </summary>
    public class LoadCommand : ICommand
    {
        private readonly IOpenFileDialogService openFileDialogService;

        //public LoadCommand(IOpenFileDialogService openFileDialogService, IStorageManager storageManager)
        //{
        //    if (openFileDialogService == null)
        //    {
        //        throw new ArgumentNullException("openFileDialogService");
        //    }
        //    this.openFileDialogService = openFileDialogService;
        //}

        #region Public Events

        /// <summary>
        /// The can execute changed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        #endregion

        #region Public Methods and Operators

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
            return true;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public void Execute(object parameter)
        {
            
        }

        #endregion
    }

    public interface IOpenFileDialogService
    {
        bool ShowDialog();
        string Filename { get; set; }
        bool CheckFileExist { get; set; }
    }
}