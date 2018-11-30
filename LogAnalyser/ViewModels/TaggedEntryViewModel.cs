using Microsoft.Practices.Prism.Mvvm;

namespace LogAnalyzer.ViewModels
{
    public class TaggedEntryViewModel : BindableBase
    {
        private string content;

        public string Content
        {
            get => this.content;
            set => this.SetProperty(ref this.content, value);
        }
    }
}