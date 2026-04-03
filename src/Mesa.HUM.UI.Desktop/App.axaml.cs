namespace Mesa.HUM.UI.Desktop
{
    using System.Linq;
    using Avalonia;
    using Avalonia.Controls.ApplicationLifetimes;
    using Avalonia.Data.Core.Plugins;
    using Avalonia.Markup.Xaml;
    using Mesa.HUM.Application.ExtensionMethods.HostBuilder;
    using Mesa.HUM.Infrastructure.ExtensionMethods.HostBuilder;
    using Mesa.HUM.Presentation.ExtensionMethods.HostBuilder;
    using Mesa.HUM.UI.Desktop.ExtensionMethods.HostBuilder;
    using Mesa.HUM.UI.Desktop.ViewModels;
    using Mesa.HUM.UI.Desktop.Views;
    using Microsoft.Extensions.Hosting;

    public partial class App : Application
    {
        private IHost? _host;

        public override void Initialize ( )
        {
            AvaloniaXamlLoader.Load ( this );
        }

        public override void OnFrameworkInitializationCompleted ( )
        {
            _host = Host.CreateDefaultBuilder ( )
                .AddDesktopUIDepencencies ( )
                .AddPresentationDepencencies ( )
                .AddApplicationDepencencies ( )
                .AddInfrastructureDepencencies ( )
                .Build ( );

            if ( ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop )
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation ( );

                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel ( ) ,
                };

                desktop.Exit += OnExit;
            }

            base.OnFrameworkInitializationCompleted ( );
        }

        private static void DisableAvaloniaDataAnnotationValidation ( )
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove = BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin> ( ).ToArray ( );

            // remove each entry found
            foreach ( var plugin in dataValidationPluginsToRemove )
            {
                BindingPlugins.DataValidators.Remove ( plugin );
            }
        }

        private void OnExit ( object? sender , ControlledApplicationLifetimeExitEventArgs e )
        {
            _host?.Dispose ( );
            _host = null;
        }
    }
}