namespace Mesa.HUM.Infrastructure.ExtensionMethods.HostBuilder
{
    using Microsoft.Extensions.Hosting;

    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder AddInfrastructureDependencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder;
        }
    }
}