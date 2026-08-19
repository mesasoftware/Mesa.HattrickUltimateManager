namespace Mesa.HUM.Presentation.ExtensionMethods.HostBuilder
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Hosting;

    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder AddPresentationDependencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder;
        }
    }
}