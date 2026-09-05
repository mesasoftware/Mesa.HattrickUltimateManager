namespace Mesa.HUM.UI.Desktop.Converters
{
    using System;
    using System.Globalization;
    using Avalonia.Data.Converters;
    using Mesa.HUM.UI.Desktop.Resources;

    internal class EnumToLocalizedValueConverter : IValueConverter
    {
        public object? Convert ( object? value , Type targetType , object? parameter , CultureInfo culture )
        {
            return value is null
                ? null
                : parameter is null
                ? Translations.ResourceManager.GetStream ( $"{value.GetType ( ).Name}_{value}" )
                : Translations.ResourceManager.GetString ( $"{value.GetType ( ).Name}_{value}_{parameter}" );
        }

        public object? ConvertBack ( object? value , Type targetType , object? parameter , CultureInfo culture )
        {
            throw new InvalidOperationException ( "CANNOT_CONVERT_FROM_TRANSLATION_TO_ENUM" );
        }
    }
}