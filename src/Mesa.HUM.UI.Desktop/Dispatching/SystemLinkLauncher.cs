namespace Mesa.HUM.UI.Desktop.Dispatching
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using Mesa.HUM.Presentation.Abstractions.Interfaces;

    [ExcludeFromCodeCoverage]
    public sealed class SystemLinkLauncher : ILinkLauncher
    {
        public void Open ( string url )
        {
            Process.Start (
                new ProcessStartInfo ( url )
                {
                    UseShellExecute = true
                } );
        }
    }
}