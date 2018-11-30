namespace LogAnalyzer.Model
{
    public class TaggedEntry
    {
        public TaggedEntry(string tag, string comment)
        {
            this.Tag = tag;
            this.Comment = comment;
        }

        public string Tag { get; private set; }
        public string Comment { get; private set; }
    }
}
