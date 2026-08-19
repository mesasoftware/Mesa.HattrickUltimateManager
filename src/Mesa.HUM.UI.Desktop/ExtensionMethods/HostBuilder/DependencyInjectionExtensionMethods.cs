namespace Mesa.HUM.UI.Desktop.ExtensionMethods.HostBuilder
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder AddDesktopUIDependencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder
                .AddConfiguration ( );
        }

        private static IHostBuilder AddConfiguration ( this IHostBuilder hostBuilder )
        {
            return hostBuilder.ConfigureAppConfiguration ( ( context , configurationBuilder ) =>
            {
                configurationBuilder.SetBasePath ( AppDomain.CurrentDomain.BaseDirectory );
                configurationBuilder.AddJsonFile ( "appSettings.json" );

                if ( context.HostingEnvironment.IsDevelopment ( ) )
                {
                    configurationBuilder.AddJsonFile ( "appSettings.development.json" );
                }
            } );
        }
    }
}