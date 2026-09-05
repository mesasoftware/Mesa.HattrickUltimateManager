namespace Mesa.HUM.Presentation.ExtensionMethods.HostBuilder
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Mesa.HUM.Presentation.Contracts;
    using Mesa.HUM.Presentation.Facades;
    using Mesa.HUM.Presentation.Facades.Interfaces;
    using Mesa.HUM.Presentation.Stores;
    using Mesa.HUM.Presentation.Stores.Interfaces;
    using Mesa.HUM.Presentation.ViewModels.Pages;
    using Mesa.HUM.Presentation.ViewModels.Windows;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionExtensionMethods
    {
        public static IHostBuilder RegisterPresentationDependencies ( this IHostBuilder hostBuilder )
        {
            return hostBuilder.ConfigureServices ( ( context , services ) =>
                services
                    .RegisterFacades ( )
                    .RegisterScopesSettings ( context.Configuration )
                    .RegisterStores ( )
                    .RegisterViewModels ( ) );
        }

        private static IServiceCollection RegisterFacades ( this IServiceCollection services )
        {
            return services.AddScoped<IAuthorizationFacade , AuthorizationFacade> ( );
        }

        private static IServiceCollection RegisterScopesSettings ( this IServiceCollection services , IConfiguration configuration )
        {
            var scopes = configuration
                .GetSection ( "Chpp:Scopes" )
                .Get<Scope [ ]> ( );

            ArgumentNullException.ThrowIfNull ( scopes );

            return services.AddSingleton ( scopes );
        }

        private static IServiceCollection RegisterStores ( this IServiceCollection services )
        {
            return services
                .AddSingleton<IUserProfileStore , UserProfileStore> ( )
                .AddSingleton<INotificationStore , NotificationStore> ( );
        }

        private static IServiceCollection RegisterViewModels ( this IServiceCollection services )
        {
            return services
                .AddScoped<MainWindowViewModel> ( )
                .AddScoped<UserProfileAuthorizationViewModel> ( );
        }
    }
}