using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.PlayerLoop
{
    public static class PlayerLoopManager
    {
        private static Dictionary<int, PlayerLoopProfile> profiles = new Dictionary<int, PlayerLoopProfile>();
        private static int activeProfile = -1;


        public static int PreventProfileChange { get; set; } = 0;

        public static bool AddProfile(System.Enum pKey, PlayerLoopProfile pProfile) => profiles.TryAdd(pKey.ToInt(), pProfile);

        public static bool AddProfile(int pKey, PlayerLoopProfile pProfile) => profiles.TryAdd(pKey, pProfile);

        public static bool RemoveProfile(System.Enum pKey) => profiles.Remove(pKey.ToInt());

        public static bool RemoveProfile(int pKey) => profiles.Remove(pKey);

        public static PlayerLoopProfile GetProfile(System.Enum pKey) => profiles.GetValueOrDefault(pKey.ToInt(), null);

        public static PlayerLoopProfile GetProfile(int pKey) => profiles.GetValueOrDefault(pKey, null);

        public static void SetActiveProfile(System.Enum pKey) => SetActiveProfile(pKey.ToInt());

        public static void SetActiveProfile(int pKey)
        {
            if (activeProfile == pKey)
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