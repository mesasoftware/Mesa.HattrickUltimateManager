namespace Mesa.HUM.UI.Desktop.Tests.Converters
{
    using System;
    using Mesa.HUM.Domain.Profiles.Enums;
    using Mesa.HUM.Tests.Shared.Helpers;
    using Mesa.HUM.UI.Desktop.Converters;

    public class EnumToLocalizedValueConverterTests
    {
        public class ConvertBackTests
        {
            [Fact]
            public void ConvertBack_ShouldReturnInvalidOperationException ( )
            {
                // Arrange.
                var sut = new EnumToLocalizedValueConverter ( );

                // Act & Assert.
                Assert.Throws<InvalidOperationException> ( ( ) => sut.ConvertBack ( null , typeof ( Enum ) , null , CultureHelper.GetCultureInfo ( ) ) );
            }
        }

        public class ConvertTests
        {
            [Theory]
            [InlineData ( ChppScope.ManageChallenges , "Manage challenges" )]
            [InlineData ( ChppScope.SetMatchOrder , "Set match orders" )]
            [InlineData ( ChppScope.ManageYouthPlayers , "Manage youth players" )]
            [InlineData ( ChppScope.SetTraining , "Set training" )]
            [InlineData ( ChppScope.PlaceBid , "Place bid" )]
            public void Convert_GivenParameterAndEnumHasTranslation_ShouldReturnLocalizedString ( ChppScope? value , string? expected )
            {
                // Arrange.
                var sut = new EnumToLocalizedValueConverter ( );

                // Act.
                object? actual = sut.Convert ( value , typeof ( string ) , "Title" , CultureHelper.GetCultureInfo ( ) );

                // Assert.
                string? stringValue = Assert.IsType<string?> ( actual );
                Assert.Equal ( expected , stringValue );
            }

            [Theory]
            [InlineData ( ChppScope.ReadAccess , null )]
            [InlineData ( ChppScope.ReadAccess , "Title" )]
            public void Convert_GivenValueHasNoTranslation_ShouldReturnNull ( ChppScope? value , string? parameter )
            {
                // Arrange.
                var sut = new EnumToLocalizedValueConverter ( );

                // Act.
                object? actual = sut.Convert ( value , typeof ( string ) , parameter , CultureHelper.GetCultureInfo ( ) );

                // Assert.
                Assert.Null ( null );
            }

            [Fact]
            public void Convert_GivenValueIsNull_ShouldReturnNull ( )
            {
                // Arrange.
                var sut = new EnumToLocalizedValueConverter ( );

                // Act.
                object? actual = sut.Convert ( null , typeof ( string ) , null , CultureHelper.GetCultureInfo ( ) );

                // Assert.
                Assert.Null ( null );
            }
        }
    }
}