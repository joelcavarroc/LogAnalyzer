namespace LogAnalyzer
{
    using System.IO;
    using System.Text;

    using LogAnalyzer.Annotations;

    [UsedImplicitly]
    class StorageManager : IStorageManager
    {
        public TextWriter GetStreamWriter(string path, bool append)
        {
            return new StreamWriter(path, append, Encoding.UTF8);
        }
    }
}