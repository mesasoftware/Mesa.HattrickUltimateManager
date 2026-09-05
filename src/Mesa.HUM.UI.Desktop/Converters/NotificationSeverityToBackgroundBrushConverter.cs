namespace Mesa.HUM.UI.Desktop.Converters
{
    using System;
    using System.Globalization;
    using Avalonia;
    using Avalonia.Data.Converters;
    using Avalonia.Media;
    using Mesa.HUM.Presentation.Stores.Contracts;

    internal class NotificationSeverityToBackgroundBrushConverter : IValueConverter
    {
        public object? Convert ( object? value , Type targetType , object? parameter , CultureInfo culture )
        {
            if ( value is not NotificationSeverity severity )
            {
                throw new InvalidCastException ( "INVALID_CAST_EXCEPTION" );
            }

            string resourceKey = severity switch
            {
                NotificationSeverity.Error => "SystemFillColorCriticalBackgroundBrush",
                NotificationSeverity.Information => "SystemFillColorSolidAttentionBackgroundBrush",
                NotificationSeverity.Success => "SystemFillColorSuccessBackgroundBrush",
                NotificationSeverity.Warning => "SystemFillColorCautionBackgroundBrush",
                _ => "ControlFillColorDefault"
            };

            var application = Application.Current;

            ArgumentNullException.ThrowIfNull ( application );

            return application.TryGetResource ( resourceKey , application.ActualThemeVariant , out object? brush )
                ? brush
                : Brushes.Transparent;
        }

        public object? ConvertBack ( object? value , Type targetType , object? parameter , CultureInfo culture )
        {
            throw new InvalidOperationException ( "CANNOT_CONVERT_FROM_BRUSH_TO_SEVERITY" );
        }
    }
}