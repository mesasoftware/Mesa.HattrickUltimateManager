namespace Mesa.HUM.Application.Features.Profiles.Authorization.CompleteChppAuthorization
{
    using MediatR;
    using Mesa.HUM.Application.Hattrick.Contracts;
    using Mesa.HUM.Domain.Common;

    public record CompleteChppAuthorizationCommand ( RequestToken RequestToken , string Verifier ) : IRequest<Result<CompleteChppAuthorizationResponse>>;
}