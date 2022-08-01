using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.LowPower.PlayerLoop
{
    public class PlayerLoopManager
    {
        private Dictionary<int, PlayerLoopProfile> profiles = new Dictionary<int, PlayerLoopProfile>();
        private int activeProfile = -1;

        private LowPowerTimeout timeout;

        public int PreventProfileChange { get; set; } = 0;

        public PlayerLoopManager(LowPowerTimeout pTimeout)
        {
            timeout = pTimeout;
            LowPowerInteraction.Instance.Timeout = timeout;
        }

        public bool AddProfile(System.Enum pKey, PlayerLoopProfile pProfile) => profiles.TryAdd(pKey.ToInt(), pProfile);

        public bool AddProfile(int pKey, PlayerLoopProfile pProfile) => profiles.TryAdd(pKey, pProfile);

        public bool RemoveProfile(System.Enum pKey) => profiles.Remove(pKey.ToInt());

        public bool RemoveProfile(int pKey) => profiles.Remove(pKey);

        public PlayerLoopProfile GetProfile(System.Enum pKey) => profiles.GetValueOrDefault(pKey.ToInt(), null);

        public PlayerLoopProfile GetProfile(int pKey) => profiles.GetValueOrDefault(pKey, null);

        public void SetActiveProfile(System.Enum pKey) => SetActiveProfile(pKey.ToInt());

        public void SetActiveProfile(int pKey)
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

            timeout.Profile = profile;
            UnityEngine.LowLevel.PlayerLoop.SetPlayerLoop(profile.GetResultingSystem());
        }
    }
}