namespace Mesa.HUM.Infrastructure.ExtensionMethods.Dictionary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class DictionaryExtensionMethods
    {
        internal static string ExtractParametersString ( this Dictionary<string , string> dictionary )
        {
            return string.Join (
                "&" ,
                dictionary
                .Select ( p => (Key: Uri.EscapeDataString ( p.Key ) , Value: Uri.EscapeDataString ( p.Value )) )
                .OrderBy ( p => p.Key , StringComparer.Ordinal )
                .ThenBy ( p => p.Value , StringComparer.Ordinal )
                .Select ( p => $"{p.Key}={p.Value}" ) );
        }
    }
}