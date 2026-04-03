namespace Mesa.HUM.Application.ExtensionMethods.HostBuilder
{
    using Microsoft.Extensions.Hosting;

    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder AddApplicationDepencencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder;
        }
    }
}
