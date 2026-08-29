namespace Mesa.HUM.Application.Hattrick.Abstractions.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using Mesa.HUM.Application.Hattrick.Contracts;

    public interface IChppAuthenticationClient
    {
        string BuildAuthorizationUrl ( RequestToken requestToken , string [ ]? scopes = null );

        Task<AccessToken> ExchangeVerifierForAccessTokenAsync ( RequestToken requestToken , string verifier , CancellationToken cancellationToken = default );

        Task<RequestToken> GetRequestTokenAsync ( CancellationToken cancellationToken = default );
    }
}