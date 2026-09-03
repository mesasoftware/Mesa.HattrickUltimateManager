namespace Mesa.HUM.Presentation.Stores.Contracts
{
    using System;

    public sealed record UserProfileDto ( Guid Id , DateTime? SynchronizedAt , bool IsAuthorized );
}