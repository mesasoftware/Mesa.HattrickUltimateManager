namespace Mesa.HUM.UI.Desktop
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Controls.ApplicationLifetimes;
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
            if ( Design.IsDesignMode )
            {
                base.OnFrameworkInitializationCompleted ( );

                return;
            }

            _host = Host.CreateDefaultBuilder ( )
                .RegisterDesktopUIDependencies ( )
                .RegisterPresentationDependencies ( )
                .RegisterApplicationDependencies ( )
                .RegisterInfrastructureDependencies ( )
                .Build ( );

            _host.ApplyMigrations ( );

            _host.Start ( );

            if ( ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop )
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel ( ) ,
                };

                desktop.Exit += OnExit;
            }

            base.OnFrameworkInitializationCompleted ( );
        }

        private void OnExit ( object? sender , ControlledApplicationLifetimeExitEventArgs e )
        {
            _host?.Dispose ( );
            _host = null;
        }
    }
}