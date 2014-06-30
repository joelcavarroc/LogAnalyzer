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

    using LogAnalyzer.ViewModels;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static CompositionContainer container;

        public static CompositionContainer Container
        {
            get
            {
                return container;
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

            AssemblyCatalog cat = new AssemblyCatalog(typeof(App).Assembly, builder);
            container = new CompositionContainer(cat);

            base.OnStartup(e);
        }

        #endregion
    }
}