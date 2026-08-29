namespace Mesa.HUM.Infrastructure.OAuth.Abstractions.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    internal interface IOAuthSignatureBaseGenerator
    {
        string GenerateSignatureBase ( HttpMethod httpMethod , Uri uri , Dictionary<string , string> parameters );
    }
}