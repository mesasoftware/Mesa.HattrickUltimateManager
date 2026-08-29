namespace Mesa.HUM.Application.Hattrick.Contracts
{
    using System;

    public record AccessToken ( string Token , string TokenSecret , DateTimeOffset ObtainedAt , DateTimeOffset ExpiresAt );
}