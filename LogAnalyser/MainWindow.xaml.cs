// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="JCS">
//   JCSCopyright
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogAnalyzer
{
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;

    using LogAnalyzer.ViewModels;

    using Microsoft.Win32;

    /// <summary>
    ///     Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow
    {
        #region Fields

        /// <summary>
        /// The view model.
        /// </summary>
        private readonly MainWindowViewModel viewModel;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.DataContext = this.viewModel = new MainWindowViewModel();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The control_ on mouse double click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.viewModel.SelectedError != null)
            {
                int line = this.viewModel.SelectedError.Line - 1;
                int firstCharacterIndex = this.LogTextBox.GetCharacterIndexFromLineIndex(line);

                this.LogTextBox.Select(firstCharacterIndex, this.LogTextBox.GetLineLength(line));

                this.LogTextBox.Focus();
                this.LogTextBox.ScrollToLine(line);
            }
        }

        #endregion

        /// <summary>
        /// The on load click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnLoadClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { CheckFileExists = true, Multiselect = false };

            if (openFileDialog.ShowDialog() == true)
            {
                this.viewModel.Filename = openFileDialog.FileName;
                using (Stream stream = openFileDialog.OpenFile())
                using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                {
                    this.viewModel.LogText = this.viewModel.LastSavedText = streamReader.ReadToEnd();
                }
            }
        }
    }
}