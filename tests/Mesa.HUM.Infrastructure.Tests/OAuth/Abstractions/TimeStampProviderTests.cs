namespace Mesa.HUM.Infrastructure.Tests.OAuth.Abstractions
{
    using System;
    using Mesa.HUM.Infrastructure.OAuth.Abstractions;

    public class TimeStampProviderTests
    {
        [Fact]
        public void GetTimestamp_IsWithinReasonableRange ( )
        {
            // Arrange.
            var sut = new TimeStampProvider ( );

            long expected = DateTimeOffset.UtcNow.ToUnixTimeSeconds ( );

            // Act.
            long actual = long.Parse ( sut.GetTimeStamp ( ) );

            // Assert.
            Assert.InRange ( actual , expected - 5 , expected + 5 );
        }

        [Fact]
        public void GetTimestamp_ReturnsNumericString ( )
        {
            // Arrange.
            var sut = new TimeStampProvider ( );

            // Act.
            bool actual = long.TryParse (
                sut.GetTimeStamp ( ) , out _ );

            // Assert.
            Assert.True ( actual );
        }
    }
}