namespace Mesa.HUM.Infrastructure.Tests.OAuth.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Mesa.HUM.Infrastructure.OAuth.Abstractions;
    using Mesa.HUM.Infrastructure.OAuth.Abstractions.Interfaces;
    using Mesa.HUM.Infrastructure.OAuth.Contracts;
    using Moq;

    public class OAuthAuthorizationHeaderBuilderTests
    {
        private const string ExpectedHeaderWithDefaults =
            "OAuth oauth_consumer_key=\"testConsumerKey\", oauth_nonce=\"testNonce\", oauth_signature=\"testSignature\", oauth_signature_method=\"HMAC-SHA1\", oauth_timestamp=\"1000000000\", oauth_version=\"1.0\"";

        private const string Nonce = "testNonce";

        private const string Signature = "testSignature";

        private const string TimeStamp = "1000000000";

        private const string Url = "https://test.host.com/path";

        private readonly ConsumerCredentials _consumer = new ( "testConsumerKey" , "testConsumerSecret" , "testUserAgent" );

        private readonly Mock<INonceProvider> _nonceProvider = new ( );

        private readonly Mock<IOAuthSigner> _oauthSigner = new ( );

        private readonly Mock<IOAuthSignatureBaseGenerator> _signatureBaseGenerator = new ( );

        private readonly Mock<ITimeStampProvider> _timeStampProvider = new ( );

        private readonly OAuthToken _token = new ( "testToken" , "testTokenSecret" );

        public OAuthAuthorizationHeaderBuilderTests ( )
        {
            _nonceProvider.Setup ( x => x.GenerateNonce ( ) ).Returns ( Nonce );
            _timeStampProvider.Setup ( x => x.GetTimeStamp ( ) ).Returns ( TimeStamp );

            _signatureBaseGenerator
                .Setup ( x => x.GenerateSignatureBase ( It.IsAny<HttpMethod> ( ) , It.IsAny<Uri> ( ) , It.IsAny<Dictionary<string , string>> ( ) ) )
                .Returns ( "signatureBase" );

            _oauthSigner
                .Setup ( x => x.GenerateSignature ( It.IsAny<string> ( ) , It.IsAny<string> ( ) , It.IsAny<string?> ( ) ) )
                .Returns ( Signature );
        }

        [Fact]
        public void Build_WhenCallBackUrlIsSet_ShouldIncludeOAuthCallback ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForUrl ( Url );
            sut.WithConsumer ( _consumer );
            sut.WithCallBackUrl ( "oob" );

            // Act.
            string actual = sut.Build ( );

            // Assert.
            Assert.Contains ( "oauth_callback=\"oob\"" , actual );
        }

        [Fact]
        public void Build_WhenCalledTwice_ShouldClearStateBetweenBuilds ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForUrl ( Url );
            sut.WithConsumer ( _consumer );
            sut.WithToken ( _token );
            sut.WithVerifier ( "testVerifier" );
            sut.WithCallBackUrl ( "oob" );
            sut.Build ( );

            // Act.
            sut.ForUrl ( Url );
            sut.WithConsumer ( _consumer );
            string actual = sut.Build ( );

            // Assert.
            Assert.Equal ( ExpectedHeaderWithDefaults , actual );
        }

        [Fact]
        public void Build_WhenConsumerCredentialsAreNotSet_ShouldThrowArgumentNullException ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForUrl ( Url );

            // Act.
            var actual = Record.Exception ( sut.Build );

            // Assert.
            Assert.NotNull ( actual );
            Assert.IsType<ArgumentNullException> ( actual );
        }

        [Fact]
        public void Build_WhenHttpMethodIsNotSet_ShouldDefaultToGet ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForUrl ( Url );
            sut.WithConsumer ( _consumer );

            // Act.
            sut.Build ( );

            // Assert.
            _signatureBaseGenerator.Verify (
                x => x.GenerateSignatureBase ( HttpMethod.Get , It.IsAny<Uri> ( ) , It.IsAny<Dictionary<string , string>> ( ) ) ,
                Times.Once );
        }

        [Fact]
        public void Build_WhenHttpMethodIsSet_ShouldPassItToSignatureBaseGenerator ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForUrl ( Url );
            sut.WithConsumer ( _consumer );
            sut.ForHttpMethod ( HttpMethod.Post );

            // Act.
            sut.Build ( );

            // Assert.
            _signatureBaseGenerator.Verify (
                x => x.GenerateSignatureBase ( HttpMethod.Post , It.IsAny<Uri> ( ) , It.IsAny<Dictionary<string , string>> ( ) ) ,
                Times.Once );
        }

        [Fact]
        public void Build_WhenOnlyRequiredParametersAreSet_ShouldReturnHeaderWithDefaultValues ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForUrl ( Url );
            sut.WithConsumer ( _consumer );

            // Act.
            string actual = sut.Build ( );

            // Assert.
            Assert.Equal ( ExpectedHeaderWithDefaults , actual );
        }

        [Fact]
        public void Build_WhenOptionalParametersAreNotSet_ShouldNotIncludeThem ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForUrl ( Url );
            sut.WithConsumer ( _consumer );

            // Act.
            string actual = sut.Build ( );

            // Assert.
            Assert.DoesNotContain ( "oauth_token" , actual );
            Assert.DoesNotContain ( "oauth_verifier" , actual );
            Assert.DoesNotContain ( "oauth_callback" , actual );
        }

        [Fact]
        public void Build_WhenSignatureIsGenerated_ShouldIncludeItInTheHeader ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForUrl ( Url );
            sut.WithConsumer ( _consumer );

            // Act.
            string actual = sut.Build ( );

            // Assert.
            Assert.Contains ( $"oauth_signature=\"{Signature}\"" , actual );
        }

        [Fact]
        public void Build_WhenSignatureMethodIsSet_ShouldOverrideDefault ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForUrl ( Url );
            sut.WithConsumer ( _consumer );
            sut.WithSignatureMethod ( "RSA-SHA1" );

            // Act.
            string actual = sut.Build ( );

            // Assert.
            Assert.Contains ( "oauth_signature_method=\"RSA-SHA1\"" , actual );
            Assert.DoesNotContain ( "HMAC-SHA1" , actual );
        }

        [Fact]
        public void Build_WhenTokenIsNotSet_ShouldUseNullTokenSecretForSignature ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForUrl ( Url );
            sut.WithConsumer ( _consumer );

            // Act.
            sut.Build ( );

            // Assert.
            _oauthSigner.Verify (
                x => x.GenerateSignature ( It.IsAny<string> ( ) , "testConsumerSecret" , It.Is<string?> ( s => s == null ) ) ,
                Times.Once );
        }

        [Fact]
        public void Build_WhenTokenIsSet_ShouldIncludeOAuthTokenAndUseTokenSecretForSignature ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForUrl ( Url );
            sut.WithConsumer ( _consumer );
            sut.WithToken ( _token );

            // Act.
            string actual = sut.Build ( );

            // Assert.
            Assert.Contains ( "oauth_token=\"testToken\"" , actual );
            _oauthSigner.Verify (
                x => x.GenerateSignature ( It.IsAny<string> ( ) , "testConsumerSecret" , "testTokenSecret" ) ,
                Times.Once );
        }

        [Fact]
        public void Build_WhenUrlContainsQueryParameters_ShouldMergeThemIntoSignatureParameters ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForUrl ( "https://test.host.com/path?extraParam=extraValue" );
            sut.WithConsumer ( _consumer );

            // Act.
            sut.Build ( );

            // Assert.
            _signatureBaseGenerator.Verify (
                x => x.GenerateSignatureBase (
                    It.IsAny<HttpMethod> ( ) ,
                    It.IsAny<Uri> ( ) ,
                    It.Is<Dictionary<string , string>> ( p => p.ContainsKey ( "extraParam" ) && p [ "extraParam" ] == "extraValue" ) ) ,
                Times.Once );
        }

        [Fact]
        public void Build_WhenUrlIsNotSet_ShouldThrowArgumentNullException ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.WithConsumer ( _consumer );

            // Act.
            var actual = Record.Exception ( sut.Build );

            // Assert.
            Assert.NotNull ( actual );
            Assert.IsType<ArgumentNullException> ( actual );
        }

        [Fact]
        public void Build_WhenVerifierIsSet_ShouldIncludeOAuthVerifier ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForUrl ( Url );
            sut.WithConsumer ( _consumer );
            sut.WithVerifier ( "testVerifier" );

            // Act.
            string actual = sut.Build ( );

            // Assert.
            Assert.Contains ( "oauth_verifier=\"testVerifier\"" , actual );
        }

        [Fact]
        public void Build_WhenVersionIsSet_ShouldOverrideDefault ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForUrl ( Url );
            sut.WithConsumer ( _consumer );
            sut.WithVersion ( "2.0" );

            // Act.
            string actual = sut.Build ( );

            // Assert.
            Assert.Contains ( "oauth_version=\"2.0\"" , actual );
        }

        [Fact]
        public void ForHttpMethod_WhenHttpMethodIsAlreadySet_ShouldThrowInvalidOperationException ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForHttpMethod ( HttpMethod.Get );

            // Act.
            var actual = Record.Exception ( ( ) => sut.ForHttpMethod ( HttpMethod.Post ) );

            // Assert.
            var exception = Assert.IsType<InvalidOperationException> ( actual );
            Assert.Equal ( "HTTP_METHOD_OAUTH_PARAMETER_ALREADY_SET" , exception.Message );
        }

        [Fact]
        public void ForUrl_WhenUrlIsAlreadySet_ShouldThrowInvalidOperationException ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.ForUrl ( Url );

            // Act.
            var actual = Record.Exception ( ( ) => sut.ForUrl ( Url ) );

            // Assert.
            var exception = Assert.IsType<InvalidOperationException> ( actual );
            Assert.Equal ( "URL_OAUTH_PARAMETER_ALREADY_SET" , exception.Message );
        }

        [Fact]
        public void WithCallBackUrl_WhenCallBackUrlIsAlreadySet_ShouldThrowInvalidOperationException ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.WithCallBackUrl ( "oob" );

            // Act.
            var actual = Record.Exception ( ( ) => sut.WithCallBackUrl ( "oob" ) );

            // Assert.
            var exception = Assert.IsType<InvalidOperationException> ( actual );
            Assert.Equal ( "CALLBACK_URL_OAUTH_PARAMETER_ALREADY_SET" , exception.Message );
        }

        [Fact]
        public void WithConsumer_WhenConsumerCredentialsAreAlreadySet_ShouldThrowInvalidOperationException ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.WithConsumer ( _consumer );

            // Act.
            var actual = Record.Exception ( ( ) => sut.WithConsumer ( _consumer ) );

            // Assert.
            var exception = Assert.IsType<InvalidOperationException> ( actual );
            Assert.Equal ( "CONSUMER_CREDENTIALS_OAUTH_PARAMETER_ALREADY_SET" , exception.Message );
        }

        [Fact]
        public void WithSignatureMethod_WhenSignatureMethodIsAlreadySet_ShouldThrowInvalidOperationException ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.WithSignatureMethod ( "HMAC-SHA1" );

            // Act.
            var actual = Record.Exception ( ( ) => sut.WithSignatureMethod ( "RSA-SHA1" ) );

            // Assert.
            var exception = Assert.IsType<InvalidOperationException> ( actual );
            Assert.Equal ( "SIGNATURE_METHOD_OAUTH_PARAMETER_ALREADY_SET" , exception.Message );
        }

        [Fact]
        public void WithToken_WhenTokenIsAlreadySet_ShouldThrowInvalidOperationException ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.WithToken ( _token );

            // Act.
            var actual = Record.Exception ( ( ) => sut.WithToken ( _token ) );

            // Assert.
            var exception = Assert.IsType<InvalidOperationException> ( actual );
            Assert.Equal ( "TOKEN_OAUTH_PARAMETER_ALREADY_SET" , exception.Message );
        }

        [Fact]
        public void WithVerifier_WhenVerifierIsAlreadySet_ShouldThrowInvalidOperationException ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.WithVerifier ( "testVerifier" );

            // Act.
            var actual = Record.Exception ( ( ) => sut.WithVerifier ( "testVerifier" ) );

            // Assert.
            var exception = Assert.IsType<InvalidOperationException> ( actual );
            Assert.Equal ( "VERIFIER_OAUTH_PARAMETER_ALREADY_SET" , exception.Message );
        }

        [Fact]
        public void WithVersion_WhenVersionIsAlreadySet_ShouldThrowInvalidOperationException ( )
        {
            // Arrange.
            var sut = CreateSut ( );
            sut.WithVersion ( "1.0" );

            // Act.
            var actual = Record.Exception ( ( ) => sut.WithVersion ( "2.0" ) );

            // Assert.
            var exception = Assert.IsType<InvalidOperationException> ( actual );
            Assert.Equal ( "VERSION_OAUTH_PARAMETER_ALREADY_SET" , exception.Message );
        }

        private OAuthAuthorizationHeaderBuilder CreateSut ( )
        {
            return new (
                _nonceProvider.Object ,
                _oauthSigner.Object ,
                _signatureBaseGenerator.Object ,
                _timeStampProvider.Object );
        }
    }
}