namespace Mesa.HUM.Presentation.ViewModels.Pages
{
    using System.Threading;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.Input;
    using Mesa.HUM.Presentation.Contracts;
    using Mesa.HUM.Presentation.Stores.Interfaces;
    using Mesa.HUM.Presentation.ViewModels.Abstractions;
    using Mesa.HUM.Presentation.ViewModels.Abstractions.Interfaces;
    using Mesa.HUM.Presentation.ViewModels.Pages.Enums;

    public sealed class UserProfileAuthorizationViewModel : PageViewModelBase, IInitializableViewModel
    {
        private readonly IUserProfileStore _userProfileStore;

        public UserProfileAuthorizationViewModel ( IUserProfileStore userProfileStore , Scopes [ ] scopes )
        {
            _userProfileStore = userProfileStore;
            GoToStageCommand = new RelayCommand<UserProfileAuthorizationStage> ( OnGoToStage );
        }

        public IRelayCommand<UserProfileAuthorizationStage> GoToStageCommand { get; }

        public UserProfileAuthorizationStage Stage
        {
            get;

            set { SetField ( ref field , value ); }
        }

        public Task InitializeAsync ( CancellationToken cancellationToken = default )
        {
            cancellationToken.ThrowIfCancellationRequested ( );

            Stage = _userProfileStore.UserProfile?.IsAuthorized ?? false
                ? UserProfileAuthorizationStage.View
                : UserProfileAuthorizationStage.Start;

            return Task.CompletedTask;
        }

        private void OnGoToStage ( UserProfileAuthorizationStage value )
        {
            Stage = value;
        }
    }
}