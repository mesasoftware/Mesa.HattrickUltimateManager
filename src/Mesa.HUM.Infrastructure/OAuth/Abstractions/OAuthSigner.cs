namespace Mesa.HUM.Infrastructure.OAuth.Abstractions
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Mesa.HUM.Infrastructure.OAuth.Abstractions.Interfaces;

    internal class OAuthSigner : IOAuthSigner
    {
        public string GenerateSignature (
            string signatureBase ,
            string consumerSecret ,
            string? tokenSecret = null )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace ( signatureBase );
            ArgumentException.ThrowIfNullOrWhiteSpace ( consumerSecret );

            string signingKey = $"{Uri.EscapeDataString ( consumerSecret )}&{Uri.EscapeDataString ( string.IsNullOrWhiteSpace ( tokenSecret ) ? string.Empty : tokenSecret )}";

            byte [ ] signingKeyBytes = Encoding.UTF8.GetBytes ( signingKey );
            byte [ ] signatureBytes;

            using ( var hashAlgorithm = new HMACSHA1 ( signingKeyBytes ) )
            {
                byte [ ] signatureBaseBytes = Encoding.UTF8.GetBytes ( signatureBase );

                signatureBytes = hashAlgorithm.ComputeHash ( signatureBaseBytes );
            }

            return Convert.ToBase64String ( signatureBytes );
        }
    }
}