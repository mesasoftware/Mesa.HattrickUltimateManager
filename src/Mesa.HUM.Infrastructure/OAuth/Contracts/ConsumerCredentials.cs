namespace Mesa.HUM.Infrastructure.OAuth.Contracts
{
    using System;

    public sealed class ConsumerCredentials
    {
        public ConsumerCredentials ( string consumerKey , string consumerSecret , string userAgent )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace ( consumerKey );
            ArgumentException.ThrowIfNullOrWhiteSpace ( consumerSecret );
            ArgumentException.ThrowIfNullOrWhiteSpace ( userAgent );

            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            UserAgent = userAgent;
        }

        public string ConsumerKey { get; }

        public string ConsumerSecret { get; }

        public string UserAgent { get; }
    }
}