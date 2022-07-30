using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;
using static Assets.Scripts.LowPower.LowPowerImplementation;
using static Assets.Scripts.LowPower.PlayerLoop.IPlayerLoopProfile;

namespace Assets.Scripts.LowPower.PlayerLoop
{
    public class PlayerLoopProfileBuilder 
    {
#region Filter
        private List<Type> filteredSystems = new List<Type>();
        private FilterType filteredType = IPlayerLoopProfile.FilterType.REMOVE;

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
#endregion



#region Additional
        private List<PlayerLoopSystem> additionalSystems = new List<PlayerLoopSystem>();

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
#endregion



#region Interaction
        private Action<InteractionType> interactionAction = null;
        private List<InteractionType> ignoredInteraction = new List<InteractionType>();

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
#endregion



#region Timeout
        private Action timeoutAction = null;
        private int timeoutLength = 1;

        public PlayerLoopProfileBuilder TimeoutCallback(Action pAction)
        {
            timeoutAction = pAction;
            return this;
        }

        public PlayerLoopProfileBuilder TimeoutLength(int pLength)
        {
            timeoutLength = pLength;
            return this;
        }
#endregion


#region UITESST
        private Dictionary<Type, Test> UITest = new Dictionary<Type, Test>();


        public PlayerLoopProfileBuilder UI(Dictionary<Type, Test> pSystems)
        {
            UITest = pSystems;
            return this;
        }
#endregion



        #region Base
        private PlayerLoopSystem baseSystem = new PlayerLoopSystem();

        public PlayerLoopProfileBuilder BaseSystem(PlayerLoopSystem pBaseSystem)
        {
            baseSystem = pBaseSystem;
            return this;
        }
#endregion



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