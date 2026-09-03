namespace Mesa.HUM.Presentation.Tests.ViewModels.Components.UserProfiles
{
    using System.Threading.Tasks;
    using AutoFixture;
    using Mesa.HUM.Presentation.ViewModels.Components.UserProfiles;
    using Mesa.HUM.Tests.Shared.Helpers;

    public class ChppTokenVerifierViewModelTests
    {
        public class CanAuthorizeTests
        {
            [Fact]
            public void CanAuthorize_WhenVerifierIsNotNullEmptyOrWhitespace_ShouldBeTrue ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new ChppTokenVerifierViewModel ( )
                {
                    // Act.
                    Verifier = fixture.Create<string> ( )
                };

                // Assert.
                Assert.True ( sut.CanAuthorize );
            }

            [Theory]
            [InlineData ( null )]
            [InlineData ( "" )]
            [InlineData ( "   " )]
            public void CanAuthorize_WhenVerifierIsNullEmptyOrWhitespace_ShouldBeFalse ( string? value )
            {
                // Arrange.
                var sut = new ChppTokenVerifierViewModel ( )
                {
                    // Act.
                    Verifier = value
                };

                // Assert.
                Assert.False ( sut.CanAuthorize );
            }

            [Fact]
            public void Verifier_WhenValueChanges_ShouldRaisePropertyEventsForCanAuthorize ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new ChppTokenVerifierViewModel ( );

                bool canAuthorizeChangingRaised = false;
                bool canAuthorizeChangedRaised = false;

                sut.PropertyChanging += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.CanAuthorize ) )
                    {
                        canAuthorizeChangingRaised = true;
                    }
                };

                sut.PropertyChanged += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.CanAuthorize ) )
                    {
                        canAuthorizeChangedRaised = true;
                    }
                };

                // Act.
                sut.Verifier = fixture.Create<string> ( );

                // Assert.
                Assert.True ( canAuthorizeChangingRaised );
                Assert.True ( canAuthorizeChangedRaised );
            }
        }

        public class ConstructorTests
        {
            [Fact]
            public void Constructor_CommandsShouldBeInitializedCorrectly ( )
            {
                // Act.
                var sut = new ChppTokenVerifierViewModel ( );

                // Assert.
                Assert.NotNull ( sut.GetAccessTokenCommand );
                Assert.NotNull ( sut.GoBackCommand );
                Assert.Null ( sut.Verifier );
            }
        }

        public class GetAccessTokenCommandCanExecuteTests
        {
            [Fact]
            public void CanExecute_WhenVerifierIsNotNullEmptyOrWhitespace_ShouldBeTrue ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new ChppTokenVerifierViewModel ( )
                {
                    // Act.
                    Verifier = fixture.Create<string> ( )
                };

                // Assert.
                Assert.True ( sut.GetAccessTokenCommand.CanExecute ( null ) );
            }

            [Theory]
            [InlineData ( null )]
            [InlineData ( "" )]
            [InlineData ( "   " )]
            public void CanExecute_WhenVerifierIsNullEmptyOrWhitespace_ShouldBeFalse ( string? value )
            {
                // Arrange.
                var sut = new ChppTokenVerifierViewModel ( )
                {
                    // Act.
                    Verifier = value
                };

                // Assert.
                Assert.False ( sut.GetAccessTokenCommand.CanExecute ( null ) );
            }
        }

        public class GetAccessTokenTests
        {
            [Fact]
            public async Task GetAccessTokenRequested_WhenEventHasHandler_ShouldCallHandlerAndSendVerifier ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new ChppTokenVerifierViewModel ( );

                string expected = fixture.Create<string> ( );

                bool getAccessTokenRequestedRaised = false;

                sut.GetAccessTokenRequested += value =>
                {
                    if ( value == expected )
                    {
                        getAccessTokenRequestedRaised = true;
                    }

                    return Task.CompletedTask;
                };

                sut.Verifier = expected;

                // Act.
                await sut.GetAccessTokenCommand.ExecuteAsync ( null );

                // Assert.
                Assert.True ( getAccessTokenRequestedRaised );
            }

            [Fact]
            public void GetAccessTokenRequested_WhenEventHasNoHandler_ShouldReturnCompletedTask ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new ChppTokenVerifierViewModel
                {
                    Verifier = fixture.Create<string> ( )
                };

                // Act.
                var actual = sut.GetAccessTokenCommand.ExecuteAsync ( null );

                // Assert.
                Assert.True ( actual.IsCompletedSuccessfully );
            }

            [Fact]
            public void GetAccessTokenRequested_WhenVerifierIsNullOrEmpty_ShouldNotCallHandler ( )
            {
                // Arrange.
                var sut = new ChppTokenVerifierViewModel ( );

                bool getAccessTokenRequestedRaised = false;

                sut.GetAccessTokenRequested += value =>
                {
                    getAccessTokenRequestedRaised = true;

                    return Task.CompletedTask;
                };

                // Act.
                sut.GetAccessTokenCommand.ExecuteAsync ( null );

                // Assert.
                Assert.False ( getAccessTokenRequestedRaised );
            }
        }

        public class GoBackTests
        {
            [Fact]
            public void GoBackRequested_WhenEventHasHandler_ShouldCallHandler ( )
            {
                // Arrange.
                var sut = new ChppTokenVerifierViewModel ( );

                bool goBackRequestedRaised = false;

                sut.GoBackRequested += ( ) => goBackRequestedRaised = true;

                // Act.
                sut.GoBackCommand.Execute ( null );

                // Assert.
                Assert.True ( goBackRequestedRaised );
            }

            [Fact]
            public void GoBackRequested_WhenEventHasNoHandler_ShouldNotThrow ( )
            {
                // Arrange.
                var sut = new ChppTokenVerifierViewModel ( );

                // Act.
                var actual = Record.Exception ( ( ) => sut.GoBackCommand.Execute ( null ) );

                // Assert.
                Assert.Null ( actual );
            }
        }
    }
}