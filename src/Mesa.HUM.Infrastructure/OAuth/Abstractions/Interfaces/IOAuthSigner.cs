namespace Mesa.HUM.Infrastructure.OAuth.Abstractions.Interfaces
{
    internal interface IOAuthSigner
    {
        string GenerateSignature (
            string signatureBase ,
            string consumerSecret ,
            string? tokenSecret = null );
    }
}