namespace Mesa.HUM.Application.Features.Profiles.Authorization.StartChppAuthorization
{
    using Mesa.HUM.Application.Hattrick.Contracts;

    public record StartChppAuthorizationResponse ( RequestToken RequestToken , string AuthorizationUrl );
}