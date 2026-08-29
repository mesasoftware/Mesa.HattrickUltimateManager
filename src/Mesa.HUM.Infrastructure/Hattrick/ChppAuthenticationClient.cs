namespace Mesa.HUM.Infrastructure.Hattrick
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Mesa.HUM.Application.Hattrick.Abstractions.Interfaces;
    using Mesa.HUM.Application.Hattrick.Contracts;
    using Mesa.HUM.Infrastructure.OAuth.Contracts;
    using Mesa.HUM.Infrastructure.OAuth.Interfaces;

    internal class ChppAuthenticationClient : IChppAuthenticationClient
    {
        private readonly IOAuthClient _oauthClient;

        public ChppAuthenticationClient ( IOAuthClient oauthClient )
        {
            _oauthClient = oauthClient;
        }

        public string BuildAuthorizationUrl ( RequestToken requestToken , string [ ]? scopes = null )
        {
            return _oauthClient.BuildAuthorizationUrl ( ConvertToOAuthToken ( requestToken ) , scopes );
        }

        public async Task<AccessToken> ExchangeVerifierForAccessTokenAsync ( RequestToken requestToken , string verifier , CancellationToken cancellationToken = default )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace ( requestToken.Token );
            ArgumentException.ThrowIfNullOrWhiteSpace ( requestToken.TokenSecret );
            ArgumentException.ThrowIfNullOrWhiteSpace ( verifier );

            cancellationToken.ThrowIfCancellationRequested ( );

            var token = await _oauthClient.ExchangeVerifierForAccessTokenAsync (
                ConvertToOAuthToken ( requestToken ) ,
                verifier ,
                cancellationToken );

            // Using the permanent authorization URL, so Access Tokens do not expire.
            return new AccessToken ( token.Token , token.TokenSecret , DateTimeOffset.UtcNow , DateTimeOffset.MaxValue );
        }

        public async Task<RequestToken> GetRequestTokenAsync ( CancellationToken cancellationToken = default )
        {
            cancellationToken.ThrowIfCancellationRequested ( );

            var token = await _oauthClient.GetRequestTokenAsync ( cancellationToken );

            return new RequestToken ( token.Token , token.TokenSecret );
        }

        private static OAuthToken ConvertToOAuthToken ( RequestToken requestToken )
        {
            return new OAuthToken ( requestToken.Token , requestToken.TokenSecret );
        }

        private static OAuthToken ConvertToOAuthToken ( AccessToken accessToken )
        {
            return new OAuthToken ( accessToken.Token , accessToken.TokenSecret );
        }
    }
}