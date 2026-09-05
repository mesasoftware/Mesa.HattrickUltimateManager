namespace Mesa.HUM.Presentation.Tests.ViewModels.Components.UserProfiles.Models
{
    using System;
    using Mesa.HUM.Domain.Profiles.Enums;
    using Mesa.HUM.Presentation.ViewModels.Components.UserProfiles;

    public class ChppScopeItemModelTests
    {
        public class ConstructorTests
        {
            [Theory]
            [InlineData ( "NotAScope" )]
            [InlineData ( "managechallenges" )]
            [InlineData ( "Manage Challenges" )]
            public void Constructor_WhenNameIsNotValidScope_ShouldThrowInvalidCastException ( string name )
            {
                // Assert.
                Assert.Throws<InvalidCastException> ( ( ) => new ChppScopeItemModel (
                    name ,
                    false ,
                    "value" ) );
            }

            [Theory]
            [InlineData ( "ReadAccess" , ChppScope.ReadAccess )]
            [InlineData ( "ManageChallenges" , ChppScope.ManageChallenges )]
            [InlineData ( "SetMatchOrder" , ChppScope.SetMatchOrder )]
            [InlineData ( "ManageYouthPlayers" , ChppScope.ManageYouthPlayers )]
            [InlineData ( "SetTraining" , ChppScope.SetTraining )]
            [InlineData ( "PlaceBid" , ChppScope.PlaceBid )]
            public void Constructor_WhenNameIsValidScope_ShouldParseScopeFromName (
                string name ,
                ChppScope expectedScope )
            {
                // Act.
                var sut = new ChppScopeItemModel ( name , false , "value" );

                // Assert.
                Assert.Equal ( expectedScope , sut.Scope );
            }

            [Theory]
            [InlineData ( "" , "value" )]
            [InlineData ( "   " , "value" )]
            [InlineData ( "name" , "" )]
            [InlineData ( "name" , "   " )]
            public void Constructor_WhenParameterIsEmpty_ShouldThrowArgumentException (
            string name ,
            string value )
            {
                // Assert.
                Assert.Throws<ArgumentException> ( ( ) => new ChppScopeItemModel (
                    name ,
                    true ,
                    value ) );
            }

            [Theory]
            [InlineData ( null , "value" )]
            [InlineData ( "name" , null )]
            public void Constructor_WhenParameterIsNull_ShouldThrowArgumentNullException (
                string? name ,
                string? value )
            {
                // Assert.
                Assert.Throws<ArgumentNullException> ( ( ) => new ChppScopeItemModel (
                    name! ,
                    true ,
                    value! ) );
            }

            [Fact]
            public void Constructor_WhenParametersAreValid_ShouldPopulateProperties ( )
            {
                // Act.
                var sut = new ChppScopeItemModel ( "ManageChallenges" , true , "manage_challenges" );

                // Assert.
                Assert.Equal ( "ManageChallenges" , sut.Name );
                Assert.True ( sut.RequiresSupporter );
                Assert.Equal ( "manage_challenges" , sut.Value );
                Assert.Equal ( ChppScope.ManageChallenges , sut.Scope );
                Assert.False ( sut.IsSelected );
            }
        }

        public class IsSelectedTests
        {
            [Theory]
            [InlineData ( false )]
            [InlineData ( true )]
            public void IsSelected_WhenValueChanges_ShouldRaisePropertyEventsAndSetValue ( bool value )
            {
                // Arrange.
                var sut = new ChppScopeItemModel ( "ManageChallenges" , false , "manage_challenges" );

                bool propertyChangedRaised = false;
                bool propertyChangingRaised = false;

                sut.IsSelected = !value;

                sut.PropertyChanged += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.IsSelected ) )
                    {
                        propertyChangedRaised = true;
                    }
                };

                sut.PropertyChanging += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.IsSelected ) )
                    {
                        propertyChangingRaised = true;
                    }
                };

                // Act.
                sut.IsSelected = value;

                // Assert.
                Assert.True ( propertyChangedRaised );
                Assert.True ( propertyChangingRaised );
                Assert.Equal ( value , sut.IsSelected );
            }

            [Theory]
            [InlineData ( false )]
            [InlineData ( true )]
            public void IsSelected_WhenValueDoesNotChange_ShouldNotRaisePropertyEvents ( bool value )
            {
                // Arrange.
                var sut = new ChppScopeItemModel ( "ManageChallenges" , false , "manage_challenges" );

                bool propertyChangedRaised = false;
                bool propertyChangingRaised = false;

                sut.IsSelected = value;

                sut.PropertyChanged += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.IsSelected ) )
                    {
                        propertyChangedRaised = true;
                    }
                };

                sut.PropertyChanging += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.IsSelected ) )
                    {
                        propertyChangingRaised = true;
                    }
                };

                // Act.
                sut.IsSelected = value;

                // Assert.
                Assert.False ( propertyChangedRaised );
                Assert.False ( propertyChangingRaised );
                Assert.Equal ( value , sut.IsSelected );
            }
        }
    }
}