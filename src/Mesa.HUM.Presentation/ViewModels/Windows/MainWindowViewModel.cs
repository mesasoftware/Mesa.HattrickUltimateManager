namespace Mesa.HUM.Presentation.ViewModels.Windows
{
    using Mesa.HUM.Presentation.ViewModels.Abstractions;
    using Mesa.HUM.Presentation.ViewModels.Abstractions.Interfaces;
    using Mesa.HUM.Presentation.ViewModels.Components.Notifications;
    using Mesa.HUM.Presentation.ViewModels.Pages;

    public sealed class MainWindowViewModel : WindowViewModelBase
    {
        public MainWindowViewModel ( UserProfileAuthorizationViewModel childViewModel , NotificationsPanelViewModel notificationsPanelViewModel )
        {
            NotificationsPanel = notificationsPanelViewModel;

            ChildViewModel = childViewModel;

            if ( ChildViewModel is IInitializableViewModel initializableViewModel )
            {
                initializableViewModel
                    .InitializeAsync ( )
                    .ConfigureAwait ( true )
                    .GetAwaiter ( )
                    .GetResult ( );
            }
        }

        public PageViewModelBase ChildViewModel { get; }

        public NotificationsPanelViewModel NotificationsPanel { get; }
    }
}