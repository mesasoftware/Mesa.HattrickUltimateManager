namespace Mesa.HUM.Infrastructure.OAuth.Abstractions.Interfaces
{
    internal interface INonceProvider
    {
        string GenerateNonce ( );
    }
}