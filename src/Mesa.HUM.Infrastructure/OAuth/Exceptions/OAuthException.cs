namespace Mesa.HUM.Infrastructure.OAuth.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public sealed class OAuthException : Exception
    {
        public OAuthException ( string message ) : base ( message )
        {
        }

        public OAuthException ( string message , Exception inner ) : base ( message , inner )
        {
        }
    }
}