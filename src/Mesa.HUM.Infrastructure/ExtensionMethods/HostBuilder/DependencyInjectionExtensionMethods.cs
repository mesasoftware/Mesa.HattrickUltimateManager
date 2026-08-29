namespace Mesa.HUM.Infrastructure.ExtensionMethods.HostBuilder
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Mesa.HUM.Application.Hattrick.Abstractions.Interfaces;
    using Mesa.HUM.Infrastructure.Hattrick;
    using Mesa.HUM.Infrastructure.OAuth;
    using Mesa.HUM.Infrastructure.OAuth.Abstractions;
    using Mesa.HUM.Infrastructure.OAuth.Abstractions.Interfaces;
    using Mesa.HUM.Infrastructure.OAuth.Contracts;
    using Mesa.HUM.Infrastructure.OAuth.Interfaces;
    using Mesa.HUM.Infrastructure.Persistence;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder RegisterInfrastructureDependencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder.ConfigureServices (
                ( context , services ) => services
                    .RegisterChppClients ( )
                    .RegisterConsumerCredentialsSettings ( context.Configuration )
                    .RegisterDatabase ( context.Configuration )
                    .RegisterEndpointsSettings ( context.Configuration )
                    .RegisterHttpClient ( )
                    .RegisterOAuthServices ( ) );
        }

        private static IServiceCollection RegisterChppClients ( this IServiceCollection services )
        {
            return services
                .AddTransient<IChppAuthenticationClient , ChppAuthenticationClient> ( );
        }

        private static IServiceCollection RegisterConsumerCredentialsSettings (
            this IServiceCollection services ,
            IConfiguration configuration )
        {
            var credentials = configuration
                .GetSection ( "Chpp:Credentials" )
                .Get<ConsumerCredentials> ( );

            ArgumentNullException.ThrowIfNull ( credentials );

            return services.AddSingleton ( credentials );
        }

        private static IServiceCollection RegisterDatabase ( this IServiceCollection services , IConfiguration configuration )
        {
            string? connectionStringSetting = configuration.GetConnectionString ( "Database" );

            ArgumentException.ThrowIfNullOrWhiteSpace ( connectionStringSetting );
#if DEBUG
            string path = Path.Combine (
                Environment.GetFolderPath ( Environment.SpecialFolder.MyDocuments ) ,
                "HUM" ,
                "Dev" );
#else
            string path = Path.Combine (
                Environment.GetFolderPath ( Environment.SpecialFolder.MyDocuments ) ,
                "HUM" );
#endif
            if ( !Directory.Exists ( path ) )
            {
                Directory.CreateDirectory ( path );
            }

            string connectionString = string.Format ( connectionStringSetting , path );

            return services.AddDbContext<AppDbContext> (
                options => options
                    .UseSnakeCaseNamingConvention ( )
                    .UseSqlite ( connectionString ) );
        }

        private static IServiceCollection RegisterEndpointsSettings ( this IServiceCollection services , IConfiguration configuration )
        {
            var endpoints = configuration
                .GetSection ( "Chpp:Endpoints" )
                .Get<Endpoints> ( );

            ArgumentNullException.ThrowIfNull ( endpoints );

            return services.AddSingleton ( endpoints );
        }

        private static IServiceCollection RegisterHttpClient ( this IServiceCollection services )
        {
            services.AddHttpClient ( "Chpp" , ( sp , client ) =>
            {
                var credentials = sp.GetRequiredService<ConsumerCredentials> ( );

                client.DefaultRequestHeaders.UserAgent.ParseAdd ( credentials.UserAgent );
            } );

            return services;
        }

        private static IServiceCollection RegisterOAuthServices ( this IServiceCollection services )
        {
            return services
                .AddTransient<INonceProvider , NonceProvider> ( )
                .AddTransient<IOAuthAuthorizationHeaderBuilder , OAuthAuthorizationHeaderBuilder> ( )
                .AddTransient<IOAuthClient , OAuthClient> ( )
                .AddTransient<IOAuthSignatureBaseGenerator , OAuthSignatureBaseGenerator> ( )
                .AddTransient<IOAuthSigner , OAuthSigner> ( )
                .AddTransient<ITimeStampProvider , TimeStampProvider> ( );
        }
    }
}