namespace Mesa.HUM.Application.Features.Profiles.Authorization.StartChppAuthorization
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Mesa.HUM.Application.Hattrick.Abstractions.Interfaces;
    using Mesa.HUM.Domain.Common;

    internal class StartChppAuthorizationCommandHandler : IRequestHandler<StartChppAuthorizationCommand , Result<StartChppAuthorizationResponse>>
    {
        private readonly IChppAuthenticationClient _chppAuthenticationClient;

        public StartChppAuthorizationCommandHandler ( IChppAuthenticationClient chppAuthenticationClient )
        {
            _chppAuthenticationClient = chppAuthenticationClient;
        }

        public async Task<Result<StartChppAuthorizationResponse>> Handle ( StartChppAuthorizationCommand request , CancellationToken cancellationToken )
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested ( );

                var requestToken = await _chppAuthenticationClient.GetRequestTokenAsync ( cancellationToken );

                string authorizationUrl = _chppAuthenticationClient.BuildAuthorizationUrl ( requestToken , request.Scopes );

                var response = new StartChppAuthorizationResponse ( requestToken , authorizationUrl );

                return Result<StartChppAuthorizationResponse>.Success ( response );
            }
            catch ( OperationCanceledException ex )
            {
                return Result<StartChppAuthorizationResponse>.Failure (
                    Error.Failure (
                        "OPERATION_CANCELLED_ERROR" ,
                        ex.Message ) );
            }
            catch ( Exception ex )
            {
                return Result<StartChppAuthorizationResponse>.Failure (
                    Error.Failure (
                        "CHPP_INTEGRATION_ERROR" ,
                        ex.Message ) );
            }
        }
    }
}