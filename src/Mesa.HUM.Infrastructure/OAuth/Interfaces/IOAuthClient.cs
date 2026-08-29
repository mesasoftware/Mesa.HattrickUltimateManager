namespace Mesa.HUM.Infrastructure.OAuth.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using Mesa.HUM.Infrastructure.OAuth.Contracts;

    internal interface IOAuthClient
    {
        string BuildAuthorizationUrl ( OAuthToken requestToken , string [ ]? scopes = null );

        Task<OAuthToken> ExchangeVerifierForAccessTokenAsync ( OAuthToken requestToken , string verifier , CancellationToken cancellationToken = default );

        Task<OAuthToken> GetRequestTokenAsync ( CancellationToken cancellationToken = default );
    }
}