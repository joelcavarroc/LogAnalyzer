// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="JCS">
//   JCSCopyright
// </copyright>
// <summary>
//   Interaction logic for App.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LogAnalyzer
{
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Registration;
    using System.Windows;

    using ViewModels;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static CompositionContainer _container;

        public static CompositionContainer Container
        {
            get
            {
                return App._container;
            }
        }

        #region Methods

        protected override void OnStartup(StartupEventArgs e)
        {
            RegistrationBuilder builder = new RegistrationBuilder();
            builder.ForType<MainWindowViewModel>()
                .Export()
                .ImportProperties<LoadCommand>(info => info.Name == "LoadCommand")
                .ImportProperties<SaveCommand>(info => info.Name == "SaveCommand");
    
            builder.ForType<LoadCommand>()
                .Export()
                .ImportProperties<MainWindowViewModel>(info => info.Name == "MainWindowViewModel");
            
            builder.ForType<SaveCommand>()
                .Export()
                .ImportProperties<MainWindowViewModel>(info => info.Name == "MainWindowViewModel");
            
            builder.ForType<OpenFileDialogService>().Export<IOpenFileDialogService>();
            builder.ForType<StorageManager>().Export<IStorageManager>();

            AssemblyCatalog cat = new AssemblyCatalog(typeof(App).Assembly, builder);
            App._container = new CompositionContainer(cat);

            base.OnStartup(e);
        }

        #endregion
    }
}