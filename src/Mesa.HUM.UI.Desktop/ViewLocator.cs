namespace Mesa.HUM.UI.Desktop
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Avalonia.Controls;
    using Avalonia.Controls.Templates;
    using Mesa.HUM.Presentation.ViewModels.Abstractions;

    /// <summary>
    /// Given a view model, returns the corresponding view if possible.
    /// </summary>
    [ExcludeFromCodeCoverage, RequiresUnreferencedCode (
        "Default implementation of ViewLocator involves reflection which may be trimmed away." ,
        Url = "https://docs.avaloniaui.net/docs/concepts/view-locator" )]
    public class ViewLocator : IDataTemplate
    {
        public Control? Build ( object? param )
        {
            if ( param is null )
            {
                return null;
            }

            string? sourceName = param.GetType ( ).FullName;

            ArgumentException.ThrowIfNullOrWhiteSpace ( sourceName );

            string targetName = sourceName
                .Replace ( "Presentation" , "UI.Desktop" )
                .Replace ( "ViewModels" , "Views" )
                .Replace ( "ViewModel" , "Page" );

            var type = Type.GetType ( targetName );

            return type != null
                ? ( Control ) Activator.CreateInstance ( type )!
                : new TextBlock { Text = "Not Found: " + targetName };
        }

        public bool Match ( object? data )
        {
            return data is ViewModelBase;
        }
    }
}