namespace Mesa.HUM.Application.ExtensionMethods.HostBuilder
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder RegisterApplicationDependencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder.ConfigureServices (
                ( context , services ) => services
                    .RegisterMediatR ( ) );
        }

        private static IServiceCollection RegisterMediatR ( this IServiceCollection services )
        {
            return services.AddMediatR ( ( c ) => c.RegisterServicesFromAssembly ( Assembly.GetExecutingAssembly ( ) ) );
        }
    }
}