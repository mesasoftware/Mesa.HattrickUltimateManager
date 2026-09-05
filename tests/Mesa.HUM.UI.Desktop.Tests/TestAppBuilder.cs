using Avalonia.Headless;
using Mesa.HUM.UI.Desktop.Tests;

[assembly: AvaloniaTestApplication ( typeof ( TestAppBuilder ) )]

namespace Mesa.HUM.UI.Desktop.Tests
{
    using Avalonia;
    using Avalonia.Headless;

    public static class TestAppBuilder
    {
        public static AppBuilder BuildAvaloniaApp ( )
        {
            return AppBuilder
                .Configure<HeadlessTestApp> ( )
                .UseHeadless ( new AvaloniaHeadlessPlatformOptions ( ) );
        }
    }
}