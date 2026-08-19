namespace Mesa.HUM.Domain.Tests.Profiles.ValueObjects
{
    using System;
    using Mesa.HUM.Domain.Profiles.Enums;
    using Mesa.HUM.Domain.Profiles.ValueObjects;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

    public class ChppTokenTests
    {
        private const string tokenSecretTooLong = "ZYXWVUTSRQPONMLKJ";

        private const string tokenSecretTooShort = "ZYXWVUTSRQPONML";

        private const string tokenTooLong = "ABCDEFGHIJKLMNOPQ";

        private const string tokenTooShort = "ABCDEFGHIJKLMNO";

        private const string validToken = "ABCDEFGHIJKLMNOP";

        private const string validTokenSecret = "ZYXWVUTSRQPONMLK";

        public class ConstructorTests
        {
            [Theory]
            [InlineData ( tokenTooShort )]
            [InlineData ( tokenTooLong )]
            public void Constructor_GivenInvalidToken_ShouldThrowArgumentExceptionWithCorrectMessage ( string token )
            {
                // Arrange & Act.
                string expected = "TOKEN_SHOULD_BE_SIXTEEN_CHARACTERS_LONG";

                var exception = Record.Exception ( ( )
                    => new ChppToken (
                        token ,
                        validTokenSecret ,
                        ChppScope.ReadAccess | ChppScope.ManageChallenges ,
                        DateTimeOffset.UtcNow ,
                        DateTimeOffset.MaxValue ) );

                // Assert.
                Assert.NotNull ( exception );
                var actual = Assert.IsType<ArgumentException> ( exception );
                Assert.Equal ( expected , actual.Message );
            }

            [Theory]
            [InlineData ( tokenSecretTooShort )]
            [InlineData ( tokenSecretTooLong )]
            public void Constructor_GivenInvalidTokenSecret_ShouldThrowArgumentExceptionWithCorrectMessage ( string tokenSecret )
            {
                // Arrange & Act.
                string expected = "TOKEN_SECRET_SHOULD_BE_SIXTEEN_CHARACTERS_LONG";

                var exception = Record.Exception ( ( )
                    => new ChppToken (
                        validToken ,
                        tokenSecret ,
                        ChppScope.ReadAccess | ChppScope.ManageChallenges ,
                        DateTimeOffset.UtcNow ,
                        DateTimeOffset.MaxValue ) );

                // Assert.
                Assert.NotNull ( exception );
                var actual = Assert.IsType<ArgumentException> ( exception );
                Assert.Equal ( expected , actual.Message );
            }

            [Fact]
            public void Constructor_GivenValidValues_ShouldBeEqual ( )
            {
                // Arrange.
                var scope = ChppScope.ReadAccess | ChppScope.ManageChallenges;
                var obtainedAt = DateTimeOffset.UtcNow;
                var expiresAt = DateTimeOffset.MaxValue;

                // Act.
                var sut = new ChppToken (
                    validToken ,
                    validTokenSecret ,
                    ChppScope.ReadAccess | ChppScope.ManageChallenges ,
                    obtainedAt ,
                    expiresAt );

                // Assert.
                Assert.Equal ( validToken , sut.Token );
                Assert.Equal ( validTokenSecret , sut.TokenSecret );
                Assert.Equal ( scope , sut.Scope );
                Assert.Equal ( obtainedAt , sut.ObtainedAt );
                Assert.Equal ( expiresAt , sut.ExpiresAt );
            }

            [Fact]
            public void Constructor_GivenValidValues_ShouldNotThrowException ( )
            {
                // Arrange & Act.
                var exception = Record.Exception ( ( )
                    => new ChppToken (
                        validToken ,
                        validTokenSecret ,
                        ChppScope.ReadAccess | ChppScope.ManageChallenges ,
                        DateTimeOffset.UtcNow ,
                        DateTimeOffset.MaxValue ) );

                // Assert.
                Assert.Null ( exception );
            }

            [Fact]
            public void Constructor_WhenObtainedAtAndExpiresAtAreEqual_ShouldThrowArgumentExceptionWithCorrectMessage ( )
            {
                // Arrange & Act.
                var date = DateTimeOffset.UtcNow;

                string expected = "TOKEN_CANNOT_BE_EXPIRED_ON_CREATION";

                var exception = Record.Exception ( ( )
                    => new ChppToken (
                        validToken ,
                        validTokenSecret ,
                        ChppScope.ReadAccess | ChppScope.ManageChallenges ,
                        date ,
                        date ) );

                // Assert.
                Assert.NotNull ( exception );
                var actual = Assert.IsType<ArgumentException> ( exception );
                Assert.Equal ( expected , actual.Message );
            }

            [Fact]
            public void Constructor_WhenObtainedAtIsLesserThanExpiresAt_ShouldThrowArgumentExceptionWithCorrectMessage ( )
            {
                // Arrange & Act.
                var date = DateTimeOffset.UtcNow;

                string expected = "TOKEN_CANNOT_BE_EXPIRED_ON_CREATION";

                var exception = Record.Exception ( ( )
                    => new ChppToken (
                        validToken ,
                        validTokenSecret ,
                        ChppScope.ReadAccess | ChppScope.ManageChallenges ,
                        date ,
                        date.AddMicroseconds ( -1 ) ) );

                // Assert.
                Assert.NotNull ( exception );
                var actual = Assert.IsType<ArgumentException> ( exception );
                Assert.Equal ( expected , actual.Message );
            }
        }

        public class HasScopeTests
        {
            [Fact]
            public void HasScope_ReturnsFalse_ForUngrantedScope ( )
            {
                var token = CreateToken ( scope: ChppScope.SetMatchOrder );
                Assert.False ( token.HasScope ( ChppScope.ManageChallenges ) );
            }

            [Fact]
            public void HasScope_ReturnsTrue_ForEachGrantedScope ( )
            {
                var token = CreateToken ( scope: ChppScope.ManageChallenges | ChppScope.SetMatchOrder );
                Assert.True ( token.HasScope ( ChppScope.ManageChallenges ) );
                Assert.True ( token.HasScope ( ChppScope.SetMatchOrder ) );
            }

            [Theory]
            [InlineData ( ChppScope.ManageChallenges )]
            [InlineData ( ChppScope.SetMatchOrder )]
            [InlineData ( ChppScope.ManageYouthPlayers )]
            [InlineData ( ChppScope.SetTraining )]
            [InlineData ( ChppScope.PlaceBid )]
            public void HasScope_ReturnsTrue_ForReadAccess ( ChppScope scope )
            {
                var token = CreateToken ( scope );

                Assert.True ( token.HasScope ( ChppScope.ReadAccess ) );
            }

            private static ChppToken CreateToken ( ChppScope scope )
            {
                return new ChppToken (
                    validToken ,
                    validTokenSecret ,
                    scope ,
                    DateTimeOffset.UtcNow ,
                    DateTimeOffset.MaxValue );
            }
        }

        public class IsExpiredTests
        {
            [Theory]
            [InlineData ( "2025-12-31T23:59:59Z" , false )]  // before
            [InlineData ( "2026-01-01T00:00:00Z" , true )]   // exactly at — the >= boundary
            [InlineData ( "2026-01-01T00:00:01Z" , true )]   // after
            public void IsExpired_RespectsExpiryBoundary ( string nowIso , bool expected )
            {
                // Arrange & Act.
                var token = CreateToken ( expiresAt: DateTimeOffset.Parse ( "2026-01-01T00:00:00Z" ) );

                // Assert.
                Assert.Equal ( expected , token.IsExpired ( DateTimeOffset.Parse ( nowIso ) ) );
            }

            private static ChppToken CreateToken ( DateTimeOffset expiresAt )
            {
                return new ChppToken (
                    validToken ,
                    validTokenSecret ,
                    ChppScope.ReadAccess ,
                    expiresAt.AddDays ( -1 ) ,
                    expiresAt );
            }
        }
    }
}