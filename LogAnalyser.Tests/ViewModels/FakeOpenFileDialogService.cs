namespace LogAnalyzer.Tests.ViewModels
{
    using System.IO;
    using System.Text;

    internal class FakeOpenFileDialogService : IOpenFileDialogService
    {
        #region Constructors and Destructors

        public FakeOpenFileDialogService()
        {
            this.FileName = "DummyFileName.txt";
            this.FileContent = "Dummy file content";
        }

        #endregion

        #region Public Properties

        public bool CheckFileExists { get; set; }

        public string FileName { get; set; }

        public bool Multiselect { get; set; }

        public bool? ShowDialogResult { get; set; }

        #endregion

        #region Public Methods and Operators

        public Stream OpenFile()
        {
            return new MemoryStream(UTF8Encoding.UTF8.GetBytes(this.FileContent));
        }

        public string Filter { get; set; }

        public string FileContent { get; set; }

        public bool? ShowDialog()
        {
            return this.ShowDialogResult;
        }

        #endregion
    }
}