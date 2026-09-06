namespace Mesa.HUM.Presentation.Stores.Interfaces
{
    using System;
    using System.Collections.ObjectModel;
    using Mesa.HUM.Presentation.Stores.Contracts;
    using Mesa.HUM.Presentation.Stores.Enums;

    public interface INotificationsStore
    {
        ReadOnlyObservableCollection<NotificationModel> Notifications { get; }

        void Dismiss ( NotificationModel notification );

        void Notify ( string message , NotificationSeverity severity , TimeSpan? duration = null );
    }
}