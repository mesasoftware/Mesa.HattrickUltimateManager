namespace Mesa.HUM.Infrastructure.Tests.OAuth.Abstractions
{
    using System;
    using System.Net.Http;
    using Mesa.HUM.Infrastructure.ExtensionMethods.Uri;
    using Mesa.HUM.Infrastructure.OAuth.Abstractions;

    public class OAuthSignatureBaseGeneratorTests
    {
        [Theory]
        [InlineData ( "Get" , "https://test.host.com/path?param2=value2&param1=value1" , "GET&https%3A%2F%2Ftest.host.com%2Fpath&param1%3Dvalue1%26param2%3Dvalue2" )]
        [InlineData ( "Post" , "https://test.host.com/path?param2=value2&param1=value1" , "POST&https%3A%2F%2Ftest.host.com%2Fpath&param1%3Dvalue1%26param2%3Dvalue2" )]
        [InlineData ( "Delete" , "https://test.host.com/path?param2=value2&param1=value1" , "DELETE&https%3A%2F%2Ftest.host.com%2Fpath&param1%3Dvalue1%26param2%3Dvalue2" )]
        [InlineData ( "Patch" , "https://test.host.com/path?param2=value2&param1=value1" , "PATCH&https%3A%2F%2Ftest.host.com%2Fpath&param1%3Dvalue1%26param2%3Dvalue2" )]
        [InlineData ( "Put" , "https://test.host.com/path?param2=value2&param1=value1" , "PUT&https%3A%2F%2Ftest.host.com%2Fpath&param1%3Dvalue1%26param2%3Dvalue2" )]
        public void GenerateSignatureBase_GivenMultipleParameters_ShouldReturnCorrectSignatureAndParametersSorted ( string httpMethod , string url , string expected )
        {
            // Arrange.
            var sut = new OAuthSignatureBaseGenerator ( );
            var uri = new Uri ( url );

            var parameters = uri.GetQueryParameters ( );

            // Act.
            string actual = sut.GenerateSignatureBase ( HttpMethod.Parse ( httpMethod ) , new Uri ( url ) , parameters );

            // Assert.
            Assert.Equal ( expected , actual );
        }

        [Theory]
        [InlineData ( "get" , "https://test.host.com/path?param1=value1" , "GET&https%3A%2F%2Ftest.host.com%2Fpath&param1%3Dvalue1" )]
        [InlineData ( "post" , "https://test.host.com/path?param1=value1" , "POST&https%3A%2F%2Ftest.host.com%2Fpath&param1%3Dvalue1" )]
        [InlineData ( "delete" , "https://test.host.com/path?param1=value1" , "DELETE&https%3A%2F%2Ftest.host.com%2Fpath&param1%3Dvalue1" )]
        [InlineData ( "patch" , "https://test.host.com/path?param1=value1" , "PATCH&https%3A%2F%2Ftest.host.com%2Fpath&param1%3Dvalue1" )]
        [InlineData ( "put" , "https://test.host.com/path?param1=value1" , "PUT&https%3A%2F%2Ftest.host.com%2Fpath&param1%3Dvalue1" )]
        public void GenerateSignatureBase_GivenSingleParameter_ShouldReturnCorrectSignatureWithCapitalizedMethod ( string httpMethod , string url , string expected )
        {
            // Arrange.
            var sut = new OAuthSignatureBaseGenerator ( );
            var uri = new Uri ( url );

            var parameters = uri.GetQueryParameters ( );

            // Act.
            string actual = sut.GenerateSignatureBase ( HttpMethod.Parse ( httpMethod ) , new Uri ( url ) , parameters );

            // Assert.
            Assert.Equal ( expected , actual );
        }

        [Theory]
        [InlineData ( "Get" , "https://test.host.com/path?param1=" , "GET&https%3A%2F%2Ftest.host.com%2Fpath&param1%3D" )]
        [InlineData ( "Post" , "https://test.host.com/path?param1=" , "POST&https%3A%2F%2Ftest.host.com%2Fpath&param1%3D" )]
        [InlineData ( "Delete" , "https://test.host.com/path?param1=" , "DELETE&https%3A%2F%2Ftest.host.com%2Fpath&param1%3D" )]
        [InlineData ( "Patch" , "https://test.host.com/path?param1=" , "PATCH&https%3A%2F%2Ftest.host.com%2Fpath&param1%3D" )]
        [InlineData ( "Put" , "https://test.host.com/path?param1=" , "PUT&https%3A%2F%2Ftest.host.com%2Fpath&param1%3D" )]
        public void GenerateSignatureBase_GivenSingleParameterWithNoValue_ShouldReturnCorrectSignature ( string httpMethod , string url , string expected )
        {
            // Arrange.
            var sut = new OAuthSignatureBaseGenerator ( );
            var uri = new Uri ( url );

            var parameters = uri.GetQueryParameters ( );

            // Act.
            string actual = sut.GenerateSignatureBase ( HttpMethod.Parse ( httpMethod ) , new Uri ( url ) , parameters );

            // Assert.
            Assert.Equal ( expected , actual );
        }

        [Theory]
        [InlineData ( "Get" , "https://test.host.com/path?param1=value 1" , "GET&https%3A%2F%2Ftest.host.com%2Fpath&param1%3Dvalue%25201" )]
        [InlineData ( "Post" , "https://test.host.com/path?param1=value 1" , "POST&https%3A%2F%2Ftest.host.com%2Fpath&param1%3Dvalue%25201" )]
        [InlineData ( "Delete" , "https://test.host.com/path?param1=value 1" , "DELETE&https%3A%2F%2Ftest.host.com%2Fpath&param1%3Dvalue%25201" )]
        [InlineData ( "Patch" , "https://test.host.com/path?param1=value 1" , "PATCH&https%3A%2F%2Ftest.host.com%2Fpath&param1%3Dvalue%25201" )]
        [InlineData ( "Put" , "https://test.host.com/path?param1=value 1" , "PUT&https%3A%2F%2Ftest.host.com%2Fpath&param1%3Dvalue%25201" )]
        public void GenerateSignatureBase_GivenSingleParameterWithReservedCharacters_ShouldReturnCorrectSignature ( string httpMethod , string url , string expected )
        {
            // Arrange.
            var sut = new OAuthSignatureBaseGenerator ( );
            var uri = new Uri ( url );

            var parameters = uri.GetQueryParameters ( );

            // Act.
            string actual = sut.GenerateSignatureBase ( HttpMethod.Parse ( httpMethod ) , new Uri ( url ) , parameters );

            // Assert.
            Assert.Equal ( expected , actual );
        }

        [Theory]
        [InlineData ( "Get" , "https://test.host.com/path" , "GET&https%3A%2F%2Ftest.host.com%2Fpath&" )]
        [InlineData ( "Post" , "https://test.host.com/path" , "POST&https%3A%2F%2Ftest.host.com%2Fpath&" )]
        [InlineData ( "Delete" , "https://test.host.com/path" , "DELETE&https%3A%2F%2Ftest.host.com%2Fpath&" )]
        [InlineData ( "Patch" , "https://test.host.com/path" , "PATCH&https%3A%2F%2Ftest.host.com%2Fpath&" )]
        [InlineData ( "Put" , "https://test.host.com/path" , "PUT&https%3A%2F%2Ftest.host.com%2Fpath&" )]
        public void GenerateSignatureBase_WhenNoQuery_ShouldReturnCorrectSignature ( string httpMethod , string url , string expected )
        {
            // Arrange.
            var sut = new OAuthSignatureBaseGenerator ( );

            // Act.
            string actual = sut.GenerateSignatureBase ( HttpMethod.Parse ( httpMethod ) , new Uri ( url ) , [ ] );

            // Assert.
            Assert.Equal ( expected , actual );
        }
    }
}