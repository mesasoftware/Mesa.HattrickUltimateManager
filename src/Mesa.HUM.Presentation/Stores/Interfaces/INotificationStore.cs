namespace Mesa.HUM.Presentation.Stores.Interfaces
{
    using System;
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.Input;
    using Mesa.HUM.Presentation.Stores.Contracts;

    public interface INotificationStore
    {
        IRelayCommand<NotificationModel> DismissCommand { get; }

        ReadOnlyObservableCollection<NotificationModel> Notifications { get; }

        void Notify ( string message , NotificationSeverity severity = NotificationSeverity.Information , TimeSpan? duration = null );
    }
}