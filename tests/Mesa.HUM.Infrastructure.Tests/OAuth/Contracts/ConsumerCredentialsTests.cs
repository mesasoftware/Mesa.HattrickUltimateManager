namespace Mesa.HUM.Infrastructure.Tests.OAuth.Contracts
{
    using System;
    using Mesa.HUM.Infrastructure.OAuth.Contracts;

    public class ConsumerCredentialsTests
    {
        private const string consumerKeyValue = "consumerKey";

        private const string consumerSecretValue = "consumerSecret";

        private const string emptyValue = "";

        private const string userAgentValue = "userAgent";

        [Theory]
        [InlineData ( emptyValue , consumerSecretValue , userAgentValue )]
        [InlineData ( consumerKeyValue , emptyValue , userAgentValue )]
        [InlineData ( consumerKeyValue , consumerSecretValue , emptyValue )]
        public void Constructor_WhenParameterIsEmpty_ShouldThrowArgumentException (
        string consumerKey ,
        string consumerSecret ,
        string userAgent )
        {
            // Assert.
            Assert.Throws<ArgumentException> ( ( ) => new ConsumerCredentials (
                consumerKey ,
                consumerSecret ,
                userAgent ) );
        }

        [Theory]
        [InlineData ( null , consumerSecretValue , userAgentValue )]
        [InlineData ( consumerKeyValue , null , userAgentValue )]
        [InlineData ( consumerKeyValue , consumerSecretValue , null )]
        public void Constructor_WhenParameterIsNull_ShouldThrowArgumentNullException (
            string? consumerKey ,
            string? consumerSecret ,
            string? userAgent )
        {
            // Assert.
            Assert.Throws<ArgumentNullException> ( ( ) => new ConsumerCredentials (
                consumerKey! ,
                consumerSecret! ,
                userAgent! ) );
        }

        [Fact]
        public void Constructor_WhenParametersAreValid_ShouldPopulateProperties ( )
        {
            // Act.
            var sut = new ConsumerCredentials (
                consumerKeyValue ,
                consumerSecretValue ,
                userAgentValue );

            // Assert.
            Assert.Equal ( consumerKeyValue , sut.ConsumerKey );
            Assert.Equal ( consumerSecretValue , sut.ConsumerSecret );
            Assert.Equal ( userAgentValue , sut.UserAgent );
        }
    }
}