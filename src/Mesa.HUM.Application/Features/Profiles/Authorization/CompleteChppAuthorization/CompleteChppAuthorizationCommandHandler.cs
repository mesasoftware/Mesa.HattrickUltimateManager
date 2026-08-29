namespace Mesa.HUM.Application.Features.Profiles.Authorization.CompleteChppAuthorization
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Mesa.HUM.Application.Hattrick.Abstractions.Interfaces;
    using Mesa.HUM.Domain.Common;

    internal class CompleteChppAuthorizationCommandHandler : IRequestHandler<CompleteChppAuthorizationCommand , Result<CompleteChppAuthorizationResponse>>
    {
        private readonly IChppAuthenticationClient _chppAuthenticationClient;

        public CompleteChppAuthorizationCommandHandler ( IChppAuthenticationClient chppAuthenticationClient )
        {
            _chppAuthenticationClient = chppAuthenticationClient;
        }

        public async Task<Result<CompleteChppAuthorizationResponse>> Handle ( CompleteChppAuthorizationCommand request , CancellationToken cancellationToken )
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested ( );

                var response = await _chppAuthenticationClient.ExchangeVerifierForAccessTokenAsync (
                    request.RequestToken ,
                    request.Verifier ,
                    cancellationToken );

                ArgumentNullException.ThrowIfNull ( response );

                return Result<CompleteChppAuthorizationResponse>.Success (
                    new CompleteChppAuthorizationResponse (
                        response ) );
            }
            catch ( OperationCanceledException ex )
            {
                return Result<CompleteChppAuthorizationResponse>.Failure (
                    Error.Failure (
                        "OPERATION_CANCELLED_ERROR" ,
                        ex.Message ) );
            }
            catch ( Exception ex )
            {
                return Result<CompleteChppAuthorizationResponse>.Failure (
                    Error.Failure (
                        "CHPP_INTEGRATION_ERROR" ,
                        ex.Message ) );
            }
        }
    }
}