namespace Mesa.HUM.Application.Tests.Features.Profiles
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Mesa.HUM.Application.Features.Profiles.Authorization.StartChppAuthorization;
    using Mesa.HUM.Application.Hattrick.Abstractions.Interfaces;
    using Mesa.HUM.Application.Hattrick.Contracts;
    using Mesa.HUM.Domain.Common.Enums;
    using Moq;

    public class StartChppAuthorizationCommandHandlerTests
    {
        private static StartChppAuthorizationCommandHandler GetHandler ( IChppAuthenticationClient chppAuthenticationClient )
        {
            return new StartChppAuthorizationCommandHandler ( chppAuthenticationClient );
        }

        public class HandlerTests
        {
            [Fact]
            public async Task Handle_WhenIntegrationServiceFails_ShouldReturnFailureWithError ( )
            {
                // Arrange.
                var chppAuthenticationClientMock = new Mock<IChppAuthenticationClient> ( );

                chppAuthenticationClientMock.Setup ( x => x.GetRequestTokenAsync ( It.IsAny<CancellationToken> ( ) ) )
                    .ThrowsAsync ( new Exception ( "CANNOT_OBTAIN_REQUEST_TOKEN" ) );

                var handler = GetHandler ( chppAuthenticationClientMock.Object );

                // Act.
                var actual = await handler.Handle ( new StartChppAuthorizationCommand ( ) , CancellationToken.None );

                // Assert.
                Assert.True ( actual.IsFailure );
                Assert.NotNull ( actual.Error );
                Assert.Equal ( ErrorType.Failure , actual.Error.Type );
                Assert.Equal ( "CHPP_INTEGRATION_ERROR" , actual.Error.Code );
                Assert.Equal ( "CANNOT_OBTAIN_REQUEST_TOKEN" , actual.Error.Description );
            }

            [Theory]
            [InlineData ( null )]
            [InlineData ( [ ] )]
            [InlineData ( [ "scope1" ] )]
            [InlineData ( [ "scope1" , "scope2" ] )]
            public async Task Handle_WhenNoErrorOccurs_ShouldReturnSuccessWithCorrectResult ( params string [ ]? scopes )
            {
                // Arrange.
                string expectedToken = "ABCDEFGHIJKLMNOP";
                string expectedSecret = "ZYXWVUTSRQPONMLK";

                string scopesQueryParam = scopes?.Length > 0
                    ? $"&scope={string.Join ( ',' , scopes )}"
                    : string.Empty;

                string expectedUrl = $"https://chpp.hattrick.org/oauth/authorize.aspx?oauth_token={expectedToken}{scopesQueryParam}";

                var chppAuthenticationClientMock = new Mock<IChppAuthenticationClient> ( );

                chppAuthenticationClientMock.Setup ( x => x.GetRequestTokenAsync ( It.IsAny<CancellationToken> ( ) ) )
                    .ReturnsAsync ( ( ) => new RequestToken ( expectedToken , expectedSecret ) );

                chppAuthenticationClientMock.Setup ( x => x.BuildAuthorizationUrl ( It.IsAny<RequestToken> ( ) , It.IsAny<string [ ]?> ( ) ) )
                    .Returns ( expectedUrl );

                var sut = GetHandler ( chppAuthenticationClientMock.Object );

                var request = new StartChppAuthorizationCommand ( scopes );

                // Act.
                var actual = await sut.Handle ( request , CancellationToken.None );

                // Assert.
                Assert.True ( actual.IsSuccess );
                Assert.Equal ( expectedToken , actual.Value.RequestToken.Token );
                Assert.Equal ( expectedSecret , actual.Value.RequestToken.TokenSecret );
                Assert.Equal ( expectedUrl , actual.Value.AuthorizationUrl );
            }

            [Fact]
            public async Task Handle_WhenUserRequestedCancellation_ShouldReturnFailureWithError ( )
            {
                // Arrange.
                var chppAuthenticationClientMock = new Mock<IChppAuthenticationClient> ( );

                var cancellationTokenSource = new CancellationTokenSource ( );

                var handler = GetHandler ( chppAuthenticationClientMock.Object );

                cancellationTokenSource.Cancel ( );

                // Act.
                var actual = await handler.Handle ( new StartChppAuthorizationCommand ( ) , cancellationTokenSource.Token );

                // Assert.
                Assert.True ( actual.IsFailure );
                Assert.NotNull ( actual.Error );
                Assert.Equal ( ErrorType.Failure , actual.Error.Type );
                Assert.Equal ( "OPERATION_CANCELLED_ERROR" , actual.Error.Code );
                Assert.Equal ( "The operation was canceled." , actual.Error.Description );
                chppAuthenticationClientMock.Verify (
                    x => x.GetRequestTokenAsync ( It.IsAny<CancellationToken> ( ) ) ,
                    Times.Never );
                chppAuthenticationClientMock.Verify (
                    x => x.BuildAuthorizationUrl ( It.IsAny<RequestToken> ( ) , It.IsAny<string [ ]?> ( ) ) ,
                    Times.Never );
            }
        }
    }
}