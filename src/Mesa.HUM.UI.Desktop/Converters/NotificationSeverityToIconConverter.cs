namespace Mesa.HUM.UI.Desktop.Converters
{
    using System;
    using System.Globalization;
    using Avalonia;
    using Avalonia.Data.Converters;
    using Mesa.HUM.Presentation.Stores.Enums;

    internal class NotificationSeverityToIconConverter : IValueConverter
    {
        public object? Convert ( object? value , Type targetType , object? parameter , CultureInfo culture )
        {
            if ( value is not NotificationSeverity severity )
            {
                throw new InvalidCastException ( "INVALID_CAST_EXCEPTION" );
            }

            var application = Application.Current;

            ArgumentNullException.ThrowIfNull ( application );

            // The StreamGeometry resource keys (Information, Success, Warning, Error) match the
            // NotificationSeverity member names, so the enum value resolves the icon directly.
            return application.TryGetResource ( severity.ToString ( ) , application.ActualThemeVariant , out object? geometry )
                ? geometry
                : null;
        }

        public object? ConvertBack ( object? value , Type targetType , object? parameter , CultureInfo culture )
        {
            throw new InvalidOperationException ( "CANNOT_CONVERT_FROM_ICON_TO_SEVERITY" );
        }
    }
}