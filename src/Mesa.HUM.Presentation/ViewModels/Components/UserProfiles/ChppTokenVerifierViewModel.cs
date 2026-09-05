namespace Mesa.HUM.Presentation.ViewModels.Components.UserProfiles
{
    using System;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.Input;
    using Mesa.HUM.Presentation.Abstractions;

    public sealed class ChppTokenVerifierViewModel : ObservableComponent
    {
        public ChppTokenVerifierViewModel ( )
        {
            GetAccessTokenCommand = new AsyncRelayCommand ( OnGetAccessToken , ( ) => CanAuthorize );
            GoBackCommand = new RelayCommand ( OnGoBack );
        }

        public event Func<string , Task>? GetAccessTokenRequested;

        public event Action? GoBackRequested;

        public bool CanAuthorize
        {
            get
            {
                return !string.IsNullOrWhiteSpace ( Verifier );
            }
        }

        public IAsyncRelayCommand GetAccessTokenCommand { get; }

        public IRelayCommand GoBackCommand { get; }

        public string? Verifier
        {
            get;

            set
            {
                SetField (
                    ref field ,
                    value ,
                    nameof ( Verifier ) ,
                    nameof ( CanAuthorize ) );

                GetAccessTokenCommand.NotifyCanExecuteChanged ( );
            }
        }

        private Task OnGetAccessToken ( )
        {
            if ( string.IsNullOrWhiteSpace ( Verifier ) )
            {
                return Task.CompletedTask;
            }

            var handler = GetAccessTokenRequested;

            return handler is null
                ? Task.CompletedTask
                : handler ( Verifier );
        }

        private void OnGoBack ( )
        {
            Verifier = null;

            GoBackRequested?.Invoke ( );
        }
    }
}