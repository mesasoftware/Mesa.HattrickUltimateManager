namespace Mesa.HUM.Presentation.Abstractions.Interfaces
{
    using System;

    /// <summary>
    /// Marshals an <see cref="Action" /> onto the UI thread. Implemented in the presentation host
    /// (for example, over Avalonia's dispatcher) so the presentation layer stays platform-agnostic.
    /// </summary>
    public interface IUIDispatcher
    {
        /// <summary>
        /// Posts <paramref name="action" /> to the UI thread for asynchronous execution.
        /// </summary>
        /// <param name="action">The work to run on the UI thread.</param>
        void Post ( Action action );
    }
}