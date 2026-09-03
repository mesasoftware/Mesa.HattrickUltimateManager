namespace Mesa.HUM.Presentation.Tests.ViewModels.Components.UserProfiles.Models
{
    using System;
    using Mesa.HUM.Presentation.ViewModels.Components.UserProfiles;

    public class ChppScopeItemModelTests
    {
        public class ConstructorTests
        {
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
                var sut = new ChppScopeItemModel (
                    "name" ,
                    true ,
                    "value" );

                // Assert.
                Assert.Equal ( "name" , sut.Name );
                Assert.True ( sut.RequiresSupporter );
                Assert.Equal ( "value" , sut.Value );
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
                var sut = new ChppScopeItemModel ( "name" , false , "value" );

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
                var sut = new ChppScopeItemModel ( "name" , false , "value" );

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