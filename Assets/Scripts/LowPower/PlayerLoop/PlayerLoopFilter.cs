using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using static UnityEngine.PlayerLoop.Initialization;
using static UnityEngine.PlayerLoop.PostLateUpdate;

namespace Assets.Scripts.LowPower
{
    public static class PlayerLoopFilter
    {
        public static List<Type> KeepInLowPower = new List<Type>(new Type[]
        {
            // Causes: Higher CPU Usage than without modifing PlayerLoop
            typeof(TimeUpdate),

#if !UNITY_ANDROID || UNITY_EDITOR
            // Causese: GfxDeviceD3D11Base::WaitForLastPresentationAndGetTimestamp() was called multiple times in a row without calling GfxDeviceD3D11Base::PresentFrame(). This may result in a deadlock.
            typeof(PresentAfterDraw),
#endif

            // Keep Profiler so it still updates even when in low power mode
            typeof(ProfilerStartFrame),
            typeof(ProfilerSynchronizeStats),
            typeof(ProfilerEndFrame),
        });

        public static List<PlayerLoopSystem> AdditionalInLowPower = new List<PlayerLoopSystem>();

        public static bool LowPowerFilter(PlayerLoopSystem pSystem) => KeepInLowPower.Contains(pSystem.type);


        public static List<Type> RemoveInHighPower = new List<Type>();

        public static List<PlayerLoopSystem> AdditionalInHighPower = new List<PlayerLoopSystem>();

        public static bool HighPowerFilter(PlayerLoopSystem pSystem) => !RemoveInHighPower.Contains(pSystem.type);


        public delegate bool FilterMethod(PlayerLoopSystem pSystem);

        public static PlayerLoopSystem GetFilteredSystem(PlayerLoopSystem pSystem, FilterMethod pFilter)
        {
            // TODO: Use profiles
            return new PlayerLoopSystem();
        }

    }
}