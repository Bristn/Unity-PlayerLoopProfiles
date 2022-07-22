using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

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

        public Action InteractionAction { get; }

        public Action TimeoutAction { get; }

        public float TimeoutDuration { get; }

        public PlayerLoopSystem GetResultingSystem();

        public void PrintProfile();
    }
}