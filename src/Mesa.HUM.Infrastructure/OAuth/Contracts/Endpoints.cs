namespace Mesa.HUM.Infrastructure.OAuth.Contracts
{
    using System;

    public sealed class Endpoints
    {
        public Endpoints (
            string accessTokenUrl ,
            string callBackUrl ,
            string checkTokenUrl ,
            string protectedResourceUrl ,
            string requestTokenUrl ,
            string revokeTokenUrl ,
            string userAuthorizeUrl )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace ( accessTokenUrl );
            ArgumentException.ThrowIfNullOrWhiteSpace ( callBackUrl );
            ArgumentException.ThrowIfNullOrWhiteSpace ( checkTokenUrl );
            ArgumentException.ThrowIfNullOrWhiteSpace ( protectedResourceUrl );
            ArgumentException.ThrowIfNullOrWhiteSpace ( requestTokenUrl );
            ArgumentException.ThrowIfNullOrWhiteSpace ( revokeTokenUrl );
            ArgumentException.ThrowIfNullOrWhiteSpace ( userAuthorizeUrl );

            AccessTokenUrl = accessTokenUrl;
            CallBackUrl = callBackUrl;
            CheckTokenUrl = checkTokenUrl;
            ProtectedResourceUrl = protectedResourceUrl;
            RequestTokenUrl = requestTokenUrl;
            RevokeTokenUrl = revokeTokenUrl;
            UserAuthorizeUrl = userAuthorizeUrl;
        }

        public string AccessTokenUrl { get; }

        public string CallBackUrl { get; }

        public string CheckTokenUrl { get; }

        public string ProtectedResourceUrl { get; }

        public string RequestTokenUrl { get; }

        public string RevokeTokenUrl { get; }

        public string UserAuthorizeUrl { get; }
    }
}