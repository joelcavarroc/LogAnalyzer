using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Mvvm;

namespace LogAnalyzer.ViewModels
{
    public class TagTypeViewModel : BindableBase
    {
        private string name;

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }

        public ObservableCollection<TaggedEntryViewModel> TaggedEntries { get; set; }
    }
}