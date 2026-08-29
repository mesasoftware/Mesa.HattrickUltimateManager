namespace Mesa.HUM.Infrastructure.OAuth.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Mesa.HUM.Infrastructure.ExtensionMethods.Uri;
    using Mesa.HUM.Infrastructure.OAuth.Abstractions.Interfaces;
    using Mesa.HUM.Infrastructure.OAuth.Contracts;

    internal class OAuthAuthorizationHeaderBuilder : IOAuthAuthorizationHeaderBuilder
    {
        private const string defaultSignatureMethod = "HMAC-SHA1";

        private const string defaultVersion = "1.0";

        private readonly INonceProvider _nonceProvider;

        private readonly IOAuthSigner _oauthSigner;

        private readonly IOAuthSignatureBaseGenerator _signatureBaseGenerator;

        private readonly ITimeStampProvider _timeStampProvider;

        private string? _callBackUrl;

        private ConsumerCredentials? _consumerCredentials;

        private HttpMethod? _httpMethod;

        private string? _signatureMethod;

        private OAuthToken? _token;

        private string? _url;

        private string? _verifier;

        private string? _version;

        public OAuthAuthorizationHeaderBuilder (
            INonceProvider nonceProvider ,
            IOAuthSigner oauthSigner ,
            IOAuthSignatureBaseGenerator signatureBaseGenerator ,
            ITimeStampProvider timeStampProvider )
        {
            _nonceProvider = nonceProvider;
            _oauthSigner = oauthSigner;
            _signatureBaseGenerator = signatureBaseGenerator;
            _timeStampProvider = timeStampProvider;
        }

        public string Build ( )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace ( _url );
            ArgumentNullException.ThrowIfNull ( _consumerCredentials );

            _httpMethod ??= HttpMethod.Get;
            _signatureMethod ??= defaultSignatureMethod;
            _version ??= defaultVersion;

            var oauthParameters = new Dictionary<string , string>
            {
                [ "oauth_consumer_key" ] = _consumerCredentials.ConsumerKey ,
                [ "oauth_nonce" ] = _nonceProvider.GenerateNonce ( ) ,
                [ "oauth_signature_method" ] = _signatureMethod ,
                [ "oauth_timestamp" ] = _timeStampProvider.GetTimeStamp ( ) ,
                [ "oauth_version" ] = _version
            };

            if ( _token is not null )
            {
                oauthParameters [ "oauth_token" ] = _token.Token;
            }

            if ( !string.IsNullOrWhiteSpace ( _verifier ) )
            {
                oauthParameters [ "oauth_verifier" ] = _verifier;
            }

            if ( !string.IsNullOrWhiteSpace ( _callBackUrl ) )
            {
                oauthParameters [ "oauth_callback" ] = _callBackUrl;
            }

            var uri = new Uri ( _url );

            var allParameters = new Dictionary<string , string> ( oauthParameters );
            var additionalParameters = uri.GetQueryParameters ( );

            foreach ( var (key , value) in additionalParameters )
            {
                allParameters [ key ] = value;
            }

            string signatureBase = _signatureBaseGenerator.GenerateSignatureBase ( _httpMethod , uri , allParameters );

            string signature = _oauthSigner.GenerateSignature (
                signatureBase ,
                _consumerCredentials.ConsumerSecret ,
                _token?.TokenSecret );

            oauthParameters [ "oauth_signature" ] = signature;

            var headerParts = oauthParameters
                .OrderBy ( p => p.Key )
                .Select ( p => $"{Uri.EscapeDataString ( p.Key )}=\"{Uri.EscapeDataString ( p.Value )}\"" );

            ClearState ( );

            return "OAuth " + string.Join ( ", " , headerParts );
        }

        public IOAuthAuthorizationHeaderBuilder ForHttpMethod ( HttpMethod httpMethod )
        {
            if ( _httpMethod is not null )
            {
                throw new InvalidOperationException ( "HTTP_METHOD_OAUTH_PARAMETER_ALREADY_SET" );
            }

            _httpMethod = httpMethod;

            return this;
        }

        public IOAuthAuthorizationHeaderBuilder ForUrl ( string url )
        {
            if ( !string.IsNullOrWhiteSpace ( _url ) )
            {
                throw new InvalidOperationException ( "URL_OAUTH_PARAMETER_ALREADY_SET" );
            }

            _url = url;

            return this;
        }

        public IOAuthAuthorizationHeaderBuilder WithCallBackUrl ( string callBackUrl )
        {
            if ( !string.IsNullOrWhiteSpace ( _callBackUrl ) )
            {
                throw new InvalidOperationException ( "CALLBACK_URL_OAUTH_PARAMETER_ALREADY_SET" );
            }

            _callBackUrl = callBackUrl;

            return this;
        }

        public IOAuthAuthorizationHeaderBuilder WithConsumer ( ConsumerCredentials consumerCredentials )
        {
            if ( _consumerCredentials is not null )
            {
                throw new InvalidOperationException ( "CONSUMER_CREDENTIALS_OAUTH_PARAMETER_ALREADY_SET" );
            }

            _consumerCredentials = consumerCredentials;

            return this;
        }

        public IOAuthAuthorizationHeaderBuilder WithSignatureMethod ( string signatureMethod )
        {
            if ( !string.IsNullOrWhiteSpace ( _signatureMethod ) )
            {
                throw new InvalidOperationException ( "SIGNATURE_METHOD_OAUTH_PARAMETER_ALREADY_SET" );
            }

            _signatureMethod = signatureMethod;

            return this;
        }

        public IOAuthAuthorizationHeaderBuilder WithToken ( OAuthToken token )
        {
            if ( _token is not null )
            {
                throw new InvalidOperationException ( "TOKEN_OAUTH_PARAMETER_ALREADY_SET" );
            }

            _token = token;

            return this;
        }

        public IOAuthAuthorizationHeaderBuilder WithVerifier ( string verifier )
        {
            if ( !string.IsNullOrWhiteSpace ( _verifier ) )
            {
                throw new InvalidOperationException ( "VERIFIER_OAUTH_PARAMETER_ALREADY_SET" );
            }

            _verifier = verifier;

            return this;
        }

        public IOAuthAuthorizationHeaderBuilder WithVersion ( string version )
        {
            if ( !string.IsNullOrWhiteSpace ( _version ) )
            {
                throw new InvalidOperationException ( "VERSION_OAUTH_PARAMETER_ALREADY_SET" );
            }

            _version = version;

            return this;
        }

        private void ClearState ( )
        {
            _consumerCredentials = null;
            _httpMethod = null;
            _token = null;

            _callBackUrl =
            _signatureMethod =
            _url =
            _verifier =
            _version = null;
        }
    }
}