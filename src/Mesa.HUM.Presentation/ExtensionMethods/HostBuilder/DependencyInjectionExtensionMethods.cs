namespace Mesa.HUM.Presentation.ExtensionMethods.HostBuilder
{
    using Microsoft.Extensions.Hosting;

    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder AddPresentationDepencencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder;
        }
    }
}
