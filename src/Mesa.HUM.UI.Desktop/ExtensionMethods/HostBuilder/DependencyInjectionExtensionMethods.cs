namespace Mesa.HUM.UI.Desktop.ExtensionMethods.HostBuilder
{
    using Microsoft.Extensions.Hosting;

    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder AddDesktopUIDependencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder;
        }
    }
}