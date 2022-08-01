using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.LowLevel;
using static LowPowerInteraction;

namespace Assets.Scripts.LowPower.PlayerLoop
{
    public class PlayerLoopProfile
    {
        private PlayerLoopSystem baseSystem;

        public enum FilterType
        {
            KEEP,
            REMOVE
        }

        public delegate bool Test(Component pObject);

        public PlayerLoopProfile(
            List<Type> pFilteredSystems, 
            FilterType pFilteredType, 
            List<PlayerLoopSystem> pAdditionalSystems, 
            PlayerLoopSystem pBaseSystem,
            Action<ActionType> pInteractionAction,
            List<ActionType> pIgnoredInteraction,
            Action pTimeoutAction,
            float pTimeoutDuration,
            Dictionary<Type, Test> pUITest)
        {
            FilteredType = pFilteredType;
            FilteredSystems = pFilteredSystems;
            AdditionalSystems = pAdditionalSystems;
            InteractionAction = pInteractionAction;
            IgnoredInteraction = pIgnoredInteraction;
            TimeoutAction = pTimeoutAction;
            TimeoutDuration = pTimeoutDuration;
            UITest = pUITest;

            baseSystem = pBaseSystem;
        }

        public List<Type> FilteredSystems { get; private set; }

        public FilterType FilteredType { get; private set; }

        public List<PlayerLoopSystem> AdditionalSystems { get; private set; }

        public Action<ActionType> InteractionAction { get; private set; }

        public List<ActionType> IgnoredInteraction { get; private set; }

        public Action TimeoutAction { get; private set; }

        public float TimeoutDuration { get; private set; }

        public Dictionary<Type, Test> UITest  { get; private set; }

        public PlayerLoopSystem GetResultingSystem()
        {
            Filter filter = FilteredType == FilterType.KEEP ? KeepFilter : RemoveFilter;
            PlayerLoopSystem system = GetResultingSystem(baseSystem, filter);

            List<PlayerLoopSystem> subSystems = system.subSystemList.ToList();
            subSystems.AddRange(AdditionalSystems);
            subSystems.Add(LowPowerTimeout.UpdateSystem);
            system.subSystemList = subSystems.ToArray();

            return system;
        }

        public void PrintProfile()
        {
            StringBuilder builder = new StringBuilder();
            GetTreeRecursive(GetResultingSystem(), builder, 0);
            Debug.Log(builder.ToString());
        }

#region Helper_for_printing
        private void GetTreeRecursive(PlayerLoopSystem pSystem, StringBuilder pBuilder, int pDepth)
        {
            if (pDepth == 0)
            {
                pBuilder.AppendLine("SYSTEM ROOT");
            }
            else if (pSystem.type != null)
            {
                for (int i = 0; i < pDepth; i++)
                {
                    pBuilder.Append("\t");
                }
                pBuilder.AppendLine(pSystem.type.Name);
            }

            if (pSystem.subSystemList != null)
            {
                pDepth++;
                foreach (PlayerLoopSystem subSystem in pSystem.subSystemList)
                {
                    GetTreeRecursive(subSystem, pBuilder, pDepth);
                }
                pDepth--;
            }
        }
#endregion


#region Helper_for_filtering
        private delegate bool Filter(PlayerLoopSystem pSystem);

        private PlayerLoopSystem GetResultingSystem(PlayerLoopSystem pSystem, Filter pFilter)
        {
            if (pFilter.Invoke(pSystem))
            {
                return pSystem;
            }

            if (pSystem.subSystemList == null || pSystem.subSystemList.Length == 0)
            {
                return PlayerLoopDummy.GetDummy();
            }
            else
            {
                List<PlayerLoopSystem> systems = new List<PlayerLoopSystem>();
                foreach (PlayerLoopSystem subSystem in pSystem.subSystemList)
                {
                    PlayerLoopSystem current = GetResultingSystem(subSystem, pFilter);
                    if (!PlayerLoopDummy.IsDummy(current))
                    {
                        systems.Add(current);
                    }
                }

                if (systems.Count == 0)
                {
                    return PlayerLoopDummy.GetDummy();
                }

                pSystem.subSystemList = systems.ToArray();
                return pSystem;
            }
        }

        private bool KeepFilter(PlayerLoopSystem pSystem) => FilteredSystems.Contains(pSystem.type);

        private bool RemoveFilter(PlayerLoopSystem pSystem) => !FilteredSystems.Contains(pSystem.type);

        private class PlayerLoopDummy
        {
            public static PlayerLoopSystem GetDummy()
            {
                PlayerLoopSystem dummy = new PlayerLoopSystem();
                dummy.type = typeof(PlayerLoopDummy);
                dummy.subSystemList = new PlayerLoopSystem[0];
                return dummy;
            }

            public static bool IsDummy(PlayerLoopSystem pSystem)
            {
                return pSystem.type == typeof(PlayerLoopDummy);
            }
        }
#endregion
    }
}