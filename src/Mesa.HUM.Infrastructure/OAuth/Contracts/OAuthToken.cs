namespace Mesa.HUM.Infrastructure.OAuth.Contracts
{
    using System;

    public sealed class OAuthToken
    {
        public OAuthToken ( string token , string tokenSecret )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace ( token );
            ArgumentException.ThrowIfNullOrWhiteSpace ( tokenSecret );

            Token = token;
            TokenSecret = tokenSecret;
        }

        public string Token { get; }

        public string TokenSecret { get; }
    }
}