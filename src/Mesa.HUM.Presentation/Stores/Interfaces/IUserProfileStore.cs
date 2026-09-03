namespace Mesa.HUM.Presentation.Stores.Interfaces
{
    using Mesa.HUM.Presentation.Stores.Contracts;

    public interface IUserProfileStore
    {
        UserProfileDto? UserProfile { get; }

        void SetUserProfile ( UserProfileDto userProfileDto );
    }
}