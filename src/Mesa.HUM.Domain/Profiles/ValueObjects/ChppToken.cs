namespace Mesa.HUM.Domain.Profiles.ValueObjects
{
    using System;
    using Mesa.HUM.Domain.Profiles.Enums;

    public sealed record ChppToken
    {
        public ChppToken (
            string token ,
            string tokenSecret ,
            ChppScope scope ,
            DateTimeOffset obtainedAt ,
            DateTimeOffset expiresAt )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace ( token );

            if ( token.Length != 16 )
            {
                throw new ArgumentException ( "TOKEN_SHOULD_BE_SIXTEEN_CHARACTERS_LONG" );
            }

            ArgumentException.ThrowIfNullOrWhiteSpace ( tokenSecret );

            if ( tokenSecret.Length != 16 )
            {
                throw new ArgumentException ( "TOKEN_SECRET_SHOULD_BE_SIXTEEN_CHARACTERS_LONG" );
            }

            if ( obtainedAt >= expiresAt )
            {
                throw new ArgumentException ( "TOKEN_CANNOT_BE_EXPIRED_ON_CREATION" );
            }

            Token = token;
            TokenSecret = tokenSecret;
            Scope = scope;
            ObtainedAt = obtainedAt;
            ExpiresAt = expiresAt;
        }

        public string Token { get; }

        public string TokenSecret { get; }

        public ChppScope Scope { get; }

        public DateTimeOffset ObtainedAt { get; }

        public DateTimeOffset ExpiresAt { get; }

        public bool HasScope ( ChppScope scope )
        {
            return Scope.HasFlag ( scope );
        }

        public bool IsExpired ( DateTimeOffset now )
        {
            return now >= ExpiresAt;
        }
    }
}