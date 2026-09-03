namespace Mesa.HUM.Infrastructure.Tests.Hattrick
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Mesa.HUM.Application.Hattrick.Contracts;
    using Mesa.HUM.Infrastructure.Hattrick;
    using Mesa.HUM.Infrastructure.OAuth.Contracts;
    using Mesa.HUM.Infrastructure.OAuth.Interfaces;
    using Moq;

    public class ChppAuthenticationClientTests
    {
        [Fact]
        public void BuildAuthorizationUrl_WhenCalled_ShouldConvertRequestTokenAndDelegateToOAuthClient ( )
        {
            // Arrange.
            var oauthClientMock = new Mock<IOAuthClient> ( );
            oauthClientMock
                .Setup ( c => c.BuildAuthorizationUrl ( It.IsAny<OAuthToken> ( ) , It.IsAny<string [ ]?> ( ) ) )
                .Returns ( "https://authorize" );

            var sut = new ChppAuthenticationClient ( oauthClientMock.Object );
            var requestToken = new RequestToken ( "token" , "secret" );

            // Act.
            string actual = sut.BuildAuthorizationUrl ( requestToken );

            // Assert.
            Assert.Equal ( "https://authorize" , actual );
            oauthClientMock.Verify (
                c => c.BuildAuthorizationUrl (
                    It.Is<OAuthToken> ( t => t.Token == "token" && t.TokenSecret == "secret" ) ,
                    null ) ,
                Times.Once );
        }

        [Fact]
        public void BuildAuthorizationUrl_WhenScopesProvided_ShouldForwardScopesToOAuthClient ( )
        {
            // Arrange.
            string [ ] scopes = [ "manage_challenges" , "set_training" ];

            var oauthClientMock = new Mock<IOAuthClient> ( );
            oauthClientMock
                .Setup ( c => c.BuildAuthorizationUrl ( It.IsAny<OAuthToken> ( ) , It.IsAny<string [ ]?> ( ) ) )
                .Returns ( "https://authorize" );

            var sut = new ChppAuthenticationClient ( oauthClientMock.Object );
            var requestToken = new RequestToken ( "token" , "secret" );

            // Act.
            sut.BuildAuthorizationUrl ( requestToken , scopes );

            // Assert.
            oauthClientMock.Verify (
                c => c.BuildAuthorizationUrl ( It.IsAny<OAuthToken> ( ) , scopes ) ,
                Times.Once );
        }

        [Fact]
        public async Task ExchangeVerifierForAccessTokenAsync_WhenCalled_ShouldConvertRequestTokenAndForwardVerifier ( )
        {
            // Arrange.
            var oauthClientMock = new Mock<IOAuthClient> ( );
            oauthClientMock
                .Setup ( c => c.ExchangeVerifierForAccessTokenAsync ( It.IsAny<OAuthToken> ( ) , It.IsAny<string> ( ) , It.IsAny<CancellationToken> ( ) ) )
                .ReturnsAsync ( new OAuthToken ( "access" , "accessSecret" ) );

            var sut = new ChppAuthenticationClient ( oauthClientMock.Object );
            var requestToken = new RequestToken ( "token" , "secret" );

            // Act.
            await sut.ExchangeVerifierForAccessTokenAsync ( requestToken , "verifier" , TestContext.Current.CancellationToken );

            // Assert.
            oauthClientMock.Verify (
                c => c.ExchangeVerifierForAccessTokenAsync (
                    It.Is<OAuthToken> ( t => t.Token == "token" && t.TokenSecret == "secret" ) ,
                    "verifier" ,
                    It.IsAny<CancellationToken> ( ) ) ,
                Times.Once );
        }

        [Fact]
        public async Task ExchangeVerifierForAccessTokenAsync_WhenCancellationRequested_ShouldThrowOperationCanceledExceptionAndNotCallOAuthClient ( )
        {
            // Arrange.
            var oauthClientMock = new Mock<IOAuthClient> ( );

            var sut = new ChppAuthenticationClient ( oauthClientMock.Object );
            var requestToken = new RequestToken ( "token" , "secret" );

            using var cancellationTokenSource = new CancellationTokenSource ( );
            cancellationTokenSource.Cancel ( );

            // Assert.
            await Assert.ThrowsAsync<OperationCanceledException> ( ( ) => sut.ExchangeVerifierForAccessTokenAsync ( requestToken , "verifier" , cancellationTokenSource.Token ) );
            oauthClientMock.Verify (
                c => c.ExchangeVerifierForAccessTokenAsync ( It.IsAny<OAuthToken> ( ) , It.IsAny<string> ( ) , It.IsAny<CancellationToken> ( ) ) ,
                Times.Never );
        }

        [Fact]
        public async Task ExchangeVerifierForAccessTokenAsync_WhenResponseIsValid_ShouldReturnAccessTokenWithOAuthClientValues ( )
        {
            // Arrange.
            var oauthClientMock = new Mock<IOAuthClient> ( );
            oauthClientMock
                .Setup ( c => c.ExchangeVerifierForAccessTokenAsync ( It.IsAny<OAuthToken> ( ) , It.IsAny<string> ( ) , It.IsAny<CancellationToken> ( ) ) )
                .ReturnsAsync ( new OAuthToken ( "access" , "accessSecret" ) );

            var sut = new ChppAuthenticationClient ( oauthClientMock.Object );
            var requestToken = new RequestToken ( "token" , "secret" );

            // Act.
            var actual = await sut.ExchangeVerifierForAccessTokenAsync ( requestToken , "verifier" , TestContext.Current.CancellationToken );

            // Assert.
            Assert.Equal ( "access" , actual.Token );
            Assert.Equal ( "accessSecret" , actual.TokenSecret );
        }

        [Fact]
        public async Task ExchangeVerifierForAccessTokenAsync_WhenResponseIsValid_ShouldReturnNonExpiringAccessToken ( )
        {
            // Arrange.
            var before = DateTimeOffset.UtcNow;

            var oauthClientMock = new Mock<IOAuthClient> ( );
            oauthClientMock
                .Setup ( c => c.ExchangeVerifierForAccessTokenAsync ( It.IsAny<OAuthToken> ( ) , It.IsAny<string> ( ) , It.IsAny<CancellationToken> ( ) ) )
                .ReturnsAsync ( new OAuthToken ( "access" , "accessSecret" ) );

            var sut = new ChppAuthenticationClient ( oauthClientMock.Object );
            var requestToken = new RequestToken ( "token" , "secret" );

            // Act.
            var actual = await sut.ExchangeVerifierForAccessTokenAsync ( requestToken , "verifier" , TestContext.Current.CancellationToken );

            // Assert.
            Assert.Equal ( DateTimeOffset.MaxValue , actual.ExpiresAt );
            Assert.InRange ( actual.ObtainedAt , before , DateTimeOffset.UtcNow );
        }

        [Fact]
        public async Task GetRequestTokenAsync_WhenCancellationRequested_ShouldThrowOperationCanceledExceptionAndNotCallOAuthClient ( )
        {
            // Arrange.
            var oauthClientMock = new Mock<IOAuthClient> ( );

            var sut = new ChppAuthenticationClient ( oauthClientMock.Object );

            using var cancellationTokenSource = new CancellationTokenSource ( );
            cancellationTokenSource.Cancel ( );

            // Assert.
            await Assert.ThrowsAsync<OperationCanceledException> ( ( ) => sut.GetRequestTokenAsync ( cancellationTokenSource.Token ) );
            oauthClientMock.Verify (
                c => c.GetRequestTokenAsync ( It.IsAny<CancellationToken> ( ) ) ,
                Times.Never );
        }

        [Fact]
        public async Task GetRequestTokenAsync_WhenResponseIsValid_ShouldReturnRequestTokenWithOAuthClientValues ( )
        {
            // Arrange.
            var oauthClientMock = new Mock<IOAuthClient> ( );
            oauthClientMock
                .Setup ( c => c.GetRequestTokenAsync ( It.IsAny<CancellationToken> ( ) ) )
                .ReturnsAsync ( new OAuthToken ( "abc" , "def" ) );

            var sut = new ChppAuthenticationClient ( oauthClientMock.Object );

            // Act.
            var actual = await sut.GetRequestTokenAsync ( TestContext.Current.CancellationToken );

            // Assert.
            Assert.Equal ( "abc" , actual.Token );
            Assert.Equal ( "def" , actual.TokenSecret );
        }
    }
}