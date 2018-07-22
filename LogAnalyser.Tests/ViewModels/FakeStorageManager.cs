using System.Collections.Generic;
using System.IO;

namespace LogAnalyzer.Tests.ViewModels
{
    public class FakeStorageManager : IStorageManager
    {
        private readonly Dictionary<string, StringWriter> stringWriters = new Dictionary<string, StringWriter>();

        public Dictionary<string, StringWriter> StringWriters
        {
            get
            {
                return this.stringWriters;
            }
        }

        public TextWriter GetStreamWriter(string path, bool append)
        {
            StringWriter stringWriter = new StringWriter();

            this.StringWriters[path] = stringWriter;

            return stringWriter;
        }
    }
}