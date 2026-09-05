namespace Mesa.HUM.Presentation.Tests.ViewModels.Components.UserProfiles
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Mesa.HUM.Presentation.ViewModels.Components.UserProfiles;

    public class ChppScopePickerViewModelTests
    {
        private static ChppScopeItemModel [ ] GetScopeItems ( params string [ ] selectedScopes )
        {
            return
            [
                new ChppScopeItemModel("ManageChallenges", false, "manage_challenges")
                {
                    IsSelected = selectedScopes.Contains("manage_challenges")
                } ,
                new ChppScopeItemModel( "SetMatchOrder" , true, "set_matchorder" )
                {
                    IsSelected = selectedScopes.Contains("set_matchorder")
                },
                new ChppScopeItemModel( "ManageYouthPlayers" , false, "manage_youthplayers" )
                {
                    IsSelected = selectedScopes.Contains("manage_youthplayers")
                },
                new ChppScopeItemModel( "SetTraining" , true, "set_training" )
                {
                    IsSelected = selectedScopes.Contains("set_training")
                },
                new ChppScopeItemModel( "PlaceBid" , true , "place_bid" )
                {
                    IsSelected = selectedScopes.Contains("place_bid")
                }
            ];
        }

        public class ConstructorTests
        {
            [Fact]
            public void Constructor_GivenScopes_ShouldNotThrowException ( )
            {
                // Arrange.
                var scopes = GetScopeItems ( );

                // Act.
                var actual = Record.Exception ( ( ) => new ChppScopePickerViewModel ( scopes ) );

                // Assert.
                Assert.Null ( actual );
            }

            [Fact]
            public void Constructor_ShouldInitializeProperties ( )
            {
                // Arrange.
                var scopes = GetScopeItems ( );

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
                var scopes = GetScopeItems ( );

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
                var scopes = GetScopeItems ( );

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
                var scopes = GetScopeItems ( "manage_challenges" , "manage_youthplayers" );

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
                var scopes = GetScopeItems ( );

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
                    GetScopeItems ( "manage_challenges" , "set_matchorder" , "manage_youthplayers" , "set_training" , "place_bid" ) );

                // Act.
                string [ ] actual = sut.SelectedScopes;

                // Assert.
                Assert.Equal ( [ "manage_challenges" , "set_matchorder" , "manage_youthplayers" , "set_training" , "place_bid" ] , actual );
            }

            [Fact]
            public void SelectedScopes_WhenItemSelectionChanges_ShouldReflectCurrentState ( )
            {
                // Arrange.
                var scopeItems = GetScopeItems ( );

                var sut = new ChppScopePickerViewModel ( scopeItems );

                var item = scopeItems.Single ( x => x.Value == "manage_youthplayers" );

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
                    GetScopeItems ( ) );

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
                    GetScopeItems ( "manage_youthplayers" , "manage_challenges" ) );

                // Act.
                string [ ] actual = sut.SelectedScopes;

                // Assert.
                Assert.Equal ( [ "manage_challenges" , "manage_youthplayers" ] , actual );
            }
        }
    }
}