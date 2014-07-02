namespace LogAnalyzer
{
    using System.IO;
    using System.Text;

    class StorageManager : IStorageManager
    {
        public TextWriter GetStreamWriter(string path, bool append)
        {
            return new StreamWriter(path, true, Encoding.UTF8);
        }
    }
}