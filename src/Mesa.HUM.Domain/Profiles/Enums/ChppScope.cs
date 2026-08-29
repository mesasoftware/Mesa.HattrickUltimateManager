namespace Mesa.HUM.Domain.Profiles.Enums
{
    using System;

    [Flags]
    public enum ChppScope
    {
        /// <summary>
        /// The product can read your private team data and some private manager data, such as bookmarks, federation memberships, and achievements.
        /// It can not read private messages or forums.
        /// It can not change any data on your account.
        /// </summary>
        /// <remarks>
        /// Always granted.
        /// </remarks>
        ReadAccess = 0,

        /// <summary>
        /// The product can manage challenges for your team. Existing challenges can be accepted or denied, and new challenges can be created.
        /// </summary>
        ManageChallenges = 1,

        /// <summary>
        /// The product is allowed to set match orders for your coming matches.
        /// </summary>
        /// <remarks>
        /// Requires Supporter.
        /// </remarks>
        SetMatchOrder = 2,

        /// <summary>
        /// The product can manage your youth players. At the moment this only includes unlocking skills.
        /// </summary>
        ManageYouthPlayers = 4,

        /// <summary>
        /// The product is allowed to set training for any of your teams. This includes the type, percentage, and stamina share.
        /// </summary>
        /// <remarks>
        /// Requires Supporter.
        /// </remarks>
        SetTraining = 8,

        /// <summary>
        /// The product is allowed to place a bid on a player for sale in the name of any of your teams and remove hotlisted players.
        /// </summary>
        /// <remarks>
        /// Requires Supporter.
        /// </remarks>
        PlaceBid = 16
    }
}