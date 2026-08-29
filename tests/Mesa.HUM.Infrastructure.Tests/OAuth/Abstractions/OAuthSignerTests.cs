namespace Mesa.HUM.Infrastructure.Tests.OAuth.Abstractions
{
    using System;
    using Mesa.HUM.Infrastructure.OAuth.Abstractions;

    public class OAuthSignerTests
    {
        [Theory]
        [InlineData ( null , typeof ( ArgumentNullException ) )]
        [InlineData ( "" , typeof ( ArgumentException ) )]
        [InlineData ( "  " , typeof ( ArgumentException ) )]
        public void GenerateSignature_WhenConsumerSecretIsInvalid_ShouldThrowExceptionOfCorrectType ( string? consumerSecret , Type exceptionType )
        {
            // Arrange.
            var sut = new OAuthSigner ( );

            // Act.
            var actual = Record.Exception ( ( ) => sut.GenerateSignature ( "signatureBase" , consumerSecret! ) );

            // Assert.
            Assert.NotNull ( actual );
            Assert.IsType ( exceptionType , actual );
            Assert.Equal ( "consumerSecret" , ( ( ArgumentException ) actual ).ParamName );
        }

        [Theory]
        [InlineData ( null , typeof ( ArgumentNullException ) )]
        [InlineData ( "" , typeof ( ArgumentException ) )]
        [InlineData ( "  " , typeof ( ArgumentException ) )]
        public void GenerateSignature_WhenSignatureBaseIsInvalid_ShouldThrowExceptionOfCorrectType ( string? signatureBase , Type exceptionType )
        {
            // Arrange.
            var sut = new OAuthSigner ( );

            // Act.
            var actual = Record.Exception ( ( ) => sut.GenerateSignature ( signatureBase! , "consumerSecret" ) );

            // Assert.
            Assert.NotNull ( actual );
            Assert.IsType ( exceptionType , actual );
            Assert.Equal ( "signatureBase" , ( ( ArgumentException ) actual ).ParamName );
        }

        [Fact]
        public void GenerateSignature_WhenTokenSecretIsNotNullEmptyOrWhiteSpace_ShouldReturnValidSignature ( )
        {
            // Arrange.
            var sut = new OAuthSigner ( );
            string signatureBase = "GET&https%3A%2F%2Foauth.test.com%2Foauth%2Ffake_path&oauth_callback%3Doob%26oauth_consumer_key%3D2qRyFGnB0Yuvrd8YCSNYCx%26oauth_nonce%3Dabcdefghijklmnopqrstu%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1787676662%26oauth_version%3D1.0";
            string consumerSecret = "tLQB2g7nmPaQdhjSUvSBVd";
            string tokenSecret = "dec7bHTD7YUvu5EH";
            string expected = "BMHtmbNpkG+cjZptn9P5IL9TrZ8=";

            // Act.
            string actual = sut.GenerateSignature ( signatureBase , consumerSecret , tokenSecret );

            // Assert.
            Assert.Equal ( expected , actual );
        }

        [Theory]
        [InlineData ( null )]
        [InlineData ( "" )]
        [InlineData ( "   " )]
        public void GenerateSignature_WhenTokenSecretIsNullEmptyOrWhiteSpace_ShouldReturnSameSignature ( string? tokenSecret )
        {
            // Arrange.
            var sut = new OAuthSigner ( );
            string signatureBase = "GET&https%3A%2F%2Foauth.test.com%2Foauth%2Ffake_path&oauth_callback%3Doob%26oauth_consumer_key%3DHc0zMlrQhNHThyD2oMYhT1%26oauth_nonce%3Dabcdefghijklmnopqrstu%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1787676662%26oauth_version%3D1.0";
            string consumerSecret = "23fVGlhJuzXYchK01gnb5Q";
            string expected = "CQfmMVFF5xYFmPLxPWkBZwoH0uQ=";

            // Act.
            string actual = sut.GenerateSignature ( signatureBase , consumerSecret , tokenSecret );

            // Assert.
            Assert.Equal ( expected , actual );
        }
    }
}