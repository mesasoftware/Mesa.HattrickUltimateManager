namespace Mesa.HUM.Infrastructure.Tests.OAuth.Contracts
{
    using System;
    using Mesa.HUM.Infrastructure.OAuth.Contracts;

    public class EndpointsTests
    {
        private const string accessTokenUrlValue = "accessTokenUrl";

        private const string callBackUrlValue = "callBackUrl";

        private const string checkTokenUrlValue = "checkTokenUrl";

        private const string emptyValue = "";

        private const string protectedResourceUrlValue = "protectedResourceUrl";

        private const string requestTokenUrlValue = "requestTokenUrl";

        private const string revokeTokenUrlValue = "revokeTokenUrl";

        private const string userAuthorizeUrlValue = "userAuthorizeUrl";

        [Theory]
        [InlineData ( emptyValue , callBackUrlValue , checkTokenUrlValue , protectedResourceUrlValue , requestTokenUrlValue , revokeTokenUrlValue , userAuthorizeUrlValue )]
        [InlineData ( accessTokenUrlValue , emptyValue , checkTokenUrlValue , protectedResourceUrlValue , requestTokenUrlValue , revokeTokenUrlValue , userAuthorizeUrlValue )]
        [InlineData ( accessTokenUrlValue , callBackUrlValue , emptyValue , protectedResourceUrlValue , requestTokenUrlValue , revokeTokenUrlValue , userAuthorizeUrlValue )]
        [InlineData ( accessTokenUrlValue , callBackUrlValue , checkTokenUrlValue , emptyValue , requestTokenUrlValue , revokeTokenUrlValue , userAuthorizeUrlValue )]
        [InlineData ( accessTokenUrlValue , callBackUrlValue , checkTokenUrlValue , protectedResourceUrlValue , emptyValue , revokeTokenUrlValue , userAuthorizeUrlValue )]
        [InlineData ( accessTokenUrlValue , callBackUrlValue , checkTokenUrlValue , protectedResourceUrlValue , requestTokenUrlValue , emptyValue , userAuthorizeUrlValue )]
        [InlineData ( accessTokenUrlValue , callBackUrlValue , checkTokenUrlValue , protectedResourceUrlValue , requestTokenUrlValue , revokeTokenUrlValue , emptyValue )]
        public void Constructor_WhenParameterIsEmpty_ShouldThrowArgumentException (
        string accessTokenUrl ,
        string callBackUrl ,
        string checkTokenUrl ,
        string protectedResourceUrl ,
        string requestTokenUrl ,
        string revokeTokenUrl ,
        string userAuthorizeUrl )
        {
            // Assert.
            Assert.Throws<ArgumentException> ( ( ) => new Endpoints (
                accessTokenUrl ,
                callBackUrl ,
                checkTokenUrl ,
                protectedResourceUrl ,
                requestTokenUrl ,
                revokeTokenUrl ,
                userAuthorizeUrl ) );
        }

        [Theory]
        [InlineData ( null , callBackUrlValue , checkTokenUrlValue , protectedResourceUrlValue , requestTokenUrlValue , revokeTokenUrlValue , userAuthorizeUrlValue )]
        [InlineData ( accessTokenUrlValue , null , checkTokenUrlValue , protectedResourceUrlValue , requestTokenUrlValue , revokeTokenUrlValue , userAuthorizeUrlValue )]
        [InlineData ( accessTokenUrlValue , callBackUrlValue , null , protectedResourceUrlValue , requestTokenUrlValue , revokeTokenUrlValue , userAuthorizeUrlValue )]
        [InlineData ( accessTokenUrlValue , callBackUrlValue , checkTokenUrlValue , null , requestTokenUrlValue , revokeTokenUrlValue , userAuthorizeUrlValue )]
        [InlineData ( accessTokenUrlValue , callBackUrlValue , checkTokenUrlValue , protectedResourceUrlValue , null , revokeTokenUrlValue , userAuthorizeUrlValue )]
        [InlineData ( accessTokenUrlValue , callBackUrlValue , checkTokenUrlValue , protectedResourceUrlValue , requestTokenUrlValue , null , userAuthorizeUrlValue )]
        [InlineData ( accessTokenUrlValue , callBackUrlValue , checkTokenUrlValue , protectedResourceUrlValue , requestTokenUrlValue , revokeTokenUrlValue , null )]
        public void Constructor_WhenParameterIsNull_ShouldThrowArgumentNullException (
        string? accessTokenUrl ,
        string? callBackUrl ,
        string? checkTokenUrl ,
        string? protectedResourceUrl ,
        string? requestTokenUrl ,
        string? revokeTokenUrl ,
        string? userAuthorizeUrl )
        {
            // Assert.
            Assert.Throws<ArgumentNullException> ( ( ) => new Endpoints (
                accessTokenUrl! ,
                callBackUrl! ,
                checkTokenUrl! ,
                protectedResourceUrl! ,
                requestTokenUrl! ,
                revokeTokenUrl! ,
                userAuthorizeUrl! ) );
        }

        [Fact]
        public void Constructor_WhenParametersAreValid_ShouldPopulateProperties ( )
        {
            // Act.
            var sut = new Endpoints (
                accessTokenUrlValue ,
                callBackUrlValue ,
                checkTokenUrlValue ,
                protectedResourceUrlValue ,
                requestTokenUrlValue ,
                revokeTokenUrlValue ,
                userAuthorizeUrlValue );

            // Assert.
            Assert.Equal ( accessTokenUrlValue , sut.AccessTokenUrl );
            Assert.Equal ( callBackUrlValue , sut.CallBackUrl );
            Assert.Equal ( checkTokenUrlValue , sut.CheckTokenUrl );
            Assert.Equal ( protectedResourceUrlValue , sut.ProtectedResourceUrl );
            Assert.Equal ( requestTokenUrlValue , sut.RequestTokenUrl );
            Assert.Equal ( revokeTokenUrlValue , sut.RevokeTokenUrl );
            Assert.Equal ( userAuthorizeUrlValue , sut.UserAuthorizeUrl );
        }
    }
}