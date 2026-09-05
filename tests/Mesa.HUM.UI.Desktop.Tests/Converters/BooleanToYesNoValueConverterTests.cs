namespace Mesa.HUM.UI.Desktop.Tests.Converters
{
    using System;
    using AutoFixture;
    using Mesa.HUM.Tests.Shared.Helpers;
    using Mesa.HUM.UI.Desktop.Converters;
    using Mesa.HUM.UI.Desktop.Resources;

    public class BooleanToYesNoValueConverterTests
    {
        public class ConvertBackTests
        {
            [Fact]
            public void Convert_GivenInvalidString_ShouldArgumentOutOfRangeException ( )
            {
                // Arrange.
                var fixture = FixtureHelper.GetFixture ( );
                string value = fixture.Create<string> ( );

                var sut = new BooleanToYesNoValueConverter ( );

                // Act.
                var actual = Record.Exception ( ( ) => sut.ConvertBack ( value , typeof ( bool ) , null , CultureHelper.GetCultureInfo ( ) ) );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<ArgumentOutOfRangeException> ( actual );
                Assert.Equal ( "value" , exception.ParamName );
            }

            [Fact]
            public void Convert_GivenInvalidTypeValue_ShouldArgumentException ( )
            {
                // Arrange.
                var sut = new BooleanToYesNoValueConverter ( );

                // Act.
                var actual = Record.Exception ( ( ) => sut.ConvertBack ( new object ( ) , typeof ( bool ) , null , CultureHelper.GetCultureInfo ( ) ) );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<ArgumentException> ( actual );
                Assert.Equal ( "value" , exception.ParamName );
            }

            [Fact]
            public void Convert_GivenNoValue_ShouldReturnFalse ( )
            {
                // Arrange.
                var sut = new BooleanToYesNoValueConverter ( );
                bool expected = false;

                // Act.
                object? actual = sut.ConvertBack ( Translations.No , typeof ( bool ) , null , CultureHelper.GetCultureInfo ( ) );

                // Assert.
                Assert.NotNull ( actual );
                bool value = Assert.IsType<bool> ( actual );
                Assert.Equal ( expected , value );
            }

            [Fact]
            public void Convert_GivenNullString_ShouldArgumentException ( )
            {
                // Arrange.
                var sut = new BooleanToYesNoValueConverter ( );

                // Act.
                var actual = Record.Exception ( ( ) => sut.ConvertBack ( null! , typeof ( bool ) , null , CultureHelper.GetCultureInfo ( ) ) );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<ArgumentException> ( actual );
                Assert.Equal ( "value" , exception.ParamName );
            }

            [Fact]
            public void Convert_GivenYesValue_ShouldReturnTrue ( )
            {
                // Arrange.
                var sut = new BooleanToYesNoValueConverter ( );
                bool expected = true;

                // Act.
                object? actual = sut.ConvertBack ( Translations.Yes , typeof ( bool ) , null , CultureHelper.GetCultureInfo ( ) );

                // Assert.
                Assert.NotNull ( actual );
                bool value = Assert.IsType<bool> ( actual );
                Assert.Equal ( expected , value );
            }
        }

        public class ConvertTests
        {
            [Fact]
            public void Convert_GivenFalseValue_ShouldReturnNo ( )
            {
                // Arrange.
                var sut = new BooleanToYesNoValueConverter ( );
                string expected = Translations.No;

                // Act.
                object? actual = sut.Convert ( false , typeof ( string ) , null , CultureHelper.GetCultureInfo ( ) );

                // Assert.
                Assert.NotNull ( actual );
                string value = Assert.IsType<string> ( actual );
                Assert.Equal ( expected , value );
            }

            [Fact]
            public void Convert_GivenNotBooleanValue_ShouldThrowInvalidCastException ( )
            {
                // Arrange.
                var sut = new BooleanToYesNoValueConverter ( );
                string expected = "INVALID_CAST_EXCEPTION";

                // Act.
                var actual = Record.Exception ( ( ) => sut.Convert ( new object ( ) , typeof ( string ) , null , CultureHelper.GetCultureInfo ( ) ) );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<InvalidCastException> ( actual );
                Assert.Equal ( expected , exception.Message );
            }

            [Fact]
            public void Convert_GivenNullValue_ShouldThrowInvalidCastException ( )
            {
                // Arrange.
                var sut = new BooleanToYesNoValueConverter ( );
                string expected = "INVALID_CAST_EXCEPTION";

                // Act.
                var actual = Record.Exception ( ( ) => sut.Convert ( null , typeof ( string ) , null , CultureHelper.GetCultureInfo ( ) ) );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<InvalidCastException> ( actual );
                Assert.Equal ( expected , exception.Message );
            }

            [Fact]
            public void Convert_GivenTrueValue_ShouldReturnYes ( )
            {
                // Arrange.
                var sut = new BooleanToYesNoValueConverter ( );
                string expected = Translations.Yes;

                // Act.
                object? actual = sut.Convert ( true , typeof ( string ) , null , CultureHelper.GetCultureInfo ( ) );

                // Assert.
                Assert.NotNull ( actual );
                string value = Assert.IsType<string> ( actual );
                Assert.Equal ( expected , value );
            }
        }
    }
}