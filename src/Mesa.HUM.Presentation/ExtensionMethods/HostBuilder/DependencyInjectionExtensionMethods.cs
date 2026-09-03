namespace Mesa.HUM.Presentation.ExtensionMethods.HostBuilder
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Mesa.HUM.Presentation.Contracts;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder RegisterPresentationDependencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder.ConfigureServices ( ( context , services ) =>
                services.RegisterScopesSettings ( context.Configuration ) );
        }

        private static IServiceCollection RegisterScopesSettings ( this IServiceCollection services , IConfiguration configuration )
        {
            var scopes = configuration
                .GetSection ( "Chpp:Scopes" )
                .Get<Scopes [ ]> ( );

            ArgumentNullException.ThrowIfNull ( scopes );

            return services.AddSingleton ( scopes );
        }
    }
}