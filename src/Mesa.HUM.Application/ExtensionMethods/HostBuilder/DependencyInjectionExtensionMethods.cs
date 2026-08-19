namespace Mesa.HUM.Application.ExtensionMethods.HostBuilder
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Hosting;

    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder AddApplicationDependencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder;
        }
    }
}