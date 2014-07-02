using LogAnalyzer.Tests.ViewModels;
namespace LogAnalyzer.ViewModels.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class LoadCommandTests
    {
        #region Public Methods and Operators

        [TestMethod()]
        public void CanExecuteTest()
        {
            LoadCommand loadCommand = new LoadCommand(new FakeOpenFileDialogService());

            Assert.IsTrue(loadCommand.CanExecute(null));
        }

        [TestMethod()]
        public void ExecuteWithCancelTest()
        {
            FakeOpenFileDialogService fakeOpenFileDialogService = new FakeOpenFileDialogService();
            LoadCommand loadCommand = new LoadCommand(fakeOpenFileDialogService);
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            loadCommand.MainWindowViewModel = mainWindowViewModel;

            fakeOpenFileDialogService.ShowDialogResult = false;

            loadCommand.Execute(null);

            Assert.AreEqual(null, loadCommand.MainWindowViewModel.Filename);
            Assert.AreEqual(null, loadCommand.MainWindowViewModel.LogText);
            Assert.AreEqual(null, loadCommand.MainWindowViewModel.LastSavedText);
        }

        [TestMethod()]
        public void ExecuteWithOkTest()
        {
            FakeOpenFileDialogService fakeOpenFileDialogService = new FakeOpenFileDialogService();
            LoadCommand loadCommand = new LoadCommand(fakeOpenFileDialogService);
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            loadCommand.MainWindowViewModel = mainWindowViewModel;

            fakeOpenFileDialogService.ShowDialogResult = true;

            loadCommand.Execute(null);

            Assert.AreEqual(fakeOpenFileDialogService.FileName, loadCommand.MainWindowViewModel.Filename);
            Assert.AreEqual(fakeOpenFileDialogService.FileContent, loadCommand.MainWindowViewModel.LogText);
            Assert.AreEqual(fakeOpenFileDialogService.FileContent, loadCommand.MainWindowViewModel.LastSavedText);
        }

        #endregion
    }
}