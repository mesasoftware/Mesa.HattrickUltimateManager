namespace Mesa.HUM.Presentation.Tests.ViewModels.Pages
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFixture;
    using Mesa.HUM.Domain.Common;
    using Mesa.HUM.Presentation.Abstractions.Interfaces;
    using Mesa.HUM.Presentation.Contracts;
    using Mesa.HUM.Presentation.Facades.Contracts.Authorization;
    using Mesa.HUM.Presentation.Facades.Interfaces;
    using Mesa.HUM.Presentation.Stores.Contracts;
    using Mesa.HUM.Presentation.Stores.Interfaces;
    using Mesa.HUM.Presentation.ViewModels.Pages;
    using Mesa.HUM.Presentation.ViewModels.Pages.Enums;
    using Mesa.HUM.Tests.Shared.Helpers;
    using Moq;
    using TextCopy;

    public class UserProfileAuthorizationViewModelTests
    {
        private static Scope [ ] GetScopes ( )
        {
            return
            [
                new Scope("ManageChallenges", false, "manage_challenges"),
                new Scope("SetMatchOrder", true, "set_matchorder"),
                new Scope("ManageYouthPlayers", false, "manage_youthplayers"),
                new Scope("SetTraining", true, "set_training"),
                new Scope("PlaceBid", true, "place_bid"),
            ];
        }

        public class ConstructorTests
        {
            [Fact]
            public void Constructor_GoToStageCommand_ShouldNotBeNull ( )
            {
                // Arrange & Act.
                var sut = new UserProfileAuthorizationViewModel (
                    new Mock<IAuthorizationFacade> ( ).Object ,
                    new Mock<IClipboard> ( ).Object ,
                    new Mock<ILinkLauncher> ( ).Object ,
                    new Mock<INotificationsStore> ( ).Object ,
                    new Mock<IUserProfileStore> ( ).Object ,
                    GetScopes ( ) );

                // Assert.
                Assert.NotNull ( sut.GoToStageCommand );
            }
        }

        public class DisposeTests
        {
            [Fact]
            public void Dispose_ShouldUnsubscribeFromChppScopePickerCopyLinkRequested ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var getAuthorizationUrlResult = fixture.Build<GetAuthorizationUrlResultDto> ( )
                    .With ( x => x.AuthorizationUrl , ( ) => "https://chpp.hattrick.org/" )
                    .Create ( );

                var authorizationFacadeMock = new Mock<IAuthorizationFacade> ( );

                authorizationFacadeMock.Setup ( x => x.GetAuthorizationUrlAsync ( It.IsAny<string [ ]?> ( ) , It.IsAny<CancellationToken> ( ) ) )
                    .ReturnsAsync ( Result<GetAuthorizationUrlResultDto>.Success ( getAuthorizationUrlResult ) );

                var sut = new UserProfileAuthorizationViewModel (
                    authorizationFacadeMock.Object ,
                    new Mock<IClipboard> ( ).Object ,
                    new Mock<ILinkLauncher> ( ).Object ,
                    new Mock<INotificationsStore> ( ).Object ,
                    new Mock<IUserProfileStore> ( ).Object ,
                    GetScopes ( ) );

                sut.GoToStageCommand.Execute ( UserProfileAuthorizationStage.Start );

                // The handler is wired while the view model is alive.
                sut.ChppScopePicker.CopyLinkCommand.Execute ( null );
                Assert.Equal ( UserProfileAuthorizationStage.Complete , sut.Stage );

                sut.GoToStageCommand.Execute ( UserProfileAuthorizationStage.Start );

                // Act.
                sut.Dispose ( );
                sut.ChppScopePicker.CopyLinkCommand.Execute ( null );

                // Assert. The handler no longer runs after disposal, so Stage is unchanged.
                Assert.Equal ( UserProfileAuthorizationStage.Start , sut.Stage );
                authorizationFacadeMock.Verify ( x => x.GetAuthorizationUrlAsync (
                    It.IsAny<string [ ]?> ( ) ,
                    It.IsAny<CancellationToken> ( ) ) , Times.Once );
            }

            [Fact]
            public void Dispose_ShouldUnsubscribeFromChppScopePickerOpenLinkRequested ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var getAuthorizationUrlResult = fixture.Build<GetAuthorizationUrlResultDto> ( )
                    .With ( x => x.AuthorizationUrl , ( ) => "https://chpp.hattrick.org/" )
                    .Create ( );

                var authorizationFacadeMock = new Mock<IAuthorizationFacade> ( );

                authorizationFacadeMock.Setup ( x => x.GetAuthorizationUrlAsync ( It.IsAny<string [ ]?> ( ) , It.IsAny<CancellationToken> ( ) ) )
                    .ReturnsAsync ( Result<GetAuthorizationUrlResultDto>.Success ( getAuthorizationUrlResult ) );

                var sut = new UserProfileAuthorizationViewModel (
                    authorizationFacadeMock.Object ,
                    new Mock<IClipboard> ( ).Object ,
                    new Mock<ILinkLauncher> ( ).Object ,
                    new Mock<INotificationsStore> ( ).Object ,
                    new Mock<IUserProfileStore> ( ).Object ,
                    GetScopes ( ) );

                sut.GoToStageCommand.Execute ( UserProfileAuthorizationStage.Start );

                sut.ChppScopePicker.OpenLinkCommand.Execute ( null );
                Assert.Equal ( UserProfileAuthorizationStage.Complete , sut.Stage );

                sut.GoToStageCommand.Execute ( UserProfileAuthorizationStage.Start );

                // Act.
                sut.Dispose ( );
                sut.ChppScopePicker.OpenLinkCommand.Execute ( null );

                // Assert.
                Assert.Equal ( UserProfileAuthorizationStage.Start , sut.Stage );
                authorizationFacadeMock.Verify ( x => x.GetAuthorizationUrlAsync (
                    It.IsAny<string [ ]?> ( ) ,
                    It.IsAny<CancellationToken> ( ) ) , Times.Once );
            }

            [Fact]
            public void Dispose_ShouldUnsubscribeFromChppTokenVerifierGetAccessTokenRequested ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var getAuthorizationUrlResult = fixture.Build<GetAuthorizationUrlResultDto> ( )
                    .With ( x => x.AuthorizationUrl , ( ) => "https://chpp.hattrick.org/" )
                    .Create ( );

                var getAccessTokenResult = fixture.Create<GetAccessTokenResultDto> ( );

                var authorizationFacadeMock = new Mock<IAuthorizationFacade> ( );

                authorizationFacadeMock.Setup ( x => x.GetAuthorizationUrlAsync ( It.IsAny<string [ ]?> ( ) , It.IsAny<CancellationToken> ( ) ) )
                    .ReturnsAsync ( Result<GetAuthorizationUrlResultDto>.Success ( getAuthorizationUrlResult ) );

                authorizationFacadeMock.Setup ( x => x.GetAccessTokenAsync (
                        It.IsAny<RequestTokenDto> ( ) ,
                        It.IsAny<string> ( ) ,
                        It.IsAny<CancellationToken> ( ) ) )
                    .ReturnsAsync ( Result<GetAccessTokenResultDto>.Success ( getAccessTokenResult ) );

                var sut = new UserProfileAuthorizationViewModel (
                    authorizationFacadeMock.Object ,
                    new Mock<IClipboard> ( ).Object ,
                    new Mock<ILinkLauncher> ( ).Object ,
                    new Mock<INotificationsStore> ( ).Object ,
                    new Mock<IUserProfileStore> ( ).Object ,
                    GetScopes ( ) );

                // Start the authorization flow so the request token is available to the verifier.
                sut.ChppScopePicker.OpenLinkCommand.Execute ( null );

                sut.ChppTokenVerifier.Verifier = "verifier-code";

                // The handler is wired while the view model is alive.
                sut.ChppTokenVerifier.GetAccessTokenCommand.Execute ( null );
                Assert.Equal ( UserProfileAuthorizationStage.View , sut.Stage );

                sut.GoToStageCommand.Execute ( UserProfileAuthorizationStage.Start );

                // Act.
                sut.Dispose ( );
                sut.ChppTokenVerifier.GetAccessTokenCommand.Execute ( null );

                // Assert.
                Assert.Equal ( UserProfileAuthorizationStage.Start , sut.Stage );
                authorizationFacadeMock.Verify ( x => x.GetAccessTokenAsync (
                    It.IsAny<RequestTokenDto> ( ) ,
                    It.IsAny<string> ( ) ,
                    It.IsAny<CancellationToken> ( ) ) , Times.Once );
            }

            [Fact]
            public void Dispose_ShouldUnsubscribeFromChppTokenVerifierGoBackRequested ( )
            {
                // Arrange.
                var sut = new UserProfileAuthorizationViewModel (
                    new Mock<IAuthorizationFacade> ( ).Object ,
                    new Mock<IClipboard> ( ).Object ,
                    new Mock<ILinkLauncher> ( ).Object ,
                    new Mock<INotificationsStore> ( ).Object ,
                    new Mock<IUserProfileStore> ( ).Object ,
                    GetScopes ( ) );

                sut.GoToStageCommand.Execute ( UserProfileAuthorizationStage.View );

                sut.ChppTokenVerifier.GoBackCommand.Execute ( null );
                Assert.Equal ( UserProfileAuthorizationStage.Start , sut.Stage );

                sut.GoToStageCommand.Execute ( UserProfileAuthorizationStage.View );

                // Act.
                sut.Dispose ( );
                sut.ChppTokenVerifier.GoBackCommand.Execute ( null );

                // Assert.
                Assert.Equal ( UserProfileAuthorizationStage.View , sut.Stage );
            }

            [Fact]
            public void Dispose_WhenCalledMultipleTimes_ShouldNotThrow ( )
            {
                // Arrange.
                var sut = new UserProfileAuthorizationViewModel (
                    new Mock<IAuthorizationFacade> ( ).Object ,
                    new Mock<IClipboard> ( ).Object ,
                    new Mock<ILinkLauncher> ( ).Object ,
                    new Mock<INotificationsStore> ( ).Object ,
                    new Mock<IUserProfileStore> ( ).Object ,
                    GetScopes ( ) );

                // Act.
                var exception = Record.Exception ( ( ) =>
                {
                    sut.Dispose ( );
                    sut.Dispose ( );
                } );

                // Assert.
                Assert.Null ( exception );
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

                var sut = new UserProfileAuthorizationViewModel (
                    new Mock<IAuthorizationFacade> ( ).Object ,
                    new Mock<IClipboard> ( ).Object ,
                    new Mock<ILinkLauncher> ( ).Object ,
                    new Mock<INotificationsStore> ( ).Object ,
                    userProfileStoreMock.Object ,
                    GetScopes ( ) );

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

                var sut = new UserProfileAuthorizationViewModel (
                    new Mock<IAuthorizationFacade> ( ).Object ,
                    new Mock<IClipboard> ( ).Object ,
                    new Mock<ILinkLauncher> ( ).Object ,
                    new Mock<INotificationsStore> ( ).Object ,
                    userProfileStoreMock.Object ,
                    GetScopes ( ) );

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

                var sut = new UserProfileAuthorizationViewModel (
                    new Mock<IAuthorizationFacade> ( ).Object ,
                    new Mock<IClipboard> ( ).Object ,
                    new Mock<ILinkLauncher> ( ).Object ,
                    new Mock<INotificationsStore> ( ).Object ,
                    userProfileStoreMock.Object ,
                    GetScopes ( ) );

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
                var sut = new UserProfileAuthorizationViewModel (
                    new Mock<IAuthorizationFacade> ( ).Object ,
                    new Mock<IClipboard> ( ).Object ,
                    new Mock<ILinkLauncher> ( ).Object ,
                    new Mock<INotificationsStore> ( ).Object ,
                    new Mock<IUserProfileStore> ( ).Object ,
                    GetScopes ( ) );

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
                var sut = new UserProfileAuthorizationViewModel (
                    new Mock<IAuthorizationFacade> ( ).Object ,
                    new Mock<IClipboard> ( ).Object ,
                    new Mock<ILinkLauncher> ( ).Object ,
                    new Mock<INotificationsStore> ( ).Object ,
                    new Mock<IUserProfileStore> ( ).Object ,
                    GetScopes ( ) );

                // Act.
                sut.GoToStageCommand.Execute ( value );

                // Assert.
                Assert.Equal ( value , sut.Stage );
            }
        }

        public class PropertyTests
        {
            public class ShowChppScopePickerTests
            {
                [Theory]
                [InlineData ( UserProfileAuthorizationStage.Start , true )]
                [InlineData ( UserProfileAuthorizationStage.Complete , false )]
                [InlineData ( UserProfileAuthorizationStage.View , false )]
                public void ShowChppScopePicker_GivenStageValue_ShouldBeEqual ( UserProfileAuthorizationStage value , bool expected )
                {
                    // Arrange.
                    var sut = new UserProfileAuthorizationViewModel (
                        new Mock<IAuthorizationFacade> ( ).Object ,
                        new Mock<IClipboard> ( ).Object ,
                        new Mock<ILinkLauncher> ( ).Object ,
                        new Mock<INotificationsStore> ( ).Object ,
                        new Mock<IUserProfileStore> ( ).Object ,
                        GetScopes ( ) );

                    // Act.
                    sut.GoToStageCommand.Execute ( value );

                    // Assert.
                    Assert.Equal ( expected , sut.ShowChppScopePicker );
                }

                [Theory]
                [InlineData ( UserProfileAuthorizationStage.Start , UserProfileAuthorizationStage.View )]
                [InlineData ( UserProfileAuthorizationStage.Complete , UserProfileAuthorizationStage.Start )]
                [InlineData ( UserProfileAuthorizationStage.View , UserProfileAuthorizationStage.Start )]
                public void ShowChppScopePicker_WhenValueChanges_ShouldRaisePropertyEvents ( UserProfileAuthorizationStage value , UserProfileAuthorizationStage initialValue )
                {
                    // Arrange.
                    var sut = new UserProfileAuthorizationViewModel (
                        new Mock<IAuthorizationFacade> ( ).Object ,
                        new Mock<IClipboard> ( ).Object ,
                        new Mock<ILinkLauncher> ( ).Object ,
                        new Mock<INotificationsStore> ( ).Object ,
                        new Mock<IUserProfileStore> ( ).Object ,
                        GetScopes ( ) );

                    sut.GoToStageCommand.Execute ( initialValue );

                    bool propertyChangedRaised = false;
                    bool propertyChangingRaised = false;

                    sut.PropertyChanged += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.ShowChppScopePicker ) )
                        {
                            propertyChangedRaised = true;
                        }
                    };

                    sut.PropertyChanging += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.ShowChppScopePicker ) )
                        {
                            propertyChangingRaised = true;
                        }
                    };

                    // Act.
                    sut.GoToStageCommand.Execute ( value );

                    // Assert.
                    Assert.True ( propertyChangedRaised );
                    Assert.True ( propertyChangingRaised );
                }

                [Theory]
                [InlineData ( UserProfileAuthorizationStage.Start )]
                [InlineData ( UserProfileAuthorizationStage.Complete )]
                [InlineData ( UserProfileAuthorizationStage.View )]
                public void ShowChppScopePicker_WhenValueDoesNotChange_ShouldNotRaisePropertyEvents ( UserProfileAuthorizationStage value )
                {
                    // Arrange.
                    var sut = new UserProfileAuthorizationViewModel (
                        new Mock<IAuthorizationFacade> ( ).Object ,
                        new Mock<IClipboard> ( ).Object ,
                        new Mock<ILinkLauncher> ( ).Object ,
                        new Mock<INotificationsStore> ( ).Object ,
                        new Mock<IUserProfileStore> ( ).Object ,
                        GetScopes ( ) );

                    sut.GoToStageCommand.Execute ( value );

                    bool propertyChangedRaised = false;
                    bool propertyChangingRaised = false;

                    sut.PropertyChanged += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.ShowChppScopePicker ) )
                        {
                            propertyChangedRaised = true;
                        }
                    };

                    sut.PropertyChanging += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.ShowChppScopePicker ) )
                        {
                            propertyChangingRaised = true;
                        }
                    };

                    // Act.
                    sut.GoToStageCommand.Execute ( value );

                    // Assert.
                    Assert.False ( propertyChangedRaised );
                    Assert.False ( propertyChangingRaised );
                }
            }

            public class ShowChppTokenDetailsTests
            {
                [Theory]
                [InlineData ( UserProfileAuthorizationStage.Start , false )]
                [InlineData ( UserProfileAuthorizationStage.Complete , false )]
                [InlineData ( UserProfileAuthorizationStage.View , true )]
                public void ShowChppTokenDetails_GivenStageValue_ShouldBeEqual ( UserProfileAuthorizationStage value , bool expected )
                {
                    // Arrange.
                    var sut = new UserProfileAuthorizationViewModel (
                        new Mock<IAuthorizationFacade> ( ).Object ,
                        new Mock<IClipboard> ( ).Object ,
                        new Mock<ILinkLauncher> ( ).Object ,
                        new Mock<INotificationsStore> ( ).Object ,
                        new Mock<IUserProfileStore> ( ).Object ,
                        GetScopes ( ) );

                    // Act.
                    sut.GoToStageCommand.Execute ( value );

                    // Assert.
                    Assert.Equal ( expected , sut.ShowChppTokenDetails );
                }

                [Theory]
                [InlineData ( UserProfileAuthorizationStage.Start , UserProfileAuthorizationStage.View )]
                [InlineData ( UserProfileAuthorizationStage.Complete , UserProfileAuthorizationStage.Start )]
                [InlineData ( UserProfileAuthorizationStage.View , UserProfileAuthorizationStage.Start )]
                public void ShowChppTokenDetails_WhenValueChanges_ShouldRaisePropertyEvents ( UserProfileAuthorizationStage value , UserProfileAuthorizationStage initialValue )
                {
                    // Arrange.
                    var sut = new UserProfileAuthorizationViewModel (
                        new Mock<IAuthorizationFacade> ( ).Object ,
                        new Mock<IClipboard> ( ).Object ,
                        new Mock<ILinkLauncher> ( ).Object ,
                        new Mock<INotificationsStore> ( ).Object ,
                        new Mock<IUserProfileStore> ( ).Object ,
                        GetScopes ( ) );

                    sut.GoToStageCommand.Execute ( initialValue );

                    bool propertyChangedRaised = false;
                    bool propertyChangingRaised = false;

                    sut.PropertyChanged += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.ShowChppTokenDetails ) )
                        {
                            propertyChangedRaised = true;
                        }
                    };

                    sut.PropertyChanging += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.ShowChppTokenDetails ) )
                        {
                            propertyChangingRaised = true;
                        }
                    };

                    // Act.
                    sut.GoToStageCommand.Execute ( value );

                    // Assert.
                    Assert.True ( propertyChangedRaised );
                    Assert.True ( propertyChangingRaised );
                }

                [Theory]
                [InlineData ( UserProfileAuthorizationStage.Start )]
                [InlineData ( UserProfileAuthorizationStage.Complete )]
                [InlineData ( UserProfileAuthorizationStage.View )]
                public void ShowChppTokenDetails_WhenValueDoesNotChange_ShouldNotRaisePropertyEvents ( UserProfileAuthorizationStage value )
                {
                    // Arrange.
                    var sut = new UserProfileAuthorizationViewModel (
                        new Mock<IAuthorizationFacade> ( ).Object ,
                        new Mock<IClipboard> ( ).Object ,
                        new Mock<ILinkLauncher> ( ).Object ,
                        new Mock<INotificationsStore> ( ).Object ,
                        new Mock<IUserProfileStore> ( ).Object ,
                        GetScopes ( ) );

                    sut.GoToStageCommand.Execute ( value );

                    bool propertyChangedRaised = false;
                    bool propertyChangingRaised = false;

                    sut.PropertyChanged += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.ShowChppTokenDetails ) )
                        {
                            propertyChangedRaised = true;
                        }
                    };

                    sut.PropertyChanging += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.ShowChppTokenDetails ) )
                        {
                            propertyChangingRaised = true;
                        }
                    };

                    // Act.
                    sut.GoToStageCommand.Execute ( value );

                    // Assert.
                    Assert.False ( propertyChangedRaised );
                    Assert.False ( propertyChangingRaised );
                }
            }

            public class ShowChppTokenVerifierTests
            {
                [Theory]
                [InlineData ( UserProfileAuthorizationStage.Start , false )]
                [InlineData ( UserProfileAuthorizationStage.Complete , true )]
                [InlineData ( UserProfileAuthorizationStage.View , false )]
                public void ShowChppTokenVerifier_GivenStageValue_ShouldBeEqual ( UserProfileAuthorizationStage value , bool expected )
                {
                    // Arrange.
                    var sut = new UserProfileAuthorizationViewModel (
                        new Mock<IAuthorizationFacade> ( ).Object ,
                        new Mock<IClipboard> ( ).Object ,
                        new Mock<ILinkLauncher> ( ).Object ,
                        new Mock<INotificationsStore> ( ).Object ,
                        new Mock<IUserProfileStore> ( ).Object ,
                        GetScopes ( ) );

                    // Act.
                    sut.GoToStageCommand.Execute ( value );

                    // Assert.
                    Assert.Equal ( expected , sut.ShowChppTokenVerifier );
                }

                [Theory]
                [InlineData ( UserProfileAuthorizationStage.Start , UserProfileAuthorizationStage.View )]
                [InlineData ( UserProfileAuthorizationStage.Complete , UserProfileAuthorizationStage.Start )]
                [InlineData ( UserProfileAuthorizationStage.View , UserProfileAuthorizationStage.Start )]
                public void ShowChppTokenVerifier_WhenValueChanges_ShouldRaisePropertyEvents ( UserProfileAuthorizationStage value , UserProfileAuthorizationStage initialValue )
                {
                    // Arrange.
                    var sut = new UserProfileAuthorizationViewModel (
                        new Mock<IAuthorizationFacade> ( ).Object ,
                        new Mock<IClipboard> ( ).Object ,
                        new Mock<ILinkLauncher> ( ).Object ,
                        new Mock<INotificationsStore> ( ).Object ,
                        new Mock<IUserProfileStore> ( ).Object ,
                        GetScopes ( ) );

                    sut.GoToStageCommand.Execute ( initialValue );

                    bool propertyChangedRaised = false;
                    bool propertyChangingRaised = false;

                    sut.PropertyChanged += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.ShowChppTokenVerifier ) )
                        {
                            propertyChangedRaised = true;
                        }
                    };

                    sut.PropertyChanging += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.ShowChppTokenVerifier ) )
                        {
                            propertyChangingRaised = true;
                        }
                    };

                    // Act.
                    sut.GoToStageCommand.Execute ( value );

                    // Assert.
                    Assert.True ( propertyChangedRaised );
                    Assert.True ( propertyChangingRaised );
                }

                [Theory]
                [InlineData ( UserProfileAuthorizationStage.Start )]
                [InlineData ( UserProfileAuthorizationStage.Complete )]
                [InlineData ( UserProfileAuthorizationStage.View )]
                public void ShowChppTokenVerifier_WhenValueDoesNotChange_ShouldNotRaisePropertyEvents ( UserProfileAuthorizationStage value )
                {
                    // Arrange.
                    var sut = new UserProfileAuthorizationViewModel (
                        new Mock<IAuthorizationFacade> ( ).Object ,
                        new Mock<IClipboard> ( ).Object ,
                        new Mock<ILinkLauncher> ( ).Object ,
                        new Mock<INotificationsStore> ( ).Object ,
                        new Mock<IUserProfileStore> ( ).Object ,
                        GetScopes ( ) );

                    sut.GoToStageCommand.Execute ( value );

                    bool propertyChangedRaised = false;
                    bool propertyChangingRaised = false;

                    sut.PropertyChanged += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.ShowChppTokenVerifier ) )
                        {
                            propertyChangedRaised = true;
                        }
                    };

                    sut.PropertyChanging += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.ShowChppTokenVerifier ) )
                        {
                            propertyChangingRaised = true;
                        }
                    };

                    // Act.
                    sut.GoToStageCommand.Execute ( value );

                    // Assert.
                    Assert.False ( propertyChangedRaised );
                    Assert.False ( propertyChangingRaised );
                }
            }

            public class StageTests
            {
                [Theory]
                [InlineData ( UserProfileAuthorizationStage.Start , UserProfileAuthorizationStage.View )]
                [InlineData ( UserProfileAuthorizationStage.Complete , UserProfileAuthorizationStage.Start )]
                [InlineData ( UserProfileAuthorizationStage.View , UserProfileAuthorizationStage.Start )]
                public void Stage_WhenValueChanges_ShouldRaisePropertyEvents ( UserProfileAuthorizationStage value , UserProfileAuthorizationStage initialValue )
                {
                    // Arrange.
                    var sut = new UserProfileAuthorizationViewModel (
                        new Mock<IAuthorizationFacade> ( ).Object ,
                        new Mock<IClipboard> ( ).Object ,
                        new Mock<ILinkLauncher> ( ).Object ,
                        new Mock<INotificationsStore> ( ).Object ,
                        new Mock<IUserProfileStore> ( ).Object ,
                        GetScopes ( ) );

                    sut.GoToStageCommand.Execute ( initialValue );

                    bool propertyChangedRaised = false;
                    bool propertyChangingRaised = false;

                    sut.PropertyChanged += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.Stage ) )
                        {
                            propertyChangedRaised = true;
                        }
                    };

                    sut.PropertyChanging += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.Stage ) )
                        {
                            propertyChangingRaised = true;
                        }
                    };

                    // Act.
                    sut.GoToStageCommand.Execute ( value );

                    // Assert.
                    Assert.True ( propertyChangedRaised );
                    Assert.True ( propertyChangingRaised );
                    Assert.Equal ( value , sut.Stage );
                }

                [Theory]
                [InlineData ( UserProfileAuthorizationStage.Start )]
                [InlineData ( UserProfileAuthorizationStage.Complete )]
                [InlineData ( UserProfileAuthorizationStage.View )]
                public void Stage_WhenValueDoesNotChange_ShouldNotRaisePropertyEvents ( UserProfileAuthorizationStage value )
                {
                    // Arrange.
                    var sut = new UserProfileAuthorizationViewModel (
                        new Mock<IAuthorizationFacade> ( ).Object ,
                        new Mock<IClipboard> ( ).Object ,
                        new Mock<ILinkLauncher> ( ).Object ,
                        new Mock<INotificationsStore> ( ).Object ,
                        new Mock<IUserProfileStore> ( ).Object ,
                        GetScopes ( ) );

                    sut.GoToStageCommand.Execute ( value );

                    bool propertyChangedRaised = false;
                    bool propertyChangingRaised = false;

                    sut.PropertyChanged += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.Stage ) )
                        {
                            propertyChangedRaised = true;
                        }
                    };

                    sut.PropertyChanging += ( s , e ) =>
                    {
                        if ( e.PropertyName == nameof ( sut.Stage ) )
                        {
                            propertyChangingRaised = true;
                        }
                    };

                    // Act.
                    sut.GoToStageCommand.Execute ( value );

                    // Assert.
                    Assert.False ( propertyChangedRaised );
                    Assert.False ( propertyChangingRaised );
                    Assert.Equal ( value , sut.Stage );
                }
            }
        }
    }
}