namespace Mesa.HUM.UI.Desktop.Tests.Converters
{
    using System;
    using Avalonia.Headless.XUnit;
    using Avalonia.Media;
    using Mesa.HUM.Presentation.Stores.Enums;
    using Mesa.HUM.Tests.Shared.Helpers;
    using Mesa.HUM.UI.Desktop.Converters;

    public class NotificationSeverityToBorderBrushConverterTests
    {
        public class ConvertBackTests
        {
            [Fact]
            public void ConvertBack_ShouldThrowInvalidOperationException ( )
            {
                // Arrange.
                var sut = new NotificationSeverityToBorderBrushConverter ( );

                // Act.
                var actual = Record.Exception ( ( ) => sut.ConvertBack ( new object ( ) , typeof ( object ) , null , CultureHelper.GetCultureInfo ( ) ) );

                // Assert.
                Assert.NotNull ( actual );
                Assert.IsType<InvalidOperationException> ( actual );
            }
        }

        public class ConvertTests
        {
            [Fact]
            public void Convert_GivenNonSeverityValue_ShouldThrowInvalidCastException ( )
            {
                // Arrange.
                var sut = new NotificationSeverityToBorderBrushConverter ( );
                string expected = "INVALID_CAST_EXCEPTION";

                // Act.
                var actual = Record.Exception ( ( ) => sut.Convert ( new object ( ) , typeof ( object ) , null , CultureHelper.GetCultureInfo ( ) ) );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<InvalidCastException> ( actual );
                Assert.Equal ( expected , exception.Message );
            }

            [Fact]
            public void Convert_GivenNullValue_ShouldThrowInvalidCastException ( )
            {
                // Arrange.
                var sut = new NotificationSeverityToBorderBrushConverter ( );
                string expected = "INVALID_CAST_EXCEPTION";

                // Act.
                var actual = Record.Exception ( ( ) => sut.Convert ( null , typeof ( object ) , null , CultureHelper.GetCultureInfo ( ) ) );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<InvalidCastException> ( actual );
                Assert.Equal ( expected , exception.Message );
            }

            [AvaloniaTheory]
            [InlineData ( NotificationSeverity.Information )]
            [InlineData ( NotificationSeverity.Success )]
            [InlineData ( NotificationSeverity.Warning )]
            [InlineData ( NotificationSeverity.Error )]
            public void Convert_GivenSeverity_ShouldReturnBrush ( NotificationSeverity severity )
            {
                // Arrange.
                var sut = new NotificationSeverityToBorderBrushConverter ( );

                // Act.
                object? actual = sut.Convert ( severity , typeof ( object ) , null , CultureHelper.GetCultureInfo ( ) );

                // Assert.
                Assert.NotNull ( actual );
                Assert.IsAssignableFrom<IBrush> ( actual );
            }

            [AvaloniaFact]
            public void Convert_GivenUnknownSeverity_ShouldReturnFallback ( )
            {
                // Arrange.
                var sut = new NotificationSeverityToBorderBrushConverter ( );

                // Act.
                object? actual = sut.Convert ( ( NotificationSeverity ) 999 , typeof ( object ) , null , CultureHelper.GetCultureInfo ( ) );

                // Assert.
                Assert.NotNull ( actual );
            }
        }
    }
}