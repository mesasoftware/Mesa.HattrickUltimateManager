namespace Mesa.HUM.Application.Tests.Features.Profiles
{
    using Mesa.HUM.Application.Features.Profiles.Authorization.CompleteChppAuthorization;
    using Mesa.HUM.Application.Hattrick.Contracts;

    public class CompleteChppAuthorizationValidatorTests
    {
        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldPass ( )
        {
            // Arrange.
            var sut = new CompleteChppAuthorizationValidator ( );

            var command = new CompleteChppAuthorizationCommand ( new RequestToken ( "token" , "secret" ) , "verifier" );

            // Act.
            var actual = sut.Validate ( command );

            // Assert.
            Assert.True ( actual.IsValid );
            Assert.Empty ( actual.Errors );
        }

        [Fact]
        public void Validate_WhenRequestTokenIsNull_ShouldFailWithoutThrowing ( )
        {
            // Arrange.
            var sut = new CompleteChppAuthorizationValidator ( );

            var command = new CompleteChppAuthorizationCommand ( null! , "verifier" );

            // Act.
            var actual = sut.Validate ( command );

            // Assert.
            Assert.False ( actual.IsValid );
            Assert.Contains ( actual.Errors , e => e.PropertyName == "RequestToken" );
        }

        [Theory]
        [InlineData ( "" )]
        [InlineData ( " " )]
        public void Validate_WhenTokenIsEmptyOrWhitespace_ShouldFail ( string token )
        {
            // Arrange.
            var sut = new CompleteChppAuthorizationValidator ( );

            var command = new CompleteChppAuthorizationCommand ( new RequestToken ( token , "secret" ) , "verifier" );

            // Act.
            var actual = sut.Validate ( command );

            // Assert.
            Assert.False ( actual.IsValid );
            Assert.Contains ( actual.Errors , e => e.PropertyName == "RequestToken.Token" );
        }

        [Theory]
        [InlineData ( "" )]
        [InlineData ( " " )]
        public void Validate_WhenTokenSecretIsEmptyOrWhitespace_ShouldFail ( string tokenSecret )
        {
            // Arrange.
            var sut = new CompleteChppAuthorizationValidator ( );

            var command = new CompleteChppAuthorizationCommand ( new RequestToken ( "token" , tokenSecret ) , "verifier" );

            // Act.
            var actual = sut.Validate ( command );

            // Assert.
            Assert.False ( actual.IsValid );
            Assert.Contains ( actual.Errors , e => e.PropertyName == "RequestToken.TokenSecret" );
        }

        [Theory]
        [InlineData ( null )]
        [InlineData ( "" )]
        [InlineData ( " " )]
        public void Validate_WhenVerifierIsNullEmptyOrWhitespace_ShouldFail ( string? verifier )
        {
            // Arrange.
            var sut = new CompleteChppAuthorizationValidator ( );

            var command = new CompleteChppAuthorizationCommand ( new RequestToken ( "token" , "secret" ) , verifier! );

            // Act.
            var actual = sut.Validate ( command );

            // Assert.
            Assert.False ( actual.IsValid );
            Assert.Contains ( actual.Errors , e => e.PropertyName == "Verifier" );
        }
    }
}