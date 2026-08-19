namespace Mesa.HUM.Infrastructure.ExtensionMethods.HostBuilder
{
    using Microsoft.Extensions.Hosting;

    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder AddInfrastructureDepencencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder;
        }
    }
}