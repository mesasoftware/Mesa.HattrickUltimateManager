namespace Mesa.HUM.UI.Desktop.ExtensionMethods.HostBuilder
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder RegisterDesktopUIDependencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder
                .RegisterConfiguration ( );
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
    }
}