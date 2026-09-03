namespace Mesa.HUM.Presentation.Stores
{
    using Mesa.HUM.Presentation.Abstractions;
    using Mesa.HUM.Presentation.Stores.Contracts;
    using Mesa.HUM.Presentation.Stores.Interfaces;

    public class UserProfileStore : ObservableComponent, IUserProfileStore
    {
        public UserProfileDto? UserProfile
        {
            get;

            private set { SetField ( ref field , value ); }
        }

        public void SetUserProfile ( UserProfileDto userProfileDto )
        {
            UserProfile = userProfileDto;
        }
    }
}