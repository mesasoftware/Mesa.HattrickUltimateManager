namespace Mesa.HUM.Infrastructure.ExtensionMethods.HostBuilder
{
    using System.Diagnostics.CodeAnalysis;
    using Mesa.HUM.Infrastructure.Persistence;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    [ExcludeFromCodeCoverage]
    public static class MigrationExtensionMethods
    {
        public static IHost ApplyMigrations ( this IHost host )
        {
            using ( var scope = host.Services.CreateScope ( ) )
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext> ( );

                dbContext.Database.Migrate ( );
            }

            return host;
        }
    }
}