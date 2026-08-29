namespace Mesa.HUM.Infrastructure.Tests.OAuth
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Mesa.HUM.Infrastructure.OAuth;
    using Mesa.HUM.Infrastructure.OAuth.Abstractions.Interfaces;
    using Mesa.HUM.Infrastructure.OAuth.Contracts;
    using Mesa.HUM.Infrastructure.OAuth.Exceptions;
    using Moq;

    public class OAuthClientTests
    {
        private const string accessTokenUrl = "https://chpp.hattrick.org/oauth/access_token.aspx";

        private const string authorizationHeader = "OAuth oauth_consumer_key=\"key\"";

        private const string callBackUrl = "https://localhost/callback";

        private const string checkTokenUrl = "https://chpp.hattrick.org/oauth/check_token.aspx";

        private const string protectedResourcesUrl = "https://chpp.hattrick.org/chppxml.ashx";

        private const string requestTokenUrl = "https://chpp.hattrick.org/oauth/request_token.aspx";

        private const string revokeTokenUrl = "https://chpp.hattrick.org/oauth/invalidate_token.aspx";

        private const string userAuthorizeUrl = "https://chpp.hattrick.org/oauth/authorize.aspx";

        [Fact]
        public void BuildAuthorizationUrl_WhenRequestTokenIsNull_ShouldThrowArgumentNullException ( )
        {
            // Arrange.
            var sut = GetOAuthClient (
                GetHeaderBuilderMock ( ) ,
                GetHttpClientFactoryMock (
                    GetHandler ( "" ) ) );

            // Assert.
            Assert.Throws<ArgumentNullException> ( ( ) => sut.BuildAuthorizationUrl ( null! ) );
        }

        [Fact]
        public void BuildAuthorizationUrl_WhenScopesAreEmpty_ShouldReturnUrlWithoutScope ( )
        {
            // Arrange.
            var sut = GetOAuthClient (
                GetHeaderBuilderMock ( ) ,
                GetHttpClientFactoryMock (
                    GetHandler ( "" ) ) );

            var requestToken = new OAuthToken ( "token" , "secret" );

            // Act.
            string actual = sut.BuildAuthorizationUrl ( requestToken , [ ] );

            // Assert.
            Assert.Equal ( $"{userAuthorizeUrl}?oauth_token=token" , actual );
        }

        [Fact]
        public void BuildAuthorizationUrl_WhenScopesAreNull_ShouldReturnUrlWithoutScope ( )
        {
            // Arrange.
            var sut = GetOAuthClient (
                GetHeaderBuilderMock ( ) ,
                GetHttpClientFactoryMock (
                    GetHandler ( "" ) ) );

            var requestToken = new OAuthToken ( "token" , "secret" );

            // Act.
            string actual = sut.BuildAuthorizationUrl ( requestToken , null );

            // Assert.
            Assert.Equal ( $"{userAuthorizeUrl}?oauth_token=token" , actual );
        }

        [Fact]
        public void BuildAuthorizationUrl_WhenScopesProvided_ShouldAppendEncodedScope ( )
        {
            // Arrange.
            var sut = GetOAuthClient (
                GetHeaderBuilderMock ( ) ,
                GetHttpClientFactoryMock (
                    GetHandler ( "" ) ) );

            var requestToken = new OAuthToken ( "token" , "secret" );

            // Act.
            string actual = sut.BuildAuthorizationUrl ( requestToken , [ "manage_challenges" , "set_training" ] );

            // Assert.
            Assert.Equal ( $"{userAuthorizeUrl}?oauth_token=token&scope=manage_challenges%2Cset_training" , actual );
        }

        [Fact]
        public void BuildAuthorizationUrl_WhenTokenContainsReservedCharacters_ShouldEncodeToken ( )
        {
            // Arrange.
            var sut = GetOAuthClient (
                GetHeaderBuilderMock ( ) ,
                GetHttpClientFactoryMock (
                    GetHandler ( "" ) ) );

            var requestToken = new OAuthToken ( "a b&c" , "secret" );

            // Act.
            string actual = sut.BuildAuthorizationUrl ( requestToken );

            // Assert.
            Assert.Equal ( $"{userAuthorizeUrl}?oauth_token=a%20b%26c" , actual );
        }

        [Fact]
        public async Task ExchangeVerifierForAccessTokenAsync_WhenOAuthTokenMissing_ShouldThrowOAuthException ( )
        {
            // Arrange.
            var handler = GetHandler ( "oauth_token_secret=accessSecret" );

            var sut = GetOAuthClient (
                GetHeaderBuilderMock ( ) ,
                GetHttpClientFactoryMock ( handler ) );

            var requestToken = new OAuthToken ( "token" , "secret" );

            // Assert.
            await Assert.ThrowsAsync<OAuthException> ( ( ) => sut.ExchangeVerifierForAccessTokenAsync ( requestToken , "verifier" , TestContext.Current.CancellationToken ) );
        }

        [Fact]
        public async Task ExchangeVerifierForAccessTokenAsync_WhenRequestTokenIsNull_ShouldThrowArgumentNullException ( )
        {
            // Arrange.
            var sut = GetOAuthClient (
                GetHeaderBuilderMock ( ) ,
                GetHttpClientFactoryMock (
                    GetHandler ( "" ) ) );

            // Assert.
            await Assert.ThrowsAsync<ArgumentNullException> ( ( ) => sut.ExchangeVerifierForAccessTokenAsync ( null! , "verifier" , TestContext.Current.CancellationToken ) );
        }

        [Fact]
        public async Task ExchangeVerifierForAccessTokenAsync_WhenResponseIsValid_ShouldReturnParsedToken ( )
        {
            // Arrange.
            var handler = GetHandler ( "oauth_token=access&oauth_token_secret=accessSecret" );

            var sut = GetOAuthClient (
                GetHeaderBuilderMock ( ) ,
                GetHttpClientFactoryMock ( handler ) );

            var requestToken = new OAuthToken ( "token" , "secret" );

            // Act.
            var actual = await sut.ExchangeVerifierForAccessTokenAsync ( requestToken , "verifier" , TestContext.Current.CancellationToken );

            // Assert.
            Assert.Equal ( "access" , actual.Token );
            Assert.Equal ( "accessSecret" , actual.TokenSecret );
        }

        [Fact]
        public async Task ExchangeVerifierForAccessTokenAsync_WhenResponseIsValid_ShouldSendGetRequestToAccessTokenUrl ( )
        {
            // Arrange.
            var handler = GetHandler ( "oauth_token=access&oauth_token_secret=accessSecret" );

            var sut = GetOAuthClient (
                GetHeaderBuilderMock ( ) ,
                GetHttpClientFactoryMock ( handler ) );

            var requestToken = new OAuthToken ( "token" , "secret" );

            // Act.
            await sut.ExchangeVerifierForAccessTokenAsync ( requestToken , "verifier" , TestContext.Current.CancellationToken );

            // Assert.
            Assert.NotNull ( handler.CapturedRequest );
            Assert.Equal ( HttpMethod.Get , handler.CapturedRequest.Method );
            Assert.Equal ( accessTokenUrl , handler.CapturedRequest.RequestUri!.ToString ( ) );
        }

        [Theory]
        [InlineData ( "" )]
        [InlineData ( " " )]
        public async Task ExchangeVerifierForAccessTokenAsync_WhenVerifierIsEmptyOrWhitespace_ShouldThrowArgumentException ( string verifier )
        {
            // Arrange.
            var sut = GetOAuthClient (
                GetHeaderBuilderMock ( ) ,
                GetHttpClientFactoryMock (
                    GetHandler ( "" ) ) );

            var requestToken = new OAuthToken ( "token" , "secret" );

            // Assert.
            await Assert.ThrowsAsync<ArgumentException> ( ( ) => sut.ExchangeVerifierForAccessTokenAsync ( requestToken , verifier , TestContext.Current.CancellationToken ) );
        }

        [Fact]
        public async Task ExchangeVerifierForAccessTokenAsync_WhenVerifierIsNull_ShouldThrowArgumentNullException ( )
        {
            // Arrange.
            var sut = GetOAuthClient (
                GetHeaderBuilderMock ( ) ,
                GetHttpClientFactoryMock (
                    GetHandler ( "" ) ) );

            var requestToken = new OAuthToken ( "token" , "secret" );

            // Assert.
            await Assert.ThrowsAsync<ArgumentNullException> ( ( ) => sut.ExchangeVerifierForAccessTokenAsync ( requestToken , null! , TestContext.Current.CancellationToken ) );
        }

        [Fact]
        public async Task GetRequestTokenAsync_WhenCancellationRequested_ShouldThrowOperationCanceledException ( )
        {
            // Arrange.
            var handler = GetHandler ( "oauth_token=abc&oauth_token_secret=def" );
            var factory = GetHttpClientFactoryMock ( handler );
            var sut = GetOAuthClient ( GetHeaderBuilderMock ( ) , factory );

            using var cancellationTokenSource = new CancellationTokenSource ( );
            cancellationTokenSource.Cancel ( );

            // Assert.
            await Assert.ThrowsAsync<OperationCanceledException> ( ( ) => sut.GetRequestTokenAsync ( cancellationTokenSource.Token ) );
            Assert.Null ( handler.CapturedRequest );
            factory.Verify ( f => f.CreateClient ( It.IsAny<string> ( ) ) , Times.Never );
        }

        [Fact]
        public async Task GetRequestTokenAsync_WhenOAuthTokenMissing_ShouldThrowOAuthException ( )
        {
            // Arrange.
            var handler = GetHandler ( "oauth_token_secret=def" );
            var sut = GetOAuthClient ( GetHeaderBuilderMock ( ) , GetHttpClientFactoryMock ( handler ) );

            // Assert.
            await Assert.ThrowsAsync<OAuthException> ( ( ) => sut.GetRequestTokenAsync ( TestContext.Current.CancellationToken ) );
        }

        [Fact]
        public async Task GetRequestTokenAsync_WhenOAuthTokenSecretMissing_ShouldThrowOAuthException ( )
        {
            // Arrange.
            var handler = GetHandler ( "oauth_token=abc" );
            var sut = GetOAuthClient ( GetHeaderBuilderMock ( ) , GetHttpClientFactoryMock ( handler ) );

            // Assert.
            await Assert.ThrowsAsync<OAuthException> ( ( ) => sut.GetRequestTokenAsync ( TestContext.Current.CancellationToken ) );
        }

        [Fact]
        public async Task GetRequestTokenAsync_WhenResponseIsValid_ShouldReturnParsedToken ( )
        {
            // Arrange.
            var handler = GetHandler ( "oauth_token=abc&oauth_token_secret=def" );
            var sut = GetOAuthClient ( GetHeaderBuilderMock ( ) , GetHttpClientFactoryMock ( handler ) );

            // Act.
            var actual = await sut.GetRequestTokenAsync ( TestContext.Current.CancellationToken );

            // Assert.
            Assert.Equal ( "abc" , actual.Token );
            Assert.Equal ( "def" , actual.TokenSecret );
        }

        [Fact]
        public async Task GetRequestTokenAsync_WhenResponseIsValid_ShouldSendGetRequestToRequestTokenUrlWithOAuthHeader ( )
        {
            // Arrange.
            var handler = GetHandler ( "oauth_token=abc&oauth_token_secret=def" );
            var sut = GetOAuthClient ( GetHeaderBuilderMock ( ) , GetHttpClientFactoryMock ( handler ) );

            // Act.
            await sut.GetRequestTokenAsync ( TestContext.Current.CancellationToken );

            // Assert.
            Assert.NotNull ( handler.CapturedRequest );
            Assert.Equal ( HttpMethod.Get , handler.CapturedRequest.Method );
            Assert.Equal ( requestTokenUrl , handler.CapturedRequest.RequestUri!.ToString ( ) );
            Assert.Equal ( "OAuth" , handler.CapturedRequest.Headers.Authorization!.Scheme );
        }

        private static ConsumerCredentials GetConsumerCredentials ( )
        {
            return new ConsumerCredentials ( "consumerKey" , "consumerSecret" , "userAgent" );
        }

        private static Endpoints GetEndpoints ( )
        {
            return new Endpoints (
                accessTokenUrl ,
                callBackUrl ,
                checkTokenUrl ,
                protectedResourcesUrl ,
                requestTokenUrl ,
                revokeTokenUrl ,
                userAuthorizeUrl );
        }

        private static StubHttpMessageHandler GetHandler ( string responseBody )
        {
            return new StubHttpMessageHandler (
                new HttpResponseMessage ( HttpStatusCode.OK )
                {
                    Content = new StringContent ( responseBody )
                } );
        }

        private static Mock<IOAuthAuthorizationHeaderBuilder> GetHeaderBuilderMock ( )
        {
            var mock = new Mock<IOAuthAuthorizationHeaderBuilder> ( );

            mock.Setup ( b => b.ForHttpMethod ( It.IsAny<HttpMethod> ( ) ) ).Returns ( mock.Object );
            mock.Setup ( b => b.ForUrl ( It.IsAny<string> ( ) ) ).Returns ( mock.Object );
            mock.Setup ( b => b.WithCallBackUrl ( It.IsAny<string> ( ) ) ).Returns ( mock.Object );
            mock.Setup ( b => b.WithConsumer ( It.IsAny<ConsumerCredentials> ( ) ) ).Returns ( mock.Object );
            mock.Setup ( b => b.WithToken ( It.IsAny<OAuthToken> ( ) ) ).Returns ( mock.Object );
            mock.Setup ( b => b.WithVerifier ( It.IsAny<string> ( ) ) ).Returns ( mock.Object );
            mock.Setup ( b => b.Build ( ) ).Returns ( authorizationHeader );

            return mock;
        }

        private static Mock<IHttpClientFactory> GetHttpClientFactoryMock ( StubHttpMessageHandler handler )
        {
            var httpClient = new HttpClient ( handler );

            var mock = new Mock<IHttpClientFactory> ( );
            mock.Setup ( f => f.CreateClient ( "Chpp" ) ).Returns ( httpClient );

            return mock;
        }

        private static OAuthClient GetOAuthClient (
            Mock<IOAuthAuthorizationHeaderBuilder> headerBuilderMock ,
            Mock<IHttpClientFactory> httpClientFactoryMock )
        {
            return new OAuthClient (
                headerBuilderMock.Object ,
                GetConsumerCredentials ( ) ,
                GetEndpoints ( ) ,
                httpClientFactoryMock.Object );
        }

        private sealed class StubHttpMessageHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _response;

            public StubHttpMessageHandler ( HttpResponseMessage response )
            {
                _response = response;
            }

            public HttpRequestMessage? CapturedRequest { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync ( HttpRequestMessage request , CancellationToken cancellationToken )
            {
                CapturedRequest = request;

                return Task.FromResult ( _response );
            }
        }
    }
}