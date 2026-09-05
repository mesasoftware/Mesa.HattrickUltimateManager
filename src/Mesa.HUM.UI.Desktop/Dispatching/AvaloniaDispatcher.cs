namespace Mesa.HUM.UI.Desktop.Dispatching
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Avalonia.Threading;
    using Mesa.HUM.Presentation.Abstractions.Interfaces;

    [ExcludeFromCodeCoverage]
    public sealed class AvaloniaDispatcher : IUIDispatcher
    {
        public void Post ( Action action )
        {
            Dispatcher.UIThread.Post ( action );
        }
    }
}