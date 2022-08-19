using System.Collections.Generic;

namespace PlayerLoopProfiles
{
    public static class PlayerLoopManager
    {
        private static Dictionary<int, PlayerLoopProfile> profiles = new Dictionary<int, PlayerLoopProfile>();
        private static int activeProfile = -1;

        /// <summary>
        /// IF this is set to anything besides 0 any profile change is blocked. 
        /// Besides blocking profile changes the timeout timer is stopped as well, meaning no 'TimeoutCallback's are invoked as well.
        /// This might be useful if profile changes aren't desired in a sepcified time span. 
        /// </summary>
        public static int PreventProfileChange { get; set; } = 0;

        /// <summary>
        /// Add/Register the profile with the corresponding enum key.
        /// The enum gets converted to an integer internally. 
        /// </summary>
        public static bool AddProfile(System.Enum pKey, PlayerLoopProfile pProfile) => profiles.TryAdd(pKey.ToInt(), pProfile);
        
        /// <summary>
        /// Add/Register the profile with the corresponding integer key.
        /// </summary>
        public static bool AddProfile(int pKey, PlayerLoopProfile pProfile) => profiles.TryAdd(pKey, pProfile);

        /// <summary>
        /// Remove profile with the corresponding enum key.
        /// The enum gets converted to an integer internally. 
        /// This might cause weird behaviour if the profile gets registered with another enum to the one when removing. 
        /// </summary>
        public static bool RemoveProfile(System.Enum pKey) => profiles.Remove(pKey.ToInt());

        /// <summary>
        /// Remove profile with the corresponding integer key.
        /// </summary>
        public static bool RemoveProfile(int pKey) => profiles.Remove(pKey);

        /// <summary>
        /// Returns the profile with the given enum key. 
        /// The enum gets converted to an integer internally. 
        /// This might cause weird behaviour if the profile gets registered with another enum to the one when querying. 
        /// </summary>
        public static PlayerLoopProfile GetProfile(System.Enum pKey) => profiles.GetValueOrDefault(pKey.ToInt(), null);

        /// <summary>
        /// Returns the profile with the given integer key. 
        /// </summary>
        public static PlayerLoopProfile GetProfile(int pKey) => profiles.GetValueOrDefault(pKey, null);

        /// <summary>
        /// Retrieves the active profile.
        /// /// </summary>
        public static int GetActiveProfile() => activeProfile;

        /// <summary>
        /// Set the active profile to the one corresponding to the enum key.
        /// The enum gets converted to an integer internally. 
        /// This might cause weird behaviour if the profile gets registered with another enum to the one when activating. 
        /// </summary>
        public static void SetActiveProfile(System.Enum pKey) => SetActiveProfile(pKey.ToInt());

        /// <summary>
        /// Set the active profile to the one corresponding to the integer key.
        /// </summary>
        public static void SetActiveProfile(int pKey)
        {
            if (activeProfile == pKey || PreventProfileChange > 0)
            {
                return;
            }
            activeProfile = pKey;

            PlayerLoopProfile profile = GetProfile(activeProfile);
            if (profile == null)
            {
                return;
            }

            PlayerLoopTimeout.Profile = profile;
            UnityEngine.LowLevel.PlayerLoop.SetPlayerLoop(profile.GetResultingSystem());
        }

        private static int ToInt(this System.Enum pEnum) => (int)(object)pEnum;
    }
}