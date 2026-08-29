namespace Mesa.HUM.Infrastructure.Tests.OAuth.Abstractions
{
    using System.Linq;
    using Mesa.HUM.Infrastructure.OAuth.Abstractions;

    public class NonceProviderTests
    {
        [Fact]
        public void GenerateNonce_ContainsNoBase64SpecialChars ( )
        {
            // Assert.
            var sut = new NonceProvider ( );
            char [ ] invalidValues = [ '+' , '/' , '=' ];

            // Act.
            var actual = Enumerable.Range ( 0 , 50 )
                .Select ( _ => sut.GenerateNonce ( ) )
                .Where ( x => x.IndexOfAny ( invalidValues ) > -1 )
                .ToList ( );

            // Assert.
            Assert.Empty ( actual );
        }

        [Fact]
        public void GenerateNonce_ReturnsNonEmptyString ( )
        {
            // Assert.
            var sut = new NonceProvider ( );

            // Act.
            string actual = sut.GenerateNonce ( );

            // Assert.
            Assert.NotNull ( actual );
            Assert.NotEmpty ( actual );
        }

        [Fact]
        public void GenerateNonce_ReturnsUniqueValues ( )
        {
            // Arrange.
            var sut = new NonceProvider ( );

            // Act.
            var actual = Enumerable.Range ( 0 , 50 )
                .Select ( _ => sut.GenerateNonce ( ) )
                .ToList ( );

            // Assert.
            Assert.Distinct ( actual );
        }
    }
}