namespace Mesa.HUM.Presentation.Stores
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.Input;
    using Mesa.HUM.Presentation.Abstractions;
    using Mesa.HUM.Presentation.Abstractions.Interfaces;
    using Mesa.HUM.Presentation.Stores.Contracts;
    using Mesa.HUM.Presentation.Stores.Interfaces;

    public sealed class NotificationStore : ObservableComponent, INotificationStore
    {
        private static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds ( 3 );

        private readonly IUIDispatcher _dispatcher;

        private readonly ObservableCollection<NotificationModel> _notifications;

        public NotificationStore ( IUIDispatcher dispatcher )
        {
            _dispatcher = dispatcher;
            _notifications = [ ];

            Notifications = new ReadOnlyObservableCollection<NotificationModel> ( _notifications );
            DismissCommand = new RelayCommand<NotificationModel> ( Dismiss );
        }

        public IRelayCommand<NotificationModel> DismissCommand { get; }

        public ReadOnlyObservableCollection<NotificationModel> Notifications { get; }

        public void Notify ( string message , NotificationSeverity severity = NotificationSeverity.Information , TimeSpan? duration = null )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace ( message );

            var notification = new NotificationModel ( message , severity );

            _notifications.Add ( notification );

            var lifetime = duration ?? DefaultDuration;

            if ( lifetime > TimeSpan.Zero )
            {
                // Task.Delay ( ... ).ContinueWith ( ... ) resumes on a thread-pool thread, so the
                // collection change is marshalled back to the UI thread through the dispatcher.
                _ = Task
                    .Delay ( lifetime )
                    .ContinueWith ( _ => _dispatcher.Post ( ( ) => Dismiss ( notification ) ) );
            }
        }

        private void Dismiss ( NotificationModel? notification )
        {
            if ( notification is not null )
            {
                _notifications.Remove ( notification );
            }
        }
    }
}