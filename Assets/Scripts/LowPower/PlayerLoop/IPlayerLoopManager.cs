using System.Collections;
using UnityEngine;

namespace Assets.Scripts.LowPower.PlayerLoop
{
    public interface IPlayerLoopManager
    {
        public bool AddProfile(System.Enum pKey, IPlayerLoopProfile pProfile);

        public bool AddProfile(int pKey, IPlayerLoopProfile pProfile);

        public bool RemoveProfile(System.Enum pKey);

        public bool RemoveProfile(int pKey);

        public IPlayerLoopProfile GetProfile(System.Enum pKey);

        public IPlayerLoopProfile GetProfile(int pKey);

        public void SetActiveProfile(System.Enum pKey);

        public void SetActiveProfile(int pKey);
    }
}