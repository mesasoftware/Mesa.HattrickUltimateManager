namespace Mesa.HUM.Presentation.Stores.Contracts
{
    public sealed class NotificationModel ( string message , NotificationSeverity severity )
    {
        public string Message { get; } = message;

        public NotificationSeverity Severity { get; } = severity;
    }
}