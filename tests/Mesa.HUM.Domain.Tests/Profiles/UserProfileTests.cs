namespace Mesa.HUM.Domain.Tests.Profiles
{
    using System;
    using Mesa.HUM.Domain.Profiles;
    using Mesa.HUM.Domain.Profiles.Enums;
    using Mesa.HUM.Domain.Profiles.ValueObjects;

    public class UserProfileTests
    {
        private static ChppToken CreateToken ( DateTimeOffset obtainedAt , DateTimeOffset expiredAt )
        {
            return new ChppToken (
                "ABCDEFGHIJKLMNOP" ,
                "ZYXWVUTSRQPONMLK" ,
                ChppScope.ReadAccess ,
                obtainedAt ,
                expiredAt );
        }

        public class CreateTests ( )
        {
            [Fact]
            public void Create_ShouldBeNotGuidEmptyAndOtherPropertiesBeNull ( )
            {
                // Arrange & Act.
                var sut = UserProfile.Create ( );

                // Assert.
                Assert.NotEqual ( Guid.Empty , sut.Id.Value );
                Assert.Null ( sut.ChppToken );
                Assert.Null ( sut.SynchronizedAt );
            }
        }

        public class MarkAsSynchronizedTests
        {
            [Fact]
            public void MarkAsSynchronized_WhenSynchronizedAtIsNull_ShouldSetItToGivenValue ( )
            {
                // Arrange.
                var sut = UserProfile.Create ( );
                var expected = DateTimeOffset.UtcNow;

                // Act.
                sut.MarkAsSynchronized ( expected );

                // Assert.
                Assert.Equal ( expected , sut.SynchronizedAt );
            }

            [Fact]
            public void MarkAsSynchronized_WhenSynchronizedAtIsNull_ShouldUpdateItToGivenValue ( )
            {
                // Arrange.
                var sut = UserProfile.Create ( );
                var expected = DateTimeOffset.UtcNow;

                sut.MarkAsSynchronized ( DateTimeOffset.Parse ( "2026-01-01 00:00:00.000" ) );

                // Act.
                sut.MarkAsSynchronized ( expected );

                // Assert.
                Assert.Equal ( expected , sut.SynchronizedAt );
            }
        }

        public class ReplaceTokenTests
        {
            [Fact]
            public void ReplaceToken_GivenExpiredToken_ShouldThrowArgumentExceptionWithCorrectMessage ( )
            {
                // Arrange.
                var sut = UserProfile.Create ( );
                string expected = "NEW_TOKEN_CANNOT_BE_EXPIRED";

                var expiredChppToken = CreateToken (
                    DateTimeOffset.Parse ( "2026-01-01 00:00:00.000" ) ,
                    DateTimeOffset.Parse ( "2026-01-02 00:00:00.000" ) );

                // Act.
                var exception = Record.Exception ( ( ) => sut.ReplaceToken ( expiredChppToken ) );

                // Assert.
                Assert.NotNull ( exception );
                var actual = Assert.IsType<ArgumentException> ( exception );
                Assert.Equal ( expected , actual.Message );
            }

            [Fact]
            public void ReplaceToken_GivenNullChppToken_ShouldThrowArgumentNullExceptionWithCorrectMessage ( )
            {
                // Arrange.
                var sut = UserProfile.Create ( );
                string expected = "Value cannot be null. (Parameter 'chppToken')";

                // Act.
                var exception = Record.Exception ( ( ) => sut.ReplaceToken ( null! ) );

                // Assert.
                Assert.NotNull ( exception );
                var actual = Assert.IsType<ArgumentNullException> ( exception );
                Assert.Equal ( expected , actual.Message );
            }
        }

        public class RevokeTokenTests
        {
            [Fact]
            public void RevokeToken_WhenTokenIsNotNull_ShouldSetTokenToNull ( )
            {
                // Arrange.
                var sut = UserProfile.Create ( );

                var chppToken = CreateToken ( DateTimeOffset.UtcNow , DateTimeOffset.MaxValue );

                sut.ReplaceToken ( chppToken );

                // Act.
                var exception = Record.Exception ( sut.RevokeToken );

                // Assert.
                Assert.Null ( sut.ChppToken );
            }

            [Fact]
            public void RevokeToken_WhenTokenIsNull_ShouldThrowInvalidOperationExceptionWithCorrectMessage ( )
            {
                // Arrange.
                var sut = UserProfile.Create ( );
                string expected = "CANNOT_REVOKE_NULL_TOKEN";

                // Act.
                var exception = Record.Exception ( sut.RevokeToken );

                // Assert.
                Assert.NotNull ( exception );
                var actual = Assert.IsType<InvalidOperationException> ( exception );
                Assert.Equal ( expected , actual.Message );
            }
        }
    }
}