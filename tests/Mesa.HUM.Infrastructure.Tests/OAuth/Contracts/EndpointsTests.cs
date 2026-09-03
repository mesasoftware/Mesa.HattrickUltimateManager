namespace Mesa.HUM.Infrastructure.Tests.OAuth.Contracts
{
    using System;
    using Mesa.HUM.Infrastructure.OAuth.Contracts;

    public class EndpointsTests
    {
        [Theory]
        [InlineData ( "" , "callBackUrl" , "checkTokenUrl" , "protectedResourceUrl" , "requestTokenUrl" , "revokeTokenUrl" , "userAuthorizeUrl" )]
        [InlineData ( "   " , "callBackUrl" , "checkTokenUrl" , "protectedResourceUrl" , "requestTokenUrl" , "revokeTokenUrl" , "userAuthorizeUrl" )]
        [InlineData ( "accessToken" , "" , "checkTokenUrl" , "protectedResourceUrl" , "requestTokenUrl" , "revokeTokenUrl" , "userAuthorizeUrl" )]
        [InlineData ( "accessToken" , "   " , "checkTokenUrl" , "protectedResourceUrl" , "requestTokenUrl" , "revokeTokenUrl" , "userAuthorizeUrl" )]
        [InlineData ( "accessToken" , "callBackUrl" , "" , "protectedResourceUrl" , "requestTokenUrl" , "revokeTokenUrl" , "userAuthorizeUrl" )]
        [InlineData ( "accessToken" , "callBackUrl" , "   " , "protectedResourceUrl" , "requestTokenUrl" , "revokeTokenUrl" , "userAuthorizeUrl" )]
        [InlineData ( "accessToken" , "callBackUrl" , "checkTokenUrl" , "" , "requestTokenUrl" , "revokeTokenUrl" , "userAuthorizeUrl" )]
        [InlineData ( "accessToken" , "callBackUrl" , "checkTokenUrl" , "   " , "requestTokenUrl" , "revokeTokenUrl" , "userAuthorizeUrl" )]
        [InlineData ( "accessToken" , "callBackUrl" , "checkTokenUrl" , "protectedResourceUrl" , "" , "revokeTokenUrl" , "userAuthorizeUrl" )]
        [InlineData ( "accessToken" , "callBackUrl" , "checkTokenUrl" , "protectedResourceUrl" , "   " , "revokeTokenUrl" , "userAuthorizeUrl" )]
        [InlineData ( "accessToken" , "callBackUrl" , "checkTokenUrl" , "protectedResourceUrl" , "requestTokenUrl" , "" , "userAuthorizeUrl" )]
        [InlineData ( "accessToken" , "callBackUrl" , "checkTokenUrl" , "protectedResourceUrl" , "requestTokenUrl" , "   " , "userAuthorizeUrl" )]
        [InlineData ( "accessToken" , "callBackUrl" , "checkTokenUrl" , "protectedResourceUrl" , "requestTokenUrl" , "revokeTokenUrl" , "" )]
        [InlineData ( "accessToken" , "callBackUrl" , "checkTokenUrl" , "protectedResourceUrl" , "requestTokenUrl" , "revokeTokenUrl" , "   " )]
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
                userAuthorizeUrl! ) );
        }

        [Theory]
        [InlineData ( null , "callBackUrl" , "checkTokenUrl" , "protectedResourceUrl" , "requestTokenUrl" , "revokeTokenUrl" , "userAuthorizeUrl" )]
        [InlineData ( "accessTokenUrl" , null , "checkTokenUrl" , "protectedResourceUrl" , "requestTokenUrl" , "revokeTokenUrl" , "userAuthorizeUrl" )]
        [InlineData ( "accessTokenUrl" , "callBackUrl" , null , "protectedResourceUrl" , "requestTokenUrl" , "revokeTokenUrl" , "userAuthorizeUrl" )]
        [InlineData ( "accessTokenUrl" , "callBackUrl" , "checkTokenUrl" , null , "requestTokenUrl" , "revokeTokenUrl" , "userAuthorizeUrl" )]
        [InlineData ( "accessTokenUrl" , "callBackUrl" , "checkTokenUrl" , "protectedResourceUrl" , null , "revokeTokenUrl" , "userAuthorizeUrl" )]
        [InlineData ( "accessTokenUrl" , "callBackUrl" , "checkTokenUrl" , "protectedResourceUrl" , "requestTokenUrl" , null , "userAuthorizeUrl" )]
        [InlineData ( "accessTokenUrl" , "callBackUrl" , "checkTokenUrl" , "protectedResourceUrl" , "requestTokenUrl" , "revokeTokenUrl" , null )]
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
                "accessTokenUrl" ,
                "callBackUrl" ,
                "checkTokenUrl" ,
                "protectedResourceUrl" ,
                "requestTokenUrl" ,
                "revokeTokenUrl" ,
                "userAuthorizeUrl" );

            // Assert.
            Assert.Equal ( "accessTokenUrl" , sut.AccessTokenUrl );
            Assert.Equal ( "callBackUrl" , sut.CallBackUrl );
            Assert.Equal ( "checkTokenUrl" , sut.CheckTokenUrl );
            Assert.Equal ( "protectedResourceUrl" , sut.ProtectedResourceUrl );
            Assert.Equal ( "requestTokenUrl" , sut.RequestTokenUrl );
            Assert.Equal ( "revokeTokenUrl" , sut.RevokeTokenUrl );
            Assert.Equal ( "userAuthorizeUrl" , sut.UserAuthorizeUrl );
        }
    }
}