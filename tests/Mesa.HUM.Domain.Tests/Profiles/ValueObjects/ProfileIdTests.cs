namespace Mesa.HUM.Domain.Tests.Profiles.ValueObjects
{
    using System;
    using Mesa.HUM.Domain.Profiles.ValueObjects;

    public class ProfileIdTests
    {
        [Fact]
        public void Constructor_GivenGuidEmptyParameter_ShouldThrowArgumentExceptionWithCorrectMessage ( )
        {
            // Arrange & Act.
            var exception = Record.Exception ( ( ) => new ProfileId ( Guid.Empty ) );
            string expected = "VALUE_CANNOT_BE_GUID_EMPTY";

            // Assert.
            Assert.NotNull ( exception );
            var actual = Assert.IsType<ArgumentException> ( exception );
            Assert.Equal ( expected , actual.Message );
        }

        [Fact]
        public void Constructor_GivenValue_ShouldBeEqual ( )
        {
            // Arrange.
            var expected = Guid.NewGuid ( );

            // Act.
            var sut = new ProfileId ( expected );

            // Assert.
            Assert.Equal ( expected , sut.Value );
        }

        [Fact]
        public void Constructor_Value_ShouldNotBeGuidEmpty ( )
        {
            // Arrange & Act.
            var sut = ProfileId.New ( );
            var expected = Guid.Empty;

            // Assert.
            Assert.NotEqual ( expected , sut.Value );
        }

        [Fact]
        public void ToString_ShouldMatchValue ( )
        {
            // Arrange.
            var sut = ProfileId.New ( );
            string expected = sut.Value.ToString ( );

            // Act.
            string actual = sut.ToString ( );

            // Assert.
            Assert.Equal ( expected , actual );
        }
    }
}