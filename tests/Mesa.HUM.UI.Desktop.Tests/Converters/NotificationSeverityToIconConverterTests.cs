namespace Mesa.HUM.UI.Desktop.Tests.Converters
{
    using System;
    using Mesa.HUM.Tests.Shared.Helpers;
    using Mesa.HUM.UI.Desktop.Converters;

    public class NotificationSeverityToIconConverterTests
    {
        public class ConvertBackTests
        {
            [Fact]
            public void ConvertBack_ShouldThrowInvalidOperationException ( )
            {
                // Arrange.
                var sut = new NotificationSeverityToIconConverter ( );

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
                var sut = new NotificationSeverityToIconConverter ( );
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
                var sut = new NotificationSeverityToIconConverter ( );
                string expected = "INVALID_CAST_EXCEPTION";

                // Act.
                var actual = Record.Exception ( ( ) => sut.Convert ( null , typeof ( object ) , null , CultureHelper.GetCultureInfo ( ) ) );

                // Assert.
                Assert.NotNull ( actual );
                var exception = Assert.IsType<InvalidCastException> ( actual );
                Assert.Equal ( expected , exception.Message );
            }
        }
    }
}