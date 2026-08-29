namespace Mesa.HUM.Infrastructure.Tests.OAuth.Contracts
{
    using System;
    using Mesa.HUM.Infrastructure.OAuth.Contracts;

    public class OAuthTokenTests
    {
        private const string emptyValue = "";

        private const string tokenSecretValue = "consumerSecret";

        private const string tokenValue = "consumerKey";

        [Theory]
        [InlineData ( emptyValue , tokenSecretValue )]
        [InlineData ( tokenValue , emptyValue )]
        public void Constructor_WhenParameterIsEmpty_ShouldThrowArgumentException (
        string token ,
        string tokenSecret )
        {
            // Assert.
            Assert.Throws<ArgumentException> ( ( ) => new OAuthToken (
                token ,
                tokenSecret ) );
        }

        [Theory]
        [InlineData ( null , tokenSecretValue )]
        [InlineData ( tokenValue , null )]
        public void Constructor_WhenParameterIsNull_ShouldThrowArgumentNullException (
            string? token ,
            string? tokenSecret )
        {
            // Assert.
            Assert.Throws<ArgumentNullException> ( ( ) => new OAuthToken (
                token! ,
                tokenSecret! ) );
        }

        [Fact]
        public void Constructor_WhenParametersAreValid_ShouldPopulateProperties ( )
        {
            // Act.
            var sut = new OAuthToken (
                tokenValue ,
                tokenSecretValue );

            // Assert.
            Assert.Equal ( tokenValue , sut.Token );
            Assert.Equal ( tokenSecretValue , sut.TokenSecret );
        }
    }
}