using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;
using static Assets.Scripts.PlayerLoop.PlayerLoopInteraction;
using static Assets.Scripts.PlayerLoop.PlayerLoopProfile;

namespace Assets.Scripts.PlayerLoop
{
    public class PlayerLoopProfileBuilder 
    {
        private List<Type> filteredSystems = new List<Type>();
        private FilterType filteredType = PlayerLoopProfile.FilterType.REMOVE;
        private List<PlayerLoopSystem> additionalSystems = new List<PlayerLoopSystem>();
        private Action<InteractionType> interactionAction = null;
        private List<InteractionType> ignoredInteraction = new List<InteractionType>();
        private Action timeoutAction = null;
        private float timeoutLength = 1;
        private Dictionary<Type, Test> UITest = new Dictionary<Type, Test>();
        private Dictionary<Type, ToolkitTest> UiToolkitTest = new Dictionary<Type, ToolkitTest>();
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

        public PlayerLoopProfileBuilder InteractionCallback(Action<InteractionType> pAction)
        {
            interactionAction = pAction;
            return this;
        }

        public PlayerLoopProfileBuilder IgnoreInteraction(params InteractionType[] pInteraction)
        {
            ignoredInteraction = pInteraction.ToList();
            return this;
        }

        public PlayerLoopProfileBuilder IgnoreInteraction(List<InteractionType> pInteraction)
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

        public PlayerLoopProfileBuilder UI(Type pType, ToolkitTest pSystems)
        {
            UiToolkitTest.TryAdd(pType, pSystems);
            return this;
        }

        public PlayerLoopProfileBuilder BaseSystem(PlayerLoopSystem pBaseSystem)
        {
            baseSystem = pBaseSystem;
            return this;
        }

        public PlayerLoopProfile Build()
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
                UITest,
                UiToolkitTest
            );
        }
    }
}