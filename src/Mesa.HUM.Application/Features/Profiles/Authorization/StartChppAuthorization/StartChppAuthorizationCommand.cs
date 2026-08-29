namespace Mesa.HUM.Application.Features.Profiles.Authorization.StartChppAuthorization
{
    using MediatR;
    using Mesa.HUM.Domain.Common;

    public record StartChppAuthorizationCommand ( string [ ]? Scopes = null ) : IRequest<Result<StartChppAuthorizationResponse>>;
}