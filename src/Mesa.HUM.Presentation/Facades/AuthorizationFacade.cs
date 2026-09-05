namespace Mesa.HUM.Presentation.Facades
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Mesa.HUM.Application.Features.Profiles.Authorization.CompleteChppAuthorization;
    using Mesa.HUM.Application.Features.Profiles.Authorization.StartChppAuthorization;
    using Mesa.HUM.Application.Hattrick.Contracts;
    using Mesa.HUM.Domain.Common;
    using Mesa.HUM.Presentation.Facades.Contracts.Authorization;
    using Mesa.HUM.Presentation.Facades.Interfaces;

    internal class AuthorizationFacade : IAuthorizationFacade
    {
        private readonly IMediator _mediator;

        public AuthorizationFacade ( IMediator mediator )
        {
            _mediator = mediator;
        }

        public async Task<Result<GetAccessTokenResultDto>> GetAccessTokenAsync ( RequestTokenDto requestToken , string verifier , CancellationToken cancellationToken = default )
        {
            var request = new CompleteChppAuthorizationCommand (
                new RequestToken (
                    requestToken.Token ,
                    requestToken.TokenSecret ) ,
                verifier );

            var response = await _mediator.Send ( request , cancellationToken );

            if ( response.IsSuccess )
            {
                ArgumentNullException.ThrowIfNull ( response.Value );

                return Result<GetAccessTokenResultDto>.Success (
                    new GetAccessTokenResultDto (
                        new AccessTokenDto (
                            response.Value.AccessToken.Token ,
                            response.Value.AccessToken.TokenSecret ,
                            response.Value.AccessToken.ObtainedAt ,
                            response.Value.AccessToken.ExpiresAt ) ) );
            }
            else
            {
                ArgumentNullException.ThrowIfNull ( response.Error );

                return Result<GetAccessTokenResultDto>.Failure ( response.Error );
            }
        }

        public async Task<Result<GetAuthorizationUrlResultDto>> GetAuthorizationUrlAsync ( string [ ]? scopes , CancellationToken cancellationToken = default )
        {
            var request = new StartChppAuthorizationCommand ( scopes );

            var response = await _mediator.Send ( request , cancellationToken );

            if ( response.IsSuccess )
            {
                ArgumentNullException.ThrowIfNull ( response.Value );

                return Result<GetAuthorizationUrlResultDto>.Success (
                    new GetAuthorizationUrlResultDto (
                        new RequestTokenDto (
                            response.Value.RequestToken.Token ,
                            response.Value.RequestToken.TokenSecret ) ,
                        response.Value.AuthorizationUrl ) );
            }
            else
            {
                ArgumentNullException.ThrowIfNull ( response.Error );

                return Result<GetAuthorizationUrlResultDto>.Failure ( response.Error );
            }
        }
    }
}