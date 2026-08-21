namespace Mesa.HUM.Infrastructure.ExtensionMethods.HostBuilder
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
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
                    .RegisterDatabase ( context.Configuration ) );
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
    }
}