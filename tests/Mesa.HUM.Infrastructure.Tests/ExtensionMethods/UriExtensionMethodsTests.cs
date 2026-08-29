namespace Mesa.HUM.Infrastructure.Tests.ExtensionMethods
{
    using System;
    using Mesa.HUM.Infrastructure.ExtensionMethods.Uri;

    public class UriExtensionMethodsTests
    {
        public class GetBaseUrlTests
        {
            [Theory]
            [InlineData ( "http://host.com" , "http://host.com/" )]
            [InlineData ( "http://host.com/" , "http://host.com/" )]
            [InlineData ( "http://host.com?param1=value1" , "http://host.com/" )]
            [InlineData ( "http://host.com/?param1=value1" , "http://host.com/" )]
            [InlineData ( "https://host.com" , "https://host.com/" )]
            [InlineData ( "https://host.com/" , "https://host.com/" )]
            [InlineData ( "https://host.com?param1=value1" , "https://host.com/" )]
            [InlineData ( "https://host.com/?param1=value1" , "https://host.com/" )]
            public void GetBaseUrl_WhenIsDefaultPort_ShouldReturnStringWithoutPort ( string url , string expected )
            {
                // Arrange.
                var uri = new Uri ( url );

                // Act.
                string actual = uri.GetBaseUrl ( );

                // Assert.
                Assert.Equal ( expected , actual );
            }

            [Theory]
            [InlineData ( "http://host.com:20" , "http://host.com:20/" )]
            [InlineData ( "http://host.com:21/" , "http://host.com:21/" )]
            [InlineData ( "http://host.com:22?param1=value1" , "http://host.com:22/" )]
            [InlineData ( "http://host.com:23/?param1=value1" , "http://host.com:23/" )]
            [InlineData ( "https://host.com:24" , "https://host.com:24/" )]
            [InlineData ( "https://host.com:25/" , "https://host.com:25/" )]
            [InlineData ( "https://host.com:26?param1=value1" , "https://host.com:26/" )]
            [InlineData ( "https://host.com:27/?param1=value1" , "https://host.com:27/" )]
            public void GetBaseUrl_WhenIsNotDefaultPort_ShouldReturnStringWithPort ( string url , string expected )
            {
                // Arrange.
                var uri = new Uri ( url );

                // Act.
                string actual = uri.GetBaseUrl ( );

                // Assert.
                Assert.Equal ( expected , actual );
            }
        }

        public class GetQueryParametersTests
        {
            [Fact]
            public void GetQueryParameters_WhenMultipleParameterQuery_ShouldReturnDictionaryWithAllElements ( )
            {
                // Arrange.
                var uri = new Uri ( "https://server.host.com?param1=value1&param2=value2&param3=" );

                // Act.
                var actual = uri.GetQueryParameters ( );

                // Assert.
                Assert.Equal ( "value1" , actual [ "param1" ] );
                Assert.Equal ( "value2" , actual [ "param2" ] );
                Assert.Empty ( actual [ "param3" ] );
            }

            [Fact]
            public void GetQueryParameters_WhenParameterIsEscaped_ShouldReturnUnescapedValue ( )
            {
                // Arrange.
                var uri = new Uri ( "https://server.host.com?param%201=value%201" );

                // Act.
                var actual = uri.GetQueryParameters ( );

                // Assert.
                Assert.Equal ( "value 1" , actual [ "param 1" ] );
            }

            [Fact]
            public void GetQueryParameters_WhenQueryIsEmpty_ShouldReturnEmptyDictionary ( )
            {
                // Arrange.
                var uri = new Uri ( "https://server.host.com" );

                // Act.
                var actual = uri.GetQueryParameters ( );

                // Assert.
                Assert.Empty ( actual );
            }

            [Fact]
            public void GetQueryParameters_WhenQueryParameterIsMalformed_ShouldParseParameterIntoPair ( )
            {
                // Arrange.
                var uri = new Uri ( "https://server.host.com?param1=val=ue1" );

                // Act.
                var actual = uri.GetQueryParameters ( );

                // Assert.
                Assert.Equal ( "val=ue1" , actual [ "param1" ] );
            }

            [Fact]
            public void GetQueryParameters_WhenSingleParameterQuery_ShouldReturnDictionaryWithOneElement ( )
            {
                // Arrange.
                var uri = new Uri ( "https://server.host.com?param1=value1" );

                // Act.
                var actual = uri.GetQueryParameters ( );

                // Assert.
                Assert.Equal ( "value1" , actual [ "param1" ] );
            }
        }
    }
}