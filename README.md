# Unity Player Loop Profiles

A collection of classes and methods to create, manage and activate different PlayerLoop profiles in order to reduce resource usage, mainly CPU usage, in specific states/stages of your application lifecycle.  

## Background

After looking into making crossplatform "Non-games" with Unity, one of the most commonly stated issues/problems peoply point out are: 

- The UI building/designing with Unity is not as good as with other crossplatform environments. 
- Unity uses a lot of CPU without actually doing anything. In other words even static UIs have a relatively high constant CPU usage.

The first point is pretty subjective and I think is lessened with the "new" UI-Toolkit. The other point is what this plugin is focused on tackeling to some extend.

## Inspiration

After further searching I stumbled across [Building Non-Game Software in Unity - Jon Manning](https://www.youtube.com/watch?v=C7CZyqHGKXw) and subsequently [Unity 2018 and PlayerLoop](https://medium.com/@thebeardphantom/unity-2018-and-playerloop-5c46a12a677). This plugin combines aspects of both resources allowing to add and remove parts of the PlayerLoop more easily, when they aren't needed, hopefully leading to better performance without affecting the UX.


## Resource usage in comparison

[![Youtube video](https://img.youtube.com/vi/WG6ZjgXcVds/0.jpg)](https://www.youtube.com/watch?v=WG6ZjgXcVds)

In case the link above isn't working, try [this one](https://www.youtube.com/watch?v=WG6ZjgXcVds).

As a quick summary of the video a table comparing the different CPU usages.

|                               | Deafult PlayerLoop    | Custom PlayerLoop profiles    |
| -------------                 | -------------         | -------------                 |
| Base usage                    | ~5%                   | ~5%                           |
| Without recent interaction    | ~5%                   | ~1%                           |
| Peaks upon interaction        | ~15%                  | ~16%                          |

## Disclaimer
This isn't a one-fits-all method of improving resource usage. In a "Non-game" context it's easy to remove a big chunk of the PlayerLoop, as most of it is never used (e.g. AI, Physics, etc.). Often times it's even possible to momentarily stop rendering of new frames if the user isn't interacting with the application. In games however, specificly in gameplay, it's harder to remove much of the PlayerLoop as many of the systems are used. Of course it's still good to remove systems your not actively using in your game, but besides from menus, in which most action is paused, you wont be able to drastically reduce resource usage with this plugin.

Futhermore the documentation of the systems contained in the PlayerLoop is a bit lacking and you probably need to to a bit of trial and error when removing systems.

If your still curious and want to see example usages, see [code examples](#code-examples).

## Project Requirements

- The plugin has been tested with Unity version 2021.3.6f1 (the 2021 LTS version), but should work with any 5.x version.
- If you want to make use of the timeout mechanic, make sure `"Project Settings/Player/Other Settings/Active Input Handling"` is either set to `"Input System Packacge (new)"` or `"Both"`, as the plugin internally uses the new Input System.


## Installation

- As you migh have noticed this repository contains a complete Unity project. To use the plugin you only need the `Plugins` folder. All other files are related to the examlpe scene.
- [Download the repository](https://github.com/Bristn/Unity-PlayerLoopProfiles/archive/refs/heads/master.zip)  and copy the `Assets/Plugins` folder into your Unity's `Assets` directory


## Code examples

The following are some smaller code examples. For a basic overview I suggest checking out the scripts in [Assets/Example/Scripts/](https://github.com/Bristn/Unity-PlayerLoopProfiles/tree/master/Assets/Example/Scripts).
For an even more complex example checkout the [simple pinger app](https://github.com/Bristn/Unity-SimplePinger) I made to demonstrate the plugin.

### Action maps

If you want to use the interaction callback system you need to add the actions maps you want to react to. In the example it's using the default UI action map. The interaction callbacks get invoked with the action name. Because of this I like to create an enum and a string array with the action names.

A small example, not all UI actions are included check [Assets/Example/Scripts/Example.cs](https://github.com/Bristn/Unity-PlayerLoopProfiles/blob/master/Assets/Example/Scripts/Example.cs) for the full code.

    using PlayerLoopProfiles;

    ...

    [SerializeField] private InputActionAsset InputAsset;

    public enum InteractionType
    {
        NAVIGATE,
        POINT,
        RIGHT_CLICK,
    }

    public static List<string> actionNames = new string[]
    {
        "Navigate",
        "Point",
        "RightClick"
    }.ToList();

    void Start()
    {
        PlayerLoopInteraction.AddActionMap(InputAsset.FindActionMap("UI"));
    }

With the `InputAsset` being a copy of the default UI action asset. 

### Create profiles

To create a new profile use the PlayerLoopProfileBuilder. There are a few methods to further customize the profile. As before the following code is a small snippet. See the classes [Assets/Example/Scripts/ProfileNormal.cs](https://github.com/Bristn/Unity-PlayerLoopProfiles/blob/master/Assets/Example/Scripts/ProfileNormal.cs) and [Assets/Example/Scripts/ProfileIdle.cs](https://github.com/Bristn/Unity-PlayerLoopProfiles/blob/master/Assets/Example/Scripts/ProfileIdle.cs).

    public static PlayerLoopProfile GetProfileNormal()
    {
        PlayerLoopProfile profile = new PlayerLoopProfileBuilder()
            .TimeoutCallback(Timeout)
            .TimeoutDuration(0.1f)
            .Build();
        return profile;
    }

    private static void Timeout()
    {
        Debug.Log("There hasn't been any user interaction for 0.1 seconds");
    }

The above profile keeps all systems of the PlayerLoop, but invokes the TimeoutCallback after a the user hasn't interacted with the app for a longer period time.

---

    public static PlayerLoopProfile GetProfileIdle()
    {
        List<Type> filter = new List<Type>(new Type[]
        {
            typeof(TimeUpdate), // Causes high CPU usage when removed

    #if !UNITY_ANDROID || UNITY_EDITOR 
            typeof(PostLateUpdate.PresentAfterDraw), // Some oddities with this system
    #endif
        });

        PlayerLoopProfile profile = new PlayerLoopProfileBuilder()
            .FilterSystems(filter)
            .FilterType(FilterType.KEEP)
            .InteractionCallback(Interaction)
            .KeepInteractionSystems(true)
            .Build();

        return profile;
    }

    private static void Interaction(string pType)
    {
        Debug.Log("There has been a interaction whilst the idle profile is active");
    }

This profile removes every system expect ones which are responsible for the Input System. If there is any interaction whilst the profile is active the `Interaction` method is called with one of the `actionNames` we defined earlier.

### Add the profile

Before you can activate a profile you need to register it. This way you can create a profile once, register it with a custom enum or a integer key, then activate it using the key. For this example I chose to use a custom enum.

    public enum Profile
    {
        IDLE,
        NORMAL,
    }

    private static void RegisterProfiles()
    {
        PlayerLoopManager.AddProfile(Profile.IDLE, GetProfileIdle());
        PlayerLoopManager.AddProfile(Profile.NORMAL, GetProfileNormal());
    }

### Set the active profile

To activate a profile you need to call the following method with the key for the desired profile.

    private static void ActivateIdle()
    {
        PlayerLoopManager.SetActiveProfile(Profile.IDLE);
    }

In the exmaple provided the `Timeout` and `Interaction` methods are altered to switch to the respective profiles.

## Feedback / Suggestions

If you have any feedback or suggestions feel free to share them with me, any feedback is welcome.
