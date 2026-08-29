namespace Mesa.HUM.Infrastructure.OAuth
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Mesa.HUM.Infrastructure.OAuth.Abstractions.Interfaces;
    using Mesa.HUM.Infrastructure.OAuth.Contracts;
    using Mesa.HUM.Infrastructure.OAuth.Exceptions;
    using Mesa.HUM.Infrastructure.OAuth.Interfaces;

    internal class OAuthClient : IOAuthClient
    {
        private readonly IOAuthAuthorizationHeaderBuilder _authorizationHeaderBuilder;

        private readonly ConsumerCredentials _consumerCredentials;

        private readonly Endpoints _endpoints;

        private readonly IHttpClientFactory _httpClientFactory;

        public OAuthClient (
            IOAuthAuthorizationHeaderBuilder authorizationHeaderBuilder ,
            ConsumerCredentials consumerCredentials ,
            Endpoints endpoints ,
            IHttpClientFactory httpClientFactory )
        {
            _authorizationHeaderBuilder = authorizationHeaderBuilder;
            _consumerCredentials = consumerCredentials;
            _endpoints = endpoints;
            _httpClientFactory = httpClientFactory;
        }

        public string BuildAuthorizationUrl ( OAuthToken requestToken , string [ ]? scopes = null )
        {
            ArgumentNullException.ThrowIfNull ( requestToken );

            string url = $"{_endpoints.UserAuthorizeUrl}?oauth_token={Uri.EscapeDataString ( requestToken.Token )}";

            if ( scopes?.Length > 0 )
            {
                url += $"&scope={Uri.EscapeDataString ( string.Join ( ',' , scopes ) )}";
            }

            return url;
        }

        public async Task<OAuthToken> ExchangeVerifierForAccessTokenAsync ( OAuthToken requestToken , string verifier , CancellationToken cancellationToken = default )
        {
            ArgumentNullException.ThrowIfNull ( requestToken );
            ArgumentException.ThrowIfNullOrWhiteSpace ( requestToken.Token );
            ArgumentException.ThrowIfNullOrWhiteSpace ( requestToken.TokenSecret );
            ArgumentException.ThrowIfNullOrWhiteSpace ( verifier );

            string authorizationHeaderValue = _authorizationHeaderBuilder
                .ForHttpMethod ( HttpMethod.Get )
                .ForUrl ( _endpoints.AccessTokenUrl )
                .WithConsumer ( _consumerCredentials )
                .WithToken ( requestToken )
                .WithVerifier ( verifier )
                .Build ( );

            var request = BuildRequest ( HttpMethod.Get , _endpoints.AccessTokenUrl , authorizationHeaderValue );

            string response = await SendRequestAsync ( request , cancellationToken );

            return ParseTokenResponse ( response );
        }

        public async Task<OAuthToken> GetRequestTokenAsync ( CancellationToken cancellationToken = default )
        {
            cancellationToken.ThrowIfCancellationRequested ( );

            string authorizationHeaderValue = _authorizationHeaderBuilder
                .ForHttpMethod ( HttpMethod.Get )
                .ForUrl ( _endpoints.RequestTokenUrl )
                .WithConsumer ( _consumerCredentials )
                .WithCallBackUrl ( _endpoints.CallBackUrl )
                .Build ( );

            var request = BuildRequest ( HttpMethod.Get , _endpoints.RequestTokenUrl , authorizationHeaderValue );

            string response = await SendRequestAsync ( request , cancellationToken );

            return ParseTokenResponse ( response );
        }

        private static HttpRequestMessage BuildRequest ( HttpMethod httpMethod , string url , string authorizationHeaderValue )
        {
            var request = new HttpRequestMessage ( httpMethod , url );

            request.Headers.Authorization = AuthenticationHeaderValue.Parse ( authorizationHeaderValue );

            return request;
        }

        private static OAuthToken ParseTokenResponse ( string body )
        {
            var parameters = body
                .Split ( '&' , StringSplitOptions.RemoveEmptyEntries )
                .Select ( p => p.Split ( '=' , 2 ) )
                .Where ( p => p.Length == 2 )
                .ToDictionary (
                    p => Uri.UnescapeDataString ( p [ 0 ] ) ,
                    p => Uri.UnescapeDataString ( p [ 1 ] ) );

            if ( !parameters.TryGetValue ( "oauth_token" , out string? token ) || string.IsNullOrWhiteSpace ( token ) )
            {
                throw new OAuthException ( $"oauth_token missing in response: {body}" );
            }

#pragma warning disable IDE0046 // Convert to conditional expression
            if ( !parameters.TryGetValue ( "oauth_token_secret" , out string? secret ) || string.IsNullOrWhiteSpace ( secret ) )
            {
                throw new OAuthException ( $"oauth_token_secret missing in response: {body}" );
            }

            return new OAuthToken ( token , secret );
#pragma warning restore IDE0046 // Convert to conditional expression
        }

        private async Task<string> SendRequestAsync ( HttpRequestMessage request , CancellationToken cancellationToken = default )
        {
            cancellationToken.ThrowIfCancellationRequested ( );

            using ( var httpClient = _httpClientFactory.CreateClient ( "Chpp" ) )
            {
                var response = await httpClient.SendAsync ( request , cancellationToken );

                response.EnsureSuccessStatusCode ( );

                return await response.Content.ReadAsStringAsync ( cancellationToken );
            }
        }
    }
}