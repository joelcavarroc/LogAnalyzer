using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LogAnalyzer.Model;
using LogAnalyzer.ViewModels;

namespace LogAnalyzer.Analysing
{
    public class TaggedEntryAnalyser
    {
        public List<TagTypeViewModel> Analyse(IEnumerable<TaggedEntry> taggedEntries)
        {
            IEnumerable<IGrouping<string, string>> entryGroupedByTag = taggedEntries.GroupBy( (t => t.Tag.ToUpper()), (t => t.Comment) );

            return entryGroupedByTag.Select(
                g => new TagTypeViewModel
                {
                    Name = g.Key,
                    TaggedEntries =
                        new ObservableCollection<TaggedEntryViewModel>(
                            g.Select(e => new TaggedEntryViewModel { Content = e }))
                }).ToList();
        }
    }
}