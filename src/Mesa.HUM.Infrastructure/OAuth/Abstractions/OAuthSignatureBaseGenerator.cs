namespace Mesa.HUM.Infrastructure.OAuth.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Mesa.HUM.Infrastructure.ExtensionMethods.Dictionary;
    using Mesa.HUM.Infrastructure.ExtensionMethods.Uri;
    using Mesa.HUM.Infrastructure.OAuth.Abstractions.Interfaces;

    internal class OAuthSignatureBaseGenerator : IOAuthSignatureBaseGenerator
    {
        public string GenerateSignatureBase ( HttpMethod httpMethod , Uri uri , Dictionary<string , string> parameters )
        {
            string baseUrl = uri.GetBaseUrl ( );
            string parametersString = parameters.ExtractParametersString ( );

            return $"{httpMethod.ToString ( ).ToUpperInvariant ( )}&{Uri.EscapeDataString ( baseUrl )}&{Uri.EscapeDataString ( parametersString )}";
        }
    }
}