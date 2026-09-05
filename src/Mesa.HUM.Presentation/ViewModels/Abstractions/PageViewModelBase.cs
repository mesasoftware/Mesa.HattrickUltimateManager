namespace Mesa.HUM.Presentation.ViewModels.Abstractions
{
    using System;

    public abstract class PageViewModelBase : ViewModelBase, IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this view model has already been disposed.
        /// Derived classes can check this before touching state that cleanup has released.
        /// </summary>
        protected bool IsDisposed { get; private set; }

        public void Dispose ( )
        {
            if ( IsDisposed )
            {
                return;
            }

            Dispose ( true );

            IsDisposed = true;

            GC.SuppressFinalize ( this );
        }

        /// <summary>
        /// Releases resources held by the page view model. Override to unsubscribe from
        /// events and dispose owned children, then call <c>base.Dispose ( disposing )</c>.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> when called from <see cref="Dispose()" /> (safe to touch other m
        /// objects); <c>false</c> when called from a finalizer (managed objects may already be gone).
        /// </param>
        protected virtual void Dispose ( bool disposing )
        {
        }
    }
}