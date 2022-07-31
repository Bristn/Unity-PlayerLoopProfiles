using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;
using static Assets.Scripts.LowPower.LowPowerImplementation;
using static Assets.Scripts.LowPower.PlayerLoop.IPlayerLoopProfile;
using static LowPowerInteraction;

namespace Assets.Scripts.LowPower.PlayerLoop
{
    public class PlayerLoopProfileBuilder 
    {
        private List<Type> filteredSystems = new List<Type>();
        private FilterType filteredType = IPlayerLoopProfile.FilterType.REMOVE;
        private List<PlayerLoopSystem> additionalSystems = new List<PlayerLoopSystem>();
        private Action<ActionType> interactionAction = null;
        private List<ActionType> ignoredInteraction = new List<ActionType>();
        private Action timeoutAction = null;
        private float timeoutLength = 1;
        private Dictionary<Type, Test> UITest = new Dictionary<Type, Test>();
        private PlayerLoopSystem baseSystem = new PlayerLoopSystem();

        public PlayerLoopProfileBuilder FilterSystems(params Type[] pSystems)
        {
            filteredSystems = pSystems.ToList();
            return this;
        }

        public PlayerLoopProfileBuilder FilterSystems(List<Type> pSystems)
        {
            filteredSystems = pSystems;
            return this;
        }

        public PlayerLoopProfileBuilder FilterType(FilterType pType)
        {
            filteredType = pType;
            return this;
        }

        public PlayerLoopProfileBuilder AdditionalSystems(params PlayerLoopSystem[] pSystems)
        {
            additionalSystems = pSystems.ToList();
            return this;
        }

        public PlayerLoopProfileBuilder AdditionalSystems(List<PlayerLoopSystem> pSystems)
        {
            additionalSystems = pSystems;
            return this;
        }

        public PlayerLoopProfileBuilder InteractionCallback(Action<ActionType> pAction)
        {
            interactionAction = pAction;
            return this;
        }

        public PlayerLoopProfileBuilder IgnoreInteraction(params ActionType[] pInteraction)
        {
            ignoredInteraction = pInteraction.ToList();
            return this;
        }

        public PlayerLoopProfileBuilder IgnoreInteraction(List<ActionType> pInteraction)
        {
            ignoredInteraction = pInteraction;
            return this;
        }
        
        public PlayerLoopProfileBuilder TimeoutCallback(Action pAction)
        {
            timeoutAction = pAction;
            return this;
        }

        public PlayerLoopProfileBuilder TimeoutDuration(float pLength)
        {
            timeoutLength = pLength;
            return this;
        }

        public PlayerLoopProfileBuilder UI(Type pType, Test pSystems)
        {
            UITest.TryAdd(pType, pSystems);
            return this;
        }

        public PlayerLoopProfileBuilder BaseSystem(PlayerLoopSystem pBaseSystem)
        {
            baseSystem = pBaseSystem;
            return this;
        }

        public IPlayerLoopProfile Build()
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
                ignoredInteraction,
                timeoutAction, 
                timeoutLength,
                UITest
            );
        }
    }
}