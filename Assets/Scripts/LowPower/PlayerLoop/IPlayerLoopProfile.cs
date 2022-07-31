using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using static Assets.Scripts.LowPower.LowPowerImplementation;
using static LowPowerInteraction;

namespace Assets.Scripts.LowPower.PlayerLoop
{
    public interface IPlayerLoopProfile
    {
        public enum FilterType
        {
            KEEP, 
            REMOVE
        }

        public List<Type> FilteredSystems { get; }

        public FilterType FilteredType { get; }

        public List<PlayerLoopSystem> AdditionalSystems { get; }

        public Action<ActionType> InteractionAction { get; }

        public List<ActionType> IgnoredInteraction { get; }

        public Dictionary<Type, Test> UITest { get; }

        public Action TimeoutAction { get; }

        public float TimeoutDuration { get; }

        public PlayerLoopSystem GetResultingSystem();

        public void PrintProfile();

        public delegate bool Test(Component pObject);
    }
}