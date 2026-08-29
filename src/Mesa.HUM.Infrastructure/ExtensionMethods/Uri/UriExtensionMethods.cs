namespace Mesa.HUM.Infrastructure.ExtensionMethods.Uri
{
    using System;
    using System.Collections.Generic;

    internal static class UriExtensionMethods
    {
        internal static string GetBaseUrl ( this Uri uri )
        {
            return $"{uri.Scheme}://{uri.Host}{( uri.IsDefaultPort ? string.Empty : $":{uri.Port}" )}{uri.AbsolutePath}";
        }

        internal static Dictionary<string , string> GetQueryParameters ( this Uri uri )
        {
            var parameters = new Dictionary<string , string> ( );

            if ( !string.IsNullOrWhiteSpace ( uri.Query ) )
            {
                string [ ] entries = uri.Query
                    .TrimStart ( '?' )
                    .Split ( '&' , StringSplitOptions.RemoveEmptyEntries );

                foreach ( string pair in entries )
                {
                    string [ ] parts = pair.Split ( '=' , 2 );

                    if ( parts.Length == 2 )
                    {
                        parameters [ Uri.UnescapeDataString ( parts [ 0 ] ) ] = Uri.UnescapeDataString ( parts [ 1 ] );
                    }
                }
            }

            return parameters;
        }
    }
}