namespace Mesa.HUM.Infrastructure.OAuth.Abstractions
{
    using System;
    using Mesa.HUM.Infrastructure.OAuth.Abstractions.Interfaces;

    internal class TimeStampProvider : ITimeStampProvider
    {
        public string GetTimeStamp ( )
        {
            return DateTimeOffset.UtcNow
                .ToUnixTimeSeconds ( )
                .ToString ( );
        }
    }
}