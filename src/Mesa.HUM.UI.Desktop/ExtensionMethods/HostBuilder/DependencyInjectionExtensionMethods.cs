namespace Mesa.HUM.UI.Desktop.ExtensionMethods.HostBuilder
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Mesa.HUM.Presentation.Abstractions.Interfaces;
    using Mesa.HUM.UI.Desktop.Dispatching;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder RegisterDesktopUIDependencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder
                .RegisterConfiguration ( )
                .RegisterServices ( );
        }

        private static IHostBuilder RegisterConfiguration ( this IHostBuilder hostBuilder )
        {
            return hostBuilder.ConfigureAppConfiguration ( ( context , configurationBuilder ) =>
            {
                configurationBuilder.SetBasePath ( AppDomain.CurrentDomain.BaseDirectory );
                configurationBuilder.AddJsonFile ( "appSettings.json" );

                if ( context.HostingEnvironment.IsDevelopment ( ) )
                {
                    configurationBuilder.AddJsonFile ( "appSettings.development.json" );

                    var assembly = Assembly.GetEntryAssembly ( );

                    ArgumentNullException.ThrowIfNull ( assembly );

                    configurationBuilder.AddUserSecrets ( assembly );
                }
            } );
        }

        private static IHostBuilder RegisterServices ( this IHostBuilder hostBuilder )
        {
            return hostBuilder.ConfigureServices ( ( _ , services ) =>
                services
                    .AddSingleton<ILinkLauncher , SystemLinkLauncher> ( )
                    .AddSingleton<IUIDispatcher , AvaloniaDispatcher> ( ) );
        }
    }
}