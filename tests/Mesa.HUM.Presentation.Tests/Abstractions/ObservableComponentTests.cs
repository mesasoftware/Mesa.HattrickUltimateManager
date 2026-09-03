namespace Mesa.HUM.Presentation.Tests.Abstractions
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Mesa.HUM.Presentation.Abstractions;
    using Mesa.HUM.Tests.Shared.Helpers;

    public class ObservableComponentTests
    {
        public class SetFieldTests
        {
            [Fact]
            public void SetField_GivenChildPropertyAndValueChanges_ShouldRaisePropertyEvents ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new ObservableComponentTest ( );

                bool childPropertyChangingRaised = false;
                bool childPropertyChangedRaised = false;

                string expected = fixture.Create<string> ( );

                sut.PropertyChanging += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.IsTextValueEmpty ) )
                    {
                        childPropertyChangingRaised = true;
                    }
                };

                sut.PropertyChanged += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.IsTextValueEmpty ) )
                    {
                        childPropertyChangedRaised = true;
                    }
                };

                // Act.
                sut.TextValue = expected;

                // Assert.
                Assert.True ( childPropertyChangingRaised );
                Assert.True ( childPropertyChangedRaised );
            }

            [Fact]
            public void SetField_GivenMultipleChildPropertiesAndValueChanges_ShouldRaisePropertyEventsForEachChild ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new ObservableComponentTest ( );

                string firstChild = fixture.Create<string> ( );
                string secondChild = fixture.Create<string> ( );

                var changingRaised = new HashSet<string> ( );
                var changedRaised = new HashSet<string> ( );

                string expected = fixture.Create<string> ( );

                sut.PropertyChanging += ( s , e ) =>
                {
                    if ( e.PropertyName is not null )
                    {
                        changingRaised.Add ( e.PropertyName );
                    }
                };

                sut.PropertyChanged += ( s , e ) =>
                {
                    if ( e.PropertyName is not null )
                    {
                        changedRaised.Add ( e.PropertyName );
                    }
                };

                // Act.
                sut.SetTextValue ( expected , "TextValue" , firstChild , secondChild );

                // Assert.
                Assert.Contains ( firstChild , changingRaised );
                Assert.Contains ( secondChild , changingRaised );
                Assert.Contains ( firstChild , changedRaised );
                Assert.Contains ( secondChild , changedRaised );
            }

            [Theory]
            [InlineData ( null , typeof ( ArgumentNullException ) )]
            [InlineData ( "" , typeof ( ArgumentException ) )]
            [InlineData ( "   " , typeof ( ArgumentException ) )]
            public void SetField_WhenChildPropertiesNamesHasNullOrEmptyValue_ShouldThrowExceptionOfCorrectTypeWithParamName ( string? childPropertyName , Type exceptionType )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new ObservableComponentTest ( );

                string expected = fixture.Create<string> ( );

                // Act.
                var actual = Record.Exception ( ( ) => sut.SetTextValue ( expected , "TextValue" , childPropertyName! ) );

                // Assert.
                Assert.NotNull ( actual );
                Assert.IsType ( exceptionType , actual );
                Assert.Equal ( "childProperty" , ( ( ArgumentException ) actual ).ParamName );
            }

            [Fact]
            public void SetField_WhenChildPropertiesNamesIsNull_ShouldTreatAsNoChildrenAndRaiseOnlyForProperty ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new ObservableComponentTest ( );

                var changingRaised = new HashSet<string> ( );
                var changedRaised = new HashSet<string> ( );

                string expected = fixture.Create<string> ( );

                sut.PropertyChanging += ( s , e ) =>
                {
                    if ( e.PropertyName is not null )
                    {
                        changingRaised.Add ( e.PropertyName );
                    }
                };

                sut.PropertyChanged += ( s , e ) =>
                {
                    if ( e.PropertyName is not null )
                    {
                        changedRaised.Add ( e.PropertyName );
                    }
                };

                // Act.
                bool actual = sut.SetTextValue ( expected , "TextValue" , null );

                // Assert.
                Assert.True ( actual );
                Assert.Equal ( new [ ] { "TextValue" } , changingRaised );
                Assert.Equal ( new [ ] { "TextValue" } , changedRaised );
            }

            [Fact]
            public void SetField_WhenPropertyNameIsInvalidAndValueIsUnchanged_ShouldStillThrow ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                string expected = fixture.Create<string> ( );

                var sut = new ObservableComponentTest
                {
                    TextValue = expected
                };

                // Act.
                var actual = Record.Exception ( ( ) => sut.SetTextValue ( expected , null ) );

                // Assert.
                Assert.IsType<ArgumentNullException> ( actual );
                Assert.Equal ( "propertyName" , ( ( ArgumentException ) actual ).ParamName );
            }

            [Theory]
            [InlineData ( null , typeof ( ArgumentNullException ) )]
            [InlineData ( "" , typeof ( ArgumentException ) )]
            [InlineData ( "   " , typeof ( ArgumentException ) )]
            public void SetField_WhenPropertyNameIsNullOrEmpty_ShouldThrowExceptionOfCorrectTypeWithParamName ( string? propertyName , Type exceptionType )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new ObservableComponentTest ( );

                string expected = fixture.Create<string> ( );

                // Act.
                var actual = Record.Exception ( ( ) => sut.SetTextValue ( expected , propertyName ) );

                // Assert.
                Assert.NotNull ( actual );
                Assert.IsType ( exceptionType , actual );
                Assert.Equal ( "propertyName" , ( ( ArgumentException ) actual ).ParamName );
            }

            [Fact]
            public void SetField_WhenValueChanges_ShouldAssignFieldBetweenChangingAndChangedEvents ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new ObservableComponentTest ( );

                string original = sut.TextValue;
                string expected = fixture.Create<string> ( );

                string? valueDuringChanging = null;
                string? valueDuringChanged = null;

                sut.PropertyChanging += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.TextValue ) )
                    {
                        valueDuringChanging = sut.TextValue;
                    }
                };

                sut.PropertyChanged += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.TextValue ) )
                    {
                        valueDuringChanged = sut.TextValue;
                    }
                };

                // Act.
                sut.TextValue = expected;

                // Assert.
                Assert.Equal ( original , valueDuringChanging );
                Assert.Equal ( expected , valueDuringChanged );
            }

            [Fact]
            public void SetField_WhenValueChanges_ShouldRaiseChangingBeforeChanged ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new ObservableComponentTest ( );

                var sequence = new List<string> ( );

                string expected = fixture.Create<string> ( );

                sut.PropertyChanging += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.TextValue ) )
                    {
                        sequence.Add ( "changing" );
                    }
                };

                sut.PropertyChanged += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.TextValue ) )
                    {
                        sequence.Add ( "changed" );
                    }
                };

                // Act.
                sut.TextValue = expected;

                // Assert.
                Assert.Equal ( new [ ] { "changing" , "changed" } , sequence );
            }

            [Fact]
            public void SetField_WhenValueChanges_ShouldRaisePropertyEventsAndAssignValue ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new ObservableComponentTest ( );

                bool propertyChangedRaised = false;
                bool propertyChangingRaised = false;

                string expected = fixture.Create<string> ( );

                sut.PropertyChanging += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.TextValue ) )
                    {
                        propertyChangingRaised = true;
                    }
                };

                sut.PropertyChanged += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.TextValue ) )
                    {
                        propertyChangedRaised = true;
                    }
                };

                // Act.
                sut.TextValue = expected;

                // Assert.
                Assert.True ( propertyChangingRaised );
                Assert.True ( propertyChangedRaised );
                Assert.Equal ( expected , sut.TextValue );
            }

            [Fact]
            public void SetField_WhenValueChanges_ShouldReturnTrue ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                var sut = new ObservableComponentTest ( );

                string expected = fixture.Create<string> ( );

                // Act.
                bool actual = sut.SetTextValue ( expected , "TextValue" );

                // Assert.
                Assert.True ( actual );
            }

            [Fact]
            public void SetField_WhenValueDoesNotChange_ShouldNotRaisePropertyEvents ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                string expected = fixture.Create<string> ( );

                var sut = new ObservableComponentTest
                {
                    TextValue = expected
                };

                bool propertyChangedRaised = false;
                bool propertyChangingRaised = false;

                sut.PropertyChanging += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.TextValue ) )
                    {
                        propertyChangingRaised = true;
                    }
                };

                sut.PropertyChanged += ( s , e ) =>
                {
                    if ( e.PropertyName == nameof ( sut.TextValue ) )
                    {
                        propertyChangedRaised = true;
                    }
                };

                // Act.
                sut.TextValue = expected;

                // Assert.
                Assert.False ( propertyChangingRaised );
                Assert.False ( propertyChangedRaised );
                Assert.Equal ( expected , sut.TextValue );
            }

            [Fact]
            public void SetField_WhenValueDoesNotChange_ShouldReturnFalse ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );

                string expected = fixture.Create<string> ( );

                var sut = new ObservableComponentTest
                {
                    TextValue = expected
                };

                // Act.
                bool actual = sut.SetTextValue ( expected , "TextValue" );

                // Assert.
                Assert.False ( actual );
            }
        }

        private class ObservableComponentTest : ObservableComponent
        {
            private string _textValue = string.Empty;

            public bool IsTextValueEmpty
            {
                get
                {
                    return _textValue.Length == 0;
                }
            }

            public string TextValue
            {
                get
                {
                    return _textValue;
                }

                set
                {
                    SetField ( ref _textValue , value , childPropertiesNames: nameof ( IsTextValueEmpty ) );
                }
            }

            public bool SetTextValue ( string value , string? propertyName , params string [ ]? childPropertiesNames )
            {
                return SetField ( ref _textValue , value , propertyName , childPropertiesNames );
            }
        }
    }
}