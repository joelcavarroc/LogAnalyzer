// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParserTests.cs" company="JCS">
//   copyright
// </copyright>
// <summary>
//   The parser tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

namespace LogAnalyzer.Tests.ViewModels
{
    public class FakeStorageManager : IStorageManager
    {
        public Dictionary<string, StringWriter> StringWriters { get; } = new Dictionary<string, StringWriter>();

        public TextWriter GetStreamWriter(string path, bool append)
        {
            StringWriter stringWriter = new StringWriter();

            this.StringWriters[path] = stringWriter;

            return stringWriter;
        }
    }
}