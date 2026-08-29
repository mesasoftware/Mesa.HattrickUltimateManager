namespace Mesa.HUM.Application.Tests.Features.Profiles
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Mesa.HUM.Application.Features.Profiles.Authorization.CompleteChppAuthorization;
    using Mesa.HUM.Application.Hattrick.Abstractions.Interfaces;
    using Mesa.HUM.Application.Hattrick.Contracts;
    using Mesa.HUM.Domain.Common.Enums;
    using Moq;

    public class CompleteChppAuthorizationCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WhenAccessTokenObtained_ShouldReturnSuccessWithResponse ( )
        {
            // Arrange.
            var accessToken = new AccessToken ( "access" , "accessSecret" , DateTimeOffset.UtcNow , DateTimeOffset.MaxValue );

            var chppAuthenticationClientMock = new Mock<IChppAuthenticationClient> ( );

            chppAuthenticationClientMock
                .Setup ( x => x.ExchangeVerifierForAccessTokenAsync (
                    It.IsAny<RequestToken> ( ) ,
                    It.IsAny<string> ( ) ,
                    It.IsAny<CancellationToken> ( ) ) )
                .ReturnsAsync ( accessToken );

            var handler = GetHandler ( chppAuthenticationClientMock.Object );

            var requestToken = new RequestToken ( "token" , "tokenSecret" );

            // Act.
            var actual = await handler.Handle ( new CompleteChppAuthorizationCommand ( requestToken , "verifier" ) , CancellationToken.None );

            // Assert.
            Assert.True ( actual.IsSuccess );
            Assert.Same ( accessToken , actual.Value.AccessToken );
            chppAuthenticationClientMock.Verify (
                x => x.ExchangeVerifierForAccessTokenAsync (
                    requestToken ,
                    "verifier" ,
                    It.IsAny<CancellationToken> ( ) ) ,
                Times.Once );
        }

        [Fact]
        public async Task Handle_WhenIntegrationServiceFails_ShouldReturnFailureWithError ( )
        {
            // Arrange.
            var chppAuthenticationClientMock = new Mock<IChppAuthenticationClient> ( );

            chppAuthenticationClientMock
                .Setup ( x => x.ExchangeVerifierForAccessTokenAsync (
                    It.IsAny<RequestToken> ( ) ,
                    It.IsAny<string> ( ) ,
                    It.IsAny<CancellationToken> ( ) ) )
                .ThrowsAsync ( new Exception ( "CANNOT_OBTAIN_ACCESS_TOKEN" ) );

            var handler = GetHandler ( chppAuthenticationClientMock.Object );

            var requestToken = new RequestToken ( "token" , "tokenSecret" );

            // Act.
            var actual = await handler.Handle ( new CompleteChppAuthorizationCommand ( requestToken , "verifier" ) , CancellationToken.None );

            // Assert.
            Assert.True ( actual.IsFailure );
            Assert.NotNull ( actual.Error );
            Assert.Equal ( ErrorType.Failure , actual.Error.Type );
            Assert.Equal ( "CHPP_INTEGRATION_ERROR" , actual.Error.Code );
            Assert.Equal ( "CANNOT_OBTAIN_ACCESS_TOKEN" , actual.Error.Description );
            chppAuthenticationClientMock.Verify (
                x => x.ExchangeVerifierForAccessTokenAsync (
                    It.IsAny<RequestToken> ( ) ,
                    It.IsAny<string> ( ) ,
                    It.IsAny<CancellationToken> ( ) ) ,
                Times.Once );
        }

        [Fact]
        public async Task Handle_WhenIntegrationServiceReturnsNull_ShouldReturnFailureWithError ( )
        {
            // Arrange.
            var chppAuthenticationClientMock = new Mock<IChppAuthenticationClient> ( );

            chppAuthenticationClientMock
                .Setup ( x => x.ExchangeVerifierForAccessTokenAsync (
                    It.IsAny<RequestToken> ( ) ,
                    It.IsAny<string> ( ) ,
                    It.IsAny<CancellationToken> ( ) ) )
                .ReturnsAsync ( ( AccessToken ) null! );

            var handler = GetHandler ( chppAuthenticationClientMock.Object );

            var requestToken = new RequestToken ( "token" , "tokenSecret" );

            // Act.
            var actual = await handler.Handle ( new CompleteChppAuthorizationCommand ( requestToken , "verifier" ) , CancellationToken.None );

            // Assert.
            Assert.True ( actual.IsFailure );
            Assert.NotNull ( actual.Error );
            Assert.Equal ( ErrorType.Failure , actual.Error.Type );
            Assert.Equal ( "CHPP_INTEGRATION_ERROR" , actual.Error.Code );
        }

        [Fact]
        public async Task Handle_WhenUserRequestedCancellation_ShouldReturnFailureWithError ( )
        {
            // Arrange.
            var chppAuthenticationClientMock = new Mock<IChppAuthenticationClient> ( );

            var cancellationTokenSource = new CancellationTokenSource ( );

            var handler = GetHandler ( chppAuthenticationClientMock.Object );

            var requestToken = new RequestToken ( "token" , "tokenSecret" );

            cancellationTokenSource.Cancel ( );

            // Act.
            var actual = await handler.Handle ( new CompleteChppAuthorizationCommand ( requestToken , "verifier" ) , cancellationTokenSource.Token );

            // Assert.
            Assert.True ( actual.IsFailure );
            Assert.NotNull ( actual.Error );
            Assert.Equal ( ErrorType.Failure , actual.Error.Type );
            Assert.Equal ( "OPERATION_CANCELLED_ERROR" , actual.Error.Code );
            Assert.Equal ( "The operation was canceled." , actual.Error.Description );
            chppAuthenticationClientMock.Verify (
                x => x.ExchangeVerifierForAccessTokenAsync (
                    It.IsAny<RequestToken> ( ) ,
                    It.IsAny<string> ( ) ,
                    It.IsAny<CancellationToken> ( ) ) ,
                Times.Never );
        }

        private static CompleteChppAuthorizationCommandHandler GetHandler ( IChppAuthenticationClient chppAuthenticationClient )
        {
            return new CompleteChppAuthorizationCommandHandler ( chppAuthenticationClient );
        }
    }
}