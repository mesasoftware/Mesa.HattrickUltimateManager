namespace Mesa.HUM.Application.ExtensionMethods.HostBuilder
{
    using Microsoft.Extensions.Hosting;

    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder AddApplicationDependencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder;
        }
    }
}