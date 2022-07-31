using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.LowPower.PlayerLoop
{
    public class PlayerLoopManager : IPlayerLoopManager
    {
        private Dictionary<int, IPlayerLoopProfile> profiles = new Dictionary<int, IPlayerLoopProfile>();
        private int activeProfile = -1;

        private LowPowerDispatcher dispatcher;
        private LowPowerTimeout timeout;

        private Thread mainThread;

        public PlayerLoopManager(LowPowerDispatcher pDispatcher, LowPowerTimeout pTimeout)
        {
            dispatcher = pDispatcher;
            timeout = pTimeout;
            LowPowerInteraction.Instance.Timeout = timeout;

            mainThread = Thread.CurrentThread;
        }

        public bool AddProfile(System.Enum pKey, IPlayerLoopProfile pProfile) => profiles.TryAdd(pKey.ToInt(), pProfile);

        public bool AddProfile(int pKey, IPlayerLoopProfile pProfile) => profiles.TryAdd(pKey, pProfile);

        public bool RemoveProfile(System.Enum pKey) => profiles.Remove(pKey.ToInt());

        public bool RemoveProfile(int pKey) => profiles.Remove(pKey);

        public IPlayerLoopProfile GetProfile(System.Enum pKey) => profiles.GetValueOrDefault(pKey.ToInt(), null);

        public IPlayerLoopProfile GetProfile(int pKey) => profiles.GetValueOrDefault(pKey, null);

        public void SetActiveProfile(System.Enum pKey) => SetActiveProfile(pKey.ToInt());

        public void SetActiveProfile(int pKey)
        {
            if (Thread.CurrentThread == mainThread)
            {
                SetActiveProfileDispatched(pKey);
            }
            else
            {
                dispatcher.DispatchEvent(SetActiveProfileDispatched, pKey);
            }
        }

        private void SetActiveProfileDispatched(int pKey)
        {
            if (activeProfile == pKey)
            {
                return;
            }
            activeProfile = pKey;

            IPlayerLoopProfile profile = GetProfile(activeProfile);
            if (profile == null)
            {
                return;
            }

            timeout.Profile = profile;
            UnityEngine.LowLevel.PlayerLoop.SetPlayerLoop(profile.GetResultingSystem());
        }
    }
}