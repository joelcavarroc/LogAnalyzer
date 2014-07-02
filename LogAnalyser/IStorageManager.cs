namespace LogAnalyzer
{
    using System.IO;

    public interface IStorageManager
    {
        #region Public Methods and Operators

        TextWriter GetStreamWriter(string path, bool append);

        #endregion
    }
}