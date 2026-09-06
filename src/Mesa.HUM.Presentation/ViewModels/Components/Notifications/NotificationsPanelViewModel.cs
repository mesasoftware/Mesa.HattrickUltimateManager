namespace Mesa.HUM.Presentation.ViewModels.Components.Notifications
{
    using System;
    using CommunityToolkit.Mvvm.Input;
    using Mesa.HUM.Presentation.Stores.Contracts;
    using Mesa.HUM.Presentation.Stores.Interfaces;

    public class NotificationsPanelViewModel
    {
        public NotificationsPanelViewModel ( INotificationsStore store )
        {
            Store = store;

            DismissCommand = new RelayCommand<NotificationModel> ( OnDismiss );
        }

        public IRelayCommand<NotificationModel> DismissCommand { get; }

        public INotificationsStore Store { get; }

        private void OnDismiss ( NotificationModel? notification )
        {
            ArgumentNullException.ThrowIfNull ( notification );

            Store.Dismiss ( notification );
        }
    }
}