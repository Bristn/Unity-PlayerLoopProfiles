using PlayerLoopProfiles;
using System;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;
using static Example;
using static PlayerLoopProfiles.PlayerLoopInteraction;
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

            // input Test
            typeof(Initialization.SynchronizeInputs),
            typeof(EarlyUpdate.UpdateInputManager),
            typeof(EarlyUpdate.ProcessRemoteInput),
            typeof(FixedUpdate.NewInputFixedUpdate),
            typeof(PreUpdate.CheckTexFieldInput),
            typeof(PreUpdate.NewInputUpdate),
            typeof(PostLateUpdate.InputEndFrame),
            typeof(PostLateUpdate.ResetInputAxis),
        });

        PlayerLoopProfile profile = new PlayerLoopProfileBuilder()
            .FilterSystems(filter)
            .FilterType(FilterType.KEEP)
            .InteractionCallback(Interaction)
            .IgnoreInteraction(InteractionType.POINT)
            .Build();
        return profile;
    }

    private static void Interaction(InteractionType pType)
    {
        if (pType == InteractionType.SCROLL_WHEEL)
        {
            return;
        }

        PlayerLoopManager.SetActiveProfile(Profile.NORMAL);
    }
}
