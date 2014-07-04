namespace LogAnalyzer
{
    using System.IO;

    using LogAnalyzer.Annotations;

    using Microsoft.Win32;

    [UsedImplicitly]
    internal class OpenFileDialogService : IOpenFileDialogService
    {
        #region Fields

        private readonly OpenFileDialog openFileDialog = new OpenFileDialog { RestoreDirectory = true };

        #endregion

        #region Public Properties

        public bool CheckFileExists
        {
            get
            {
                return this.openFileDialog.CheckFileExists;
            }
            set
            {
                this.openFileDialog.CheckFileExists = value;
            }
        }

        public string FileName
        {
            get
            {
                return openFileDialog.FileName;
            }
            set
            {
                openFileDialog.FileName = value;
            }
        }

        public bool Multiselect
        {
            get
            {
                return this.openFileDialog.Multiselect;
            }
            set
            {
                this.openFileDialog.Multiselect = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public Stream OpenFile()
        {
            return this.openFileDialog.OpenFile();
        }

        public bool? ShowDialog()
        {
            return this.openFileDialog.ShowDialog();
        }

        public string Filter
        {
            get
            {
                return this.openFileDialog.Filter;
            }
            set
            {
                this.openFileDialog.Filter = value;
            }
        }

        #endregion
    }
}