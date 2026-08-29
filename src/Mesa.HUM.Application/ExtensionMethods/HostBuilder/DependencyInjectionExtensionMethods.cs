namespace Mesa.HUM.Application.ExtensionMethods.HostBuilder
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using FluentValidation;
    using Mesa.HUM.Application.Behaviors;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder RegisterApplicationDependencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder.ConfigureServices (
                ( context , services ) => services
                    .RegisterMediatR ( )
                    .RegisterValidators ( ) );
        }

        private static IServiceCollection RegisterMediatR ( this IServiceCollection services )
        {
            return services.AddMediatR ( ( c ) =>
            {
                c.RegisterServicesFromAssembly ( Assembly.GetExecutingAssembly ( ) );
                c.AddOpenBehavior ( typeof ( ValidationBehavior<,> ) );
            } );
        }

        private static IServiceCollection RegisterValidators ( this IServiceCollection services )
        {
            return services.AddValidatorsFromAssembly ( Assembly.GetExecutingAssembly ( ) , includeInternalTypes: true );
        }
    }
}