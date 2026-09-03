namespace Mesa.HUM.Presentation.Tests.ViewModels.Pages
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFixture;
    using Mesa.HUM.Presentation.Stores.Contracts;
    using Mesa.HUM.Presentation.Stores.Interfaces;
    using Mesa.HUM.Presentation.ViewModels.Pages;
    using Mesa.HUM.Presentation.ViewModels.Pages.Enums;
    using Mesa.HUM.Tests.Shared.Helpers;
    using Moq;

    public class UserProfileAuthorizationViewModelTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void Constructor_GoToStageCommand_ShouldNotBeNull ( )
            {
                // Arrange.
                var userProfileStoreMock = new Mock<IUserProfileStore> ( );

                // Act.
                var sut = new UserProfileAuthorizationViewModel ( userProfileStoreMock.Object , [ ] );

                // Assert.
                Assert.NotNull ( sut.GoToStageCommand );
            }
        }

        public class InitializeAsyncTests
        {
            [Fact]
            public async Task InitializeAsync_GivenAuthorizedUserProfile_ShouldSetStageToView ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var userProfile = fixture.Build<UserProfileDto> ( )
                    .With ( x => x.IsAuthorized , ( ) => true )
                    .Create ( );

                var userProfileStoreMock = new Mock<IUserProfileStore> ( );

                userProfileStoreMock
                    .SetupGet ( x => x.UserProfile )
                    .Returns ( userProfile );

                var sut = new UserProfileAuthorizationViewModel ( userProfileStoreMock.Object , [ ] );

                var expected = UserProfileAuthorizationStage.View;

                // Act.
                await sut.InitializeAsync ( TestContext.Current.CancellationToken );

                // Assert.
                Assert.Equal ( expected , sut.Stage );
            }

            [Fact]
            public async Task InitializeAsync_GivenNullUserProfile_ShouldSetStageToStart ( )
            {
                // Arrange.
                var userProfileStoreMock = new Mock<IUserProfileStore> ( );

                userProfileStoreMock
                    .SetupGet ( x => x.UserProfile )
                    .Returns ( ( UserProfileDto? ) null );

                var sut = new UserProfileAuthorizationViewModel ( userProfileStoreMock.Object , [ ] );

                var expected = UserProfileAuthorizationStage.Start;

                // Act.
                await sut.InitializeAsync ( TestContext.Current.CancellationToken );

                // Assert.
                Assert.Equal ( expected , sut.Stage );
            }

            [Fact]
            public async Task InitializeAsync_GivenUnauthorizedUserProfile_ShouldSetStageToStart ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var userProfile = fixture.Build<UserProfileDto> ( )
                    .With ( x => x.IsAuthorized , ( ) => false )
                    .Create ( );

                var userProfileStoreMock = new Mock<IUserProfileStore> ( );

                userProfileStoreMock
                    .SetupGet ( x => x.UserProfile )
                    .Returns ( userProfile );

                var sut = new UserProfileAuthorizationViewModel ( userProfileStoreMock.Object , [ ] );

                var expected = UserProfileAuthorizationStage.Start;

                // Act.
                await sut.InitializeAsync ( TestContext.Current.CancellationToken );

                // Assert.
                Assert.Equal ( expected , sut.Stage );
            }

            [Fact]
            public async Task InitializeAsync_WhenCancellationIsRequested_ShouldThrowOperationCanceledException ( )
            {
                // Arrange.
                var userProfileStoreMock = new Mock<IUserProfileStore> ( );

                var sut = new UserProfileAuthorizationViewModel ( userProfileStoreMock.Object , [ ] );

                var cancellationTokenSource = new CancellationTokenSource ( );

                cancellationTokenSource.Cancel ( );

                // Act.
                var actual = await Record.ExceptionAsync ( ( ) => sut.InitializeAsync ( cancellationTokenSource.Token ) );

                // Assert.
                Assert.NotNull ( actual );
                Assert.IsType<OperationCanceledException> ( actual );
            }
        }

        public class OnGoToStageTests
        {
            [Theory]
            [InlineData ( UserProfileAuthorizationStage.Complete )]
            [InlineData ( UserProfileAuthorizationStage.Start )]
            [InlineData ( UserProfileAuthorizationStage.View )]
            public void GoToStage_GivenStageParameter_ShouldSetStageToValue ( UserProfileAuthorizationStage value )
            {
                // Arrange.
                var userProfileStoreMock = new Mock<IUserProfileStore> ( );

                var sut = new UserProfileAuthorizationViewModel ( userProfileStoreMock.Object , [ ] );

                // Act.
                sut.GoToStageCommand.Execute ( value );

                // Assert.
                Assert.Equal ( value , sut.Stage );
            }
        }
    }
}