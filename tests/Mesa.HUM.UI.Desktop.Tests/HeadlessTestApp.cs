namespace Mesa.HUM.UI.Desktop.Tests
{
    using System;
    using Avalonia;
    using Avalonia.Markup.Xaml.Styling;
    using FluentAvalonia.Styling;

    /// <summary>
    /// Minimal Avalonia application used only for headless control tests. It loads the theme and
    /// icon resources the views rely on, but deliberately does not run the production dependency
    /// injection host (which needs real configuration and a database), so control rendering can be
    /// exercised in isolation.
    /// </summary>
    public sealed class HeadlessTestApp : Application
    {
        public override void Initialize ( )
        {
            Styles.Add ( new FluentAvaloniaTheme ( ) );

            Styles.Add (
                new StyleInclude ( new Uri ( "avares://Mesa.HUM.UI.Desktop.Tests/" ) )
                {
                    Source = new Uri ( "avares://Mesa.HUM.UI.Desktop/Assets/Svg/Icons.axaml" )
                } );
        }
    }
}