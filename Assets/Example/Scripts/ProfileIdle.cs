using PlayerLoopProfiles;
using System;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;
using static Example;
using static PlayerLoopProfiles.PlayerLoopProfile;

// A simple class to capsule the idle profile completely.
public static class ProfileIdle
{
    // Method gets called in Example.cs to register the profile.
    public static PlayerLoopProfile GetProfile()
    {
        // Create a filter list.
        // The elements of this list get applied to the 'baseSystem', in this case the default PlayerLoop, with the 'FilterType'.
        //      - To see the full list of systems checkout the file 'PlayerLoopListFull.txt'.
        // For the idle profile the most of the PlayerLoop gets discarded (or in this case only a few systems are kept).
        List<Type> filter = new List<Type>(new Type[]
        {
            // Keep, as causes higher CPU usage when removed.
            typeof(TimeUpdate),

#if !UNITY_ANDROID || UNITY_EDITOR
            // Causese: WaitForLastPresentationAndGetTimestamp() was called multiple times in a row without calling PresentFrame().
            typeof(PostLateUpdate.PresentAfterDraw),
#endif

            // Keep Profiler for debugging
            typeof(Initialization.ProfilerStartFrame),
            typeof(PostLateUpdate.ProfilerSynchronizeStats),
            typeof(PostLateUpdate.ProfilerEndFrame),
        });

        PlayerLoopProfile profile = new PlayerLoopProfileBuilder()

            // Set the filter list to the one create before.
            .FilterSystems(filter)

            // As stated before this will only keep the elements in the filter list from above.
            .FilterType(FilterType.KEEP)

            // This method ensures input related systems are kept no matter the filter above.
            // The default value is true, so this call isn't necessary.
            // There is a special method for this, because I thoguht user interactions shouldn't get discard most of the time.
            .KeepInteractionSystems(true)

            // Set the interaction callback action.
            // This gets invoked when there has been any interaction of any one of the registered action maps.
            .InteractionCallback(Interaction)

            // Actions in this collection are not considered at all. 
            // They are treated as if they aren't in the action map, but this way they don't actually have to be removed from the action map.
            // Ignore any mouse movement.
            .IgnoredInteractions(actionNames[(int)InteractionType.POINT])

            // Finally build the profile 
            .Build();

        return profile;
    }

    // This gets called when this profile is active and there has been any interaction from any one of the registered action maps. 
    // The passed string parameter is the name of the respective action, which occurred.
    private static void Interaction(string pType)
    {
        // As a further example of filtering interaction types, mouse scrolls wont switch the active profile
        if (pType == actionNames[(int)InteractionType.SCROLL_WHEEL])
        {
            return;
        }

        // Otherwise activate the normal profile, as there has been a valid interaction.
        PlayerLoopManager.SetActiveProfile(Profile.NORMAL);
    }
}
