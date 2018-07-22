using LogAnalyzer.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogAnalyzer.Tests.ViewModels
{
    [TestClass()]
    public class MainWindowViewModelTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void SelectedErrorTest()
        {
            ErrorViewModel errorViewModel = new ErrorViewModel();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel { SelectedError = errorViewModel };

            Assert.AreEqual(errorViewModel, mainWindowViewModel.SelectedError);
        }

        [TestMethod]
        public void SaveCommandTest()
        {
            SaveCommand saveCommand = new SaveCommand(new FakeStorageManager());
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel { SaveCommand = saveCommand };

            Assert.AreEqual(saveCommand, mainWindowViewModel.SaveCommand);
        }

        #endregion

        [TestMethod]
        public void WhenNotModifiedModificationIndicatorIsEmptyTest()
        {
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();

            Assert.AreEqual(string.Empty, mainWindowViewModel.ModificationIndicator);
        }

        [TestMethod()]
        public void WhenModifiedModificationIndicatorIsStartTest()
        {
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();

            mainWindowViewModel.LogText = "dummy";

            Assert.AreEqual("*", mainWindowViewModel.ModificationIndicator);
        }

        [TestMethod()]
        public void UpdateWithLogWithOneErrorHasOneErrorTest()
        {
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();

            mainWindowViewModel.LogText = "% 2014-06-07\n" + "% DUMMY 15:00 15:30\n" + "% Dummy 15:30";

            Assert.AreEqual(1, mainWindowViewModel.Errors.Count);
        }

        [TestMethod()]
        public void GetNormalizedDurationTest()
        {
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();

            mainWindowViewModel.LogText = "% 2014-06-07\n" + "% DUMMY 15:00 15:30\n";

            Assert.AreEqual(1.0, mainWindowViewModel.NormalizedTotalDuration, 1e-6);
        }

    }
}