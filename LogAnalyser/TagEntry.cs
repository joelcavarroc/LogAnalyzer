namespace LogAnalyzer
{
    class TagEntry
    {
        public TagEntry(string tag, string comment)
        {
            this.Tag = tag;
            this.Comment = comment;
        }

        public string Tag { get; private set; }
        public string Comment { get; private set; }
    }
}
