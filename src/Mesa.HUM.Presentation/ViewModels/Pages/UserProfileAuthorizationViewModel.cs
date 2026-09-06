namespace Mesa.HUM.Presentation.ViewModels.Pages
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.Input;
    using Mesa.HUM.Presentation.Abstractions.Interfaces;
    using Mesa.HUM.Presentation.Contracts;
    using Mesa.HUM.Presentation.Facades.Contracts.Authorization;
    using Mesa.HUM.Presentation.Facades.Interfaces;
    using Mesa.HUM.Presentation.Stores.Enums;
    using Mesa.HUM.Presentation.Stores.Interfaces;
    using Mesa.HUM.Presentation.ViewModels.Abstractions;
    using Mesa.HUM.Presentation.ViewModels.Abstractions.Interfaces;
    using Mesa.HUM.Presentation.ViewModels.Components.UserProfiles;
    using Mesa.HUM.Presentation.ViewModels.Pages.Enums;

    public sealed class UserProfileAuthorizationViewModel : PageViewModelBase, IInitializableViewModel
    {
        private readonly IAuthorizationFacade _authorizationFacade;

        private readonly CancellationTokenSource _cancellationTokenSource;

        private readonly ILinkLauncher _linkLauncher;

        private readonly INotificationsStore _notificationStore;

        private readonly IUserProfileStore _userProfileStore;

        private string? _authorizationUrl;

        private RequestTokenDto? _requestToken;

        public UserProfileAuthorizationViewModel (
            IAuthorizationFacade authorizationFacade ,
            ILinkLauncher linkLauncher ,
            INotificationsStore notificationStore ,
            IUserProfileStore userProfileStore ,
            Scope [ ] scopes )
        {
            _authorizationFacade = authorizationFacade;
            _linkLauncher = linkLauncher;
            _notificationStore = notificationStore;
            _userProfileStore = userProfileStore;

            _cancellationTokenSource = new CancellationTokenSource ( );

            var scopeItems = scopes
                .Select ( x => new ChppScopeItemModel ( x.Name , x.RequiresSupporter , x.Value ) )
                .ToArray ( );

            ChppScopePicker = new ChppScopePickerViewModel ( scopeItems );

            ChppScopePicker.CopyLinkRequested += ChppScopePicker_CopyLinkRequested;
            ChppScopePicker.OpenLinkRequested += ChppScopePicker_OpenLinkRequested;

            ChppTokenVerifier = new ChppTokenVerifierViewModel ( );

            ChppTokenVerifier.GetAccessTokenRequested += ChppTokenVerifier_GetAccessTokenRequested;
            ChppTokenVerifier.GoBackRequested += ChppTokenVerifier_GoBackRequested;

            GoToStageCommand = new RelayCommand<UserProfileAuthorizationStage> ( OnGoToStage );
        }

        public ChppScopePickerViewModel ChppScopePicker { get; }

        public ChppTokenVerifierViewModel ChppTokenVerifier { get; }

        public IRelayCommand<UserProfileAuthorizationStage> GoToStageCommand { get; }

        public bool ShowChppScopePicker
        { get { return Stage == UserProfileAuthorizationStage.Start; } }

        public bool ShowChppTokenDetails
        { get { return Stage == UserProfileAuthorizationStage.View; } }

        public bool ShowChppTokenVerifier
        { get { return Stage == UserProfileAuthorizationStage.Complete; } }

        public UserProfileAuthorizationStage Stage
        {
            get;

            private set
            {
                SetField (
                    ref field ,
                    value ,
                    nameof ( Stage ) ,
                    nameof ( ShowChppScopePicker ) ,
                    nameof ( ShowChppTokenVerifier ) ,
                    nameof ( ShowChppTokenDetails ) );
            }
        }

        public Task InitializeAsync ( CancellationToken cancellationToken = default )
        {
            cancellationToken.ThrowIfCancellationRequested ( );

            Stage = _userProfileStore.UserProfile?.IsAuthorized ?? false
                ? UserProfileAuthorizationStage.View
                : UserProfileAuthorizationStage.Start;

            return Task.CompletedTask;
        }

        protected override void Dispose ( bool disposing )
        {
            if ( disposing )
            {
                ChppScopePicker.CopyLinkRequested -= ChppScopePicker_CopyLinkRequested;
                ChppScopePicker.OpenLinkRequested -= ChppScopePicker_OpenLinkRequested;
                ChppTokenVerifier.GetAccessTokenRequested -= ChppTokenVerifier_GetAccessTokenRequested;
                ChppTokenVerifier.GoBackRequested -= ChppTokenVerifier_GoBackRequested;
            }

            base.Dispose ( disposing );
        }

        private async Task ChppScopePicker_CopyLinkRequested ( string [ ]? scopes )
        {
            await StartChppAuthorizationAsync ( scopes );

            if ( !string.IsNullOrWhiteSpace ( _authorizationUrl ) && _requestToken is not null )
            {
                await TextCopy.ClipboardService.SetTextAsync ( _authorizationUrl );

                Stage = UserProfileAuthorizationStage.Complete;
            }
        }

        private async Task ChppScopePicker_OpenLinkRequested ( string [ ]? scopes )
        {
            await StartChppAuthorizationAsync ( scopes );

            if ( !string.IsNullOrWhiteSpace ( _authorizationUrl ) && _requestToken is not null )
            {
                _linkLauncher.Open ( _authorizationUrl );

                Stage = UserProfileAuthorizationStage.Complete;
            }
        }

        private async Task ChppTokenVerifier_GetAccessTokenRequested ( string verifier )
        {
            ArgumentNullException.ThrowIfNull ( _requestToken );
            ArgumentException.ThrowIfNullOrWhiteSpace ( verifier );

            var response = await _authorizationFacade.GetAccessTokenAsync (
                _requestToken ,
                verifier ,
                _cancellationTokenSource.Token );

            if ( response.IsSuccess )
            {
                Stage = UserProfileAuthorizationStage.View;
            }
            else
            {
                ArgumentNullException.ThrowIfNull ( response.Error );

                _notificationStore.Notify ( response.Error.Description , NotificationSeverity.Error );
            }
        }

        private void ChppTokenVerifier_GoBackRequested ( )
        {
            Stage = UserProfileAuthorizationStage.Start;
        }

        private void OnGoToStage ( UserProfileAuthorizationStage value )
        {
            Stage = value;
        }

        private async Task StartChppAuthorizationAsync ( string [ ]? scopes )
        {
            _authorizationUrl = null;
            _requestToken = null;

            var response = await _authorizationFacade.GetAuthorizationUrlAsync ( scopes , _cancellationTokenSource.Token );

            if ( response.IsSuccess )
            {
                _authorizationUrl = response.Value.AuthorizationUrl;
                _requestToken = response.Value.RequestToken;
            }
            else
            {
                ArgumentNullException.ThrowIfNull ( response.Error );

                _notificationStore.Notify ( response.Error.Description , NotificationSeverity.Error );
            }
        }
    }
}