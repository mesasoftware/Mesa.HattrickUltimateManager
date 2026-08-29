namespace Mesa.HUM.Infrastructure.OAuth.Abstractions.Interfaces
{
    using System.Net.Http;
    using Mesa.HUM.Infrastructure.OAuth.Contracts;

    internal interface IOAuthAuthorizationHeaderBuilder
    {
        string Build ( );

        IOAuthAuthorizationHeaderBuilder ForHttpMethod ( HttpMethod httpMethod );

        IOAuthAuthorizationHeaderBuilder ForUrl ( string url );

        IOAuthAuthorizationHeaderBuilder WithCallBackUrl ( string callBackUrl );

        IOAuthAuthorizationHeaderBuilder WithConsumer ( ConsumerCredentials consumerCredentials );

        IOAuthAuthorizationHeaderBuilder WithToken ( OAuthToken token );

        IOAuthAuthorizationHeaderBuilder WithVerifier ( string verifier );
    }
}