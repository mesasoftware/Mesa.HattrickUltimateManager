namespace Mesa.HUM.Presentation.Facades.Contracts.Authorization
{
    using System;

    public record AccessTokenDto ( string Token , string TokenSecret , DateTimeOffset ObtainedAt , DateTimeOffset ExpiresAt );
}