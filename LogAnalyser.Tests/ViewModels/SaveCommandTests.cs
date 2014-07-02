namespace LogAnalyzer.ViewModels.Tests
{
    using System.Linq;
    using System.Security.Cryptography;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SaveCommandTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void CanExecuteWithUnmodifiedTextAndFileYieldFalseTest()
        {
            SaveCommand saveCommand = new SaveCommand(new FakeStorageManager());
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            saveCommand.MainWindowViewModel = mainWindowViewModel;

            mainWindowViewModel.Filename = "filename";
            mainWindowViewModel.LogText = mainWindowViewModel.LastSavedText = "Same string";

            Assert.IsFalse(saveCommand.CanExecute(null));
        }

        [TestMethod]
        public void CanExecuteWithModifiedTextButNotFileNameYieldFalseTest()
        {
            SaveCommand saveCommand = new SaveCommand(new FakeStorageManager());
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            saveCommand.MainWindowViewModel = mainWindowViewModel;

            mainWindowViewModel.LogText = "Text 1";
            mainWindowViewModel.LastSavedText = "Text 2";

            Assert.IsFalse(saveCommand.CanExecute(null));
        }


        [TestMethod]
        public void CanExecuteWithModifiedTextAndFilenameYieldTrueTest()
        {
            SaveCommand saveCommand = new SaveCommand(new FakeStorageManager());
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            saveCommand.MainWindowViewModel = mainWindowViewModel;

            mainWindowViewModel.Filename = "dummy file";
            mainWindowViewModel.LogText = "Text 1";
            mainWindowViewModel.LastSavedText = "Text 2";

            Assert.IsTrue(saveCommand.CanExecute(null));
        }

        [TestMethod]
        public void ExecuteWithNotCanExecuteShouldSaveNothingTest()
        {
            FakeStorageManager fakeStorageManager = new FakeStorageManager();
            SaveCommand saveCommand = new SaveCommand(fakeStorageManager);
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            saveCommand.MainWindowViewModel = mainWindowViewModel;

            saveCommand.Execute(null);

            Assert.IsFalse(fakeStorageManager.StringWriters.Any());
        }

        [TestMethod]
        public void ExecuteWithWithNoLastSavedTextShouldNoCreateBackupTest()
        {
            FakeStorageManager fakeStorageManager = new FakeStorageManager();
            SaveCommand saveCommand = new SaveCommand(fakeStorageManager);
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            saveCommand.MainWindowViewModel = mainWindowViewModel;
            mainWindowViewModel.Filename = "dummy.txt";
            mainWindowViewModel.LastSavedText = string.Empty;
            mainWindowViewModel.LogText = "Test";

            saveCommand.Execute(null);

            Assert.AreEqual(1, fakeStorageManager.StringWriters.Count);
            Assert.IsTrue(fakeStorageManager.StringWriters.ContainsKey("dummy.txt"));
            Assert.IsFalse(fakeStorageManager.StringWriters.ContainsKey("dummy.txt.bak"));

            Assert.AreEqual("Test", mainWindowViewModel.LastSavedText);
            Assert.AreEqual("Test", fakeStorageManager.StringWriters["dummy.txt"].ToString());
        }

        [TestMethod]
        public void ExecuteWithWithLastSavedTextShouldNoCreateBackupTest()
        {
            FakeStorageManager fakeStorageManager = new FakeStorageManager();
            SaveCommand saveCommand = new SaveCommand(fakeStorageManager);
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            saveCommand.MainWindowViewModel = mainWindowViewModel;
            mainWindowViewModel.Filename = "dummy.txt";
            const string InitialText = "Initial text";
            mainWindowViewModel.LastSavedText = InitialText;
            const string FinalText = "Final Test";
            mainWindowViewModel.LogText = FinalText;

            saveCommand.Execute(null);

            Assert.AreEqual(2, fakeStorageManager.StringWriters.Count);
            Assert.IsTrue(fakeStorageManager.StringWriters.ContainsKey("dummy.txt"), "The command should save the content.");
            Assert.IsTrue(fakeStorageManager.StringWriters.ContainsKey("dummy.txt.bak"), "The command should create a backup of the initial content.");

            Assert.AreEqual(FinalText, mainWindowViewModel.LastSavedText);
            Assert.AreEqual(FinalText, fakeStorageManager.StringWriters["dummy.txt"].ToString());
            Assert.AreEqual(InitialText, fakeStorageManager.StringWriters["dummy.txt.bak"].ToString());
        }


        [TestMethod]
        public void IsModifiedChangesTriggersACanExecuteEventTest()
        {
            FakeStorageManager fakeStorageManager = new FakeStorageManager();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            SaveCommand saveCommand = new SaveCommand(fakeStorageManager) { MainWindowViewModel = mainWindowViewModel };
            bool canExecuteChangedCalled = false;

            saveCommand.CanExecuteChanged += (sender, args) => { canExecuteChangedCalled = true; };
            saveCommand.MainWindowViewModel.LogText = "dummy";

            Assert.IsTrue(canExecuteChangedCalled);
        }
        #endregion
    }
}