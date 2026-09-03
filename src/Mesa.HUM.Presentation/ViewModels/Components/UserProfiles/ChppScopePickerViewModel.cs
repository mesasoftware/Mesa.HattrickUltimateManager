namespace Mesa.HUM.Presentation.ViewModels.Components.UserProfiles
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.Input;
    using Mesa.HUM.Presentation.Abstractions;

    public sealed class ChppScopePickerViewModel : ObservableComponent
    {
        public ChppScopePickerViewModel ( ChppScopeItemModel [ ] scopes )
        {
            ArgumentNullException.ThrowIfNull ( scopes );

            if ( scopes.Length == 0 )
            {
                throw new ArgumentException ( "SCOPES_CANNOT_BE_EMPTY" , nameof ( scopes ) );
            }

            Scopes = scopes;

            CopyLinkCommand = new AsyncRelayCommand ( OnCopyLinkAsync );
            OpenLinkCommand = new AsyncRelayCommand ( OnOpenLinkAsync );
        }

        public event Func<string [ ]? , Task>? CopyLinkRequested;

        public event Func<string [ ]? , Task>? OpenLinkRequested;

        public IAsyncRelayCommand CopyLinkCommand { get; }

        public IAsyncRelayCommand OpenLinkCommand { get; }

        public ChppScopeItemModel [ ] Scopes { get; }

        public string [ ] SelectedScopes
        {
            get
            {
                return [ .. Scopes
                    .Where ( item => item.IsSelected )
                    .Select ( item => item.Value ) ];
            }
        }

        private Task OnCopyLinkAsync ( )
        {
            return CopyLinkRequested is null
                ? Task.CompletedTask
                : CopyLinkRequested ( SelectedScopes );
        }

        private Task OnOpenLinkAsync ( )
        {
            return OpenLinkRequested is null
                ? Task.CompletedTask
                : OpenLinkRequested ( SelectedScopes );
        }
    }
}