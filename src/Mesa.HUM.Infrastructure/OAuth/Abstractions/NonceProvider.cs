namespace Mesa.HUM.Infrastructure.OAuth.Abstractions
{
    using System;
    using Mesa.HUM.Infrastructure.OAuth.Abstractions.Interfaces;

    internal class NonceProvider : INonceProvider
    {
        public string GenerateNonce ( )
        {
            return Convert.ToBase64String ( Guid.NewGuid ( ).ToByteArray ( ) )
               .Replace ( "+" , string.Empty )
               .Replace ( "/" , string.Empty )
               .Replace ( "=" , string.Empty );
        }
    }
}