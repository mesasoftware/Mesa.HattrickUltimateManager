namespace Mesa.HUM.Presentation.Tests.ViewModels.Components.UserProfiles
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using Mesa.HUM.Presentation.ViewModels.Components.UserProfiles;
    using Mesa.HUM.Tests.Shared.Helpers;

    public class ChppScopePickerViewModelTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void Constructor_GivenScopes_ShouldNotThrowException ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var scopes = fixture
                    .CreateMany<ChppScopeItemModel> ( 5 )
                    .ToArray ( );

                // Act.
                var actual = Record.Exception ( ( ) => new ChppScopePickerViewModel ( scopes ) );

                // Assert.
                Assert.Null ( actual );
            }

            [Fact]
            public void Constructor_ShouldInitializeProperties ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var scopes = fixture
                    .CreateMany<ChppScopeItemModel> ( 5 )
                    .ToArray ( );

                // Act.
                var sut = new ChppScopePickerViewModel ( scopes );

                // Assert.
                Assert.Same ( scopes , sut.Scopes );
                Assert.NotNull ( sut.CopyLinkCommand );
                Assert.NotNull ( sut.OpenLinkCommand );
            }

            [Fact]
            public void Constructor_WhenScopesParameterIsEmpty_ShouldThrowExceptionOfCorrectType ( )
            {
                // Arrange.
                ChppScopeItemModel [ ]? scopes = [ ];
                string expected = "scopes";

                // Act.
                var actual = Record.Exception ( ( ) => new ChppScopePickerViewModel ( scopes! ) );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<ArgumentException> ( actual );
                Assert.Equal ( expected , exception.ParamName );
            }

            [Fact]
            public void Constructor_WhenScopesParameterIsNull_ShouldThrowExceptionOfCorrectType ( )
            {
                // Arrange.
                ChppScopeItemModel [ ]? scopes = null;
                string expected = "scopes";

                // Act.
                var actual = Record.Exception ( ( ) => new ChppScopePickerViewModel ( scopes! ) );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<ArgumentNullException> ( actual );
                Assert.Equal ( expected , exception.ParamName );
            }
        }

        public class CopyLinkRequestedTests
        {
            [Fact]
            public async Task CopyLinkRequested_WhenEventHasHandler_ShouldCallHandlerAndSendSelectedScopes ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var scopes = fixture
                    .CreateMany<ChppScopeItemModel> ( 5 )
                    .ToArray ( );

                scopes [ 0 ].IsSelected = true;
                scopes [ 2 ].IsSelected = true;

                var sut = new ChppScopePickerViewModel ( scopes );

                bool copyLinkRequestedRaised = false;

                sut.CopyLinkRequested += value =>
                {
                    if ( value is not null && value.SequenceEqual ( sut.SelectedScopes! ) )
                    {
                        copyLinkRequestedRaised = true;
                    }

                    return Task.CompletedTask;
                };

                // Act.
                await sut.CopyLinkCommand.ExecuteAsync ( null );

                // Assert.
                Assert.True ( copyLinkRequestedRaised );
            }

            [Fact]
            public void CopyLinkRequested_WhenEventHasNoHandler_ShouldReturnCompletedTask ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var scopes = fixture
                    .CreateMany<ChppScopeItemModel> ( 5 )
                    .ToArray ( );

                var sut = new ChppScopePickerViewModel ( scopes );

                // Act.
                var actual = sut.CopyLinkCommand.ExecuteAsync ( null );

                // Assert.
                Assert.True ( actual.IsCompletedSuccessfully );
            }
        }

        public class OpenLinkRequestedTests
        {
            [Fact]
            public async Task OpenLinkRequested_WhenEventHasHandler_ShouldCallHandlerAndSendSelectedScopes ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var scopes = fixture
                    .CreateMany<ChppScopeItemModel> ( 5 )
                    .ToArray ( );

                scopes [ 0 ].IsSelected = true;
                scopes [ 2 ].IsSelected = true;

                var sut = new ChppScopePickerViewModel ( scopes );

                bool openLinkRequestedRaised = false;

                sut.OpenLinkRequested += value =>
                {
                    if ( value is not null && value.SequenceEqual ( sut.SelectedScopes! ) )
                    {
                        openLinkRequestedRaised = true;
                    }

                    return Task.CompletedTask;
                };

                // Act.
                await sut.OpenLinkCommand.ExecuteAsync ( null );

                // Assert.
                Assert.True ( openLinkRequestedRaised );
            }

            [Fact]
            public void OpenLinkRequested_WhenEventHasNoHandler_ShouldReturnCompletedTask ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var scopes = fixture
                    .CreateMany<ChppScopeItemModel> ( 5 )
                    .ToArray ( );

                var sut = new ChppScopePickerViewModel ( scopes );

                // Act.
                var actual = sut.OpenLinkCommand.ExecuteAsync ( null );

                // Assert.
                Assert.True ( actual.IsCompletedSuccessfully );
            }
        }

        public class SelectedScopesTests
        {
            [Fact]
            public void SelectedScopes_WhenAllItemsAreSelected_ShouldReturnAllValues ( )
            {
                // Arrange.
                var sut = new ChppScopePickerViewModel (
                [
                    new ChppScopeItemModel ( "Manage Youth Players" , false , "manage_youthplayers" ) { IsSelected = true },
                    new ChppScopeItemModel ( "Set Matchorder" , true , "set_matchorder" ) { IsSelected = true },
                ] );

                // Act.
                string [ ] actual = sut.SelectedScopes;

                // Assert.
                Assert.Equal ( [ "manage_youthplayers" , "set_matchorder" ] , actual );
            }

            [Fact]
            public void SelectedScopes_WhenItemSelectionChanges_ShouldReflectCurrentState ( )
            {
                // Arrange.
                var item = new ChppScopeItemModel ( "Manage Youth Players" , false , "manage_youthplayers" );

                var sut = new ChppScopePickerViewModel ( [ item ] );

                // Act & Assert.
                Assert.Empty ( sut.SelectedScopes! );

                item.IsSelected = true;

                Assert.Equal ( [ "manage_youthplayers" ] , sut.SelectedScopes );
            }

            [Fact]
            public void SelectedScopes_WhenNoItemsAreSelected_ShouldReturnEmpty ( )
            {
                // Arrange.
                var sut = new ChppScopePickerViewModel (
                [
                    new ChppScopeItemModel ( "Manage Youth Players" , false , "manage_youthplayers" ),
                    new ChppScopeItemModel ( "Set Matchorder" , true , "set_matchorder" ),
                ] );

                // Act.
                string [ ] actual = sut.SelectedScopes;

                // Assert.
                Assert.Empty ( actual! );
            }

            [Fact]
            public void SelectedScopes_WhenSomeItemsAreSelected_ShouldReturnOnlySelectedValues ( )
            {
                // Arrange.
                var sut = new ChppScopePickerViewModel (
                [
                    new ChppScopeItemModel ( "Manage Youth Players" , false , "manage_youthplayers" ) { IsSelected = true },
                    new ChppScopeItemModel ( "Set Matchorder" , true , "set_matchorder" ),
                    new ChppScopeItemModel ( "Manage Challenges" , true , "manage_challenges" ) { IsSelected = true },
                ] );

                // Act.
                string [ ] actual = sut.SelectedScopes;

                // Assert.
                Assert.Equal ( [ "manage_youthplayers" , "manage_challenges" ] , actual );
            }
        }
    }
}