namespace Mesa.HUM.Domain.Profiles
{
    using System;
    using Mesa.HUM.Domain.Abstractions;
    using Mesa.HUM.Domain.Profiles.ValueObjects;

    public class UserProfile : AggregateRoot
    {
        private UserProfile (
            ProfileId id )
        {
            Id = id;
            ChppToken = null;
            SynchronizedAt = null;
        }

        public ChppToken? ChppToken { get; private set; }

        public ProfileId Id { get; }

        public DateTimeOffset? SynchronizedAt { get; private set; }

        public static UserProfile Create ( )
        {
            return new UserProfile (
                ProfileId.New ( ) );
        }

        public void MarkAsSynchronized ( DateTimeOffset synchronizedAt )
        {
            SynchronizedAt = synchronizedAt;
        }

        public void ReplaceToken ( ChppToken chppToken )
        {
            ArgumentNullException.ThrowIfNull ( chppToken );

            if ( chppToken.IsExpired ( DateTimeOffset.UtcNow ) )
            {
                throw new ArgumentException ( "NEW_TOKEN_CANNOT_BE_EXPIRED" );
            }

            ChppToken = chppToken;
        }

        public void RevokeToken ( )
        {
            if ( ChppToken == null )
            {
                throw new InvalidOperationException ( "CANNOT_REVOKE_NULL_TOKEN" );
            }

            ChppToken = null;
        }
    }
}