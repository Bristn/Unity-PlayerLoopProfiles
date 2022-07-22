using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using static Assets.Scripts.LowPower.PlayerLoop.IPlayerLoopProfile;

namespace Assets.Scripts.LowPower.PlayerLoop
{
    public class PlayerLoopProfileBuilder 
    {
        private List<Type> filteredSystems = new List<Type>();
        private FilterType filteredType = FilterType.REMOVE;

        private List<PlayerLoopSystem> additionalSystems = new List<PlayerLoopSystem>();
        private PlayerLoopSystem baseSystem = new PlayerLoopSystem();

        private Action interactionAction  =null;
        private Action timeoutAction = null;
        private int timeoutLength = 1;

        public PlayerLoopProfileBuilder Filter(List<Type> pSystems, FilterType pType)
        {
            filteredSystems = pSystems;
            filteredType = pType;
            return this;
        }

        public PlayerLoopProfileBuilder ExtraSystems(List<PlayerLoopSystem> pSystems)
        {
            additionalSystems = pSystems;
            return this;
        }

        public PlayerLoopProfileBuilder OnInteraction(Action pAction)
        {
            interactionAction = pAction;
            return this;
        }

        public PlayerLoopProfileBuilder OnTimeout(Action pAction)
        {
            timeoutAction = pAction;
            return this;
        }

        public PlayerLoopProfileBuilder TimeoutLength(int pLength)
        {
            timeoutLength = pLength;
            return this;
        }

        public PlayerLoopProfileBuilder BaseSystem(PlayerLoopSystem pBaseSystem)
        {
            baseSystem = pBaseSystem;
            return this;
        }


        public IPlayerLoopProfile build()
        {
            if (baseSystem.subSystemList == null || baseSystem.subSystemList.Length == 0)
            {
                baseSystem = UnityEngine.LowLevel.PlayerLoop.GetDefaultPlayerLoop();
            }

            return new PlayerLoopProfile(
                filteredSystems, 
                filteredType, 
                additionalSystems, 
                baseSystem, 
                interactionAction, 
                timeoutAction, 
                timeoutLength
            );
        }
    }
}