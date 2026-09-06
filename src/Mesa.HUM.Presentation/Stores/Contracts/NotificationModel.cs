namespace Mesa.HUM.Presentation.Stores.Contracts
{
    using Mesa.HUM.Presentation.Stores.Enums;

    public sealed record NotificationModel ( string Message , NotificationSeverity Severity );
}