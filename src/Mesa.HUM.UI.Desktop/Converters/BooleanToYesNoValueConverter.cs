namespace Mesa.HUM.UI.Desktop.Converters
{
    using System;
    using System.Globalization;
    using Avalonia.Data.Converters;
    using Mesa.HUM.UI.Desktop.Resources;

    public class BooleanToYesNoValueConverter : IValueConverter
    {
        public object? Convert ( object? value , Type targetType , object? parameter , CultureInfo culture )
        {
#pragma warning disable IDE0046 // Convert to conditional expression
            if ( value is bool boolValue )
            {
                return boolValue
                    ? Translations.Yes
                    : Translations.No;
            }

            throw new InvalidCastException ( "INVALID_CAST_EXCEPTION" );
#pragma warning restore IDE0046 // Convert to conditional expression
        }

        public object? ConvertBack ( object? value , Type targetType , object? parameter , CultureInfo culture )
        {
            if ( value is string stringValue )
            {
#pragma warning disable IDE0046 // Convert to conditional expression
                if ( stringValue == Translations.Yes )
                {
                    return true;
                }
                else if ( stringValue == Translations.No )
                {
                    return false;
                }
                else
                {
                    throw new ArgumentOutOfRangeException ( nameof ( value ) , "ARGUMENT_OUT_OF_RANGE_EXCEPTION" );
                }
#pragma warning restore IDE0046 // Convert to conditional expression
            }

            throw new ArgumentException ( "UNEXPECTED_ARGUMENT_TYPE" , nameof ( value ) );
        }
    }
}