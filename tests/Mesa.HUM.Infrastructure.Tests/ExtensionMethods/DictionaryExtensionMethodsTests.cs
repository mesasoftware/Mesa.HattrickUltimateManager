namespace Mesa.HUM.Infrastructure.Tests.ExtensionMethods
{
    using System.Collections.Generic;
    using Mesa.HUM.Infrastructure.ExtensionMethods.Dictionary;

    public class DictionaryExtensionMethodsTests
    {
        [Fact]
        public void ExtractParametersString_WhenDictionaryIsEmpty_ShouldReturnEmptyString ( )
        {
            // Arrange.
            var dictionary = new Dictionary<string , string> ( );

            // Act.
            string actual = dictionary.ExtractParametersString ( );

            // Assert.
            Assert.Empty ( actual );
        }

        [Fact]
        public void ExtractParametersString_WhenMultiplePairs_ShouldOrderByKeyAndJoinWithAmpersand ( )
        {
            var dictionary = new Dictionary<string , string>
            {
                [ "param2" ] = "value2" ,
                [ "param1" ] = "value1"
            };

            string expected = "param1=value1&param2=value2";

            // Act.
            string actual = dictionary.ExtractParametersString ( );

            // Assert.
            Assert.Equal ( expected , actual );
        }

        [Fact]
        public void ExtractParametersString_WhenSinglePair_ShouldReturnKeyValuePair ( )
        {
            var dictionary = new Dictionary<string , string>
            {
                [ "param1" ] = "value1"
            };

            string expected = "param1=value1";

            // Act.
            string actual = dictionary.ExtractParametersString ( );

            // Assert.
            Assert.Equal ( expected , actual );
        }
    }
}