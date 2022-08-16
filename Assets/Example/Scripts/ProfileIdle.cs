using PlayerLoopProfiles;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static Example;
using static PlayerLoopProfiles.PlayerLoopProfile;

public static class ProfileIdle
{
    public static PlayerLoopProfile GetProfile()
    {
        List<Type> filter = new List<Type>(new Type[]
        {
            // Keep, as causes higher CPU usage when removed
            typeof(TimeUpdate),

#if !UNITY_ANDROID || UNITY_EDITOR
            // Causese: GfxDeviceD3D11Base::WaitForLastPresentationAndGetTimestamp() was called multiple times in a row without calling GfxDeviceD3D11Base::PresentFrame(). This may result in a deadlock.
            typeof(PostLateUpdate.PresentAfterDraw),
#endif

            // Keep Profiler for debugging
            typeof(Initialization.ProfilerStartFrame),
            typeof(PostLateUpdate.ProfilerSynchronizeStats),
            typeof(PostLateUpdate.ProfilerEndFrame),
        });

        PlayerLoopProfile profile = new PlayerLoopProfileBuilder()
            .FilterSystems(filter)
            .FilterType(FilterType.KEEP)
            .InteractionCallback(Interaction)
            .IgnoredInteractions(actionNames[(int)InteractionType.POINT])
            .Build();

        profile.PrintProfile();
        return profile;
    }

    private static void Interaction(string pType)
    {
        if (pType == actionNames[(int)InteractionType.SCROLL_WHEEL])
        {
            return;
        }

        PlayerLoopManager.SetActiveProfile(Profile.NORMAL);
    }
}
