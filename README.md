# Unity Player Loop Profiles

A collection of classes and methods to create, manage and activate different PlayerLoop profiles in order to reduce resource usage, mainly CPU usage, in specific states/stages of your application lifecycle.  

## Background

After looking into making crossplatform "Non-games" with unity, one of the most commonly stated issues/problems peoply point out are: 

- The UI building/designing with unity is not as good as with other crossplatform environments. 
- Unity uses a lot of CPU without actually doing anything. In other words even static UIs have a relatively high constant CPU usage.

The first point is pretty subjective and I think is lessened with the "new" UI-Toolkit. The other point is what this plugin is focused on tackeling to some extend.

## Inspiration

After further searching I stumbled across [Building Non-Game Software in Unity - Jon Manning](https://www.youtube.com/watch?v=C7CZyqHGKXw) and subsequently [Unity 2018 and PlayerLoop](https://medium.com/@thebeardphantom/unity-2018-and-playerloop-5c46a12a677). This plugin combines aspects of both resources allowing to add and remove parts of the PlayerLoop more easily, when they aren't needed, hopefully leading to better performance without affecting the UX.


## Resource usage in comparison

- // todo

----

## Disclaimer
This isn't a one-fits-all method of improving resource usage. In a "Non-game" context it's easy to remove a big chunk of the PlayerLoop, as most of it is never used (e.g. AI, Physics, etc.). Often times it's even possible to momentarily stop rendering of new frames if the user isn't interacting with the application. In games however, specificly in gameplay, it's harder to remove much of the PlayerLoop as many of the systems are used. Of course it's still good to remove systems your not actively using in your game, but besides from menus, in which most action is paused, you wont be able to drastically reduce resource usage with this plugin.

Futhermore the documentation of the systems contained in the PlayerLoop is a bit lacking and you probably need to to a bit of trial and error when removing systems.

If your still curious and want to see example usages, checkout the [Example Project](#example-project) or a slightly more complex [App using this plugin](https://www.google.com/).

## Project Requirements

- The plugin has been tested with Unity version 2021.3.6f1 (the 2021 LTS version), but should work with any 5.x version.
- If you want to make use of the timeout mechanic, make sure `"Project Settings/Player/Other Settings/Active Input Handling"` is either set to `"Input System Packacge (new)"` or `"Both"`, as the plugin internally uses the new Input System.


## Installation

- [Download the repository](https://github.com/Bristn/Unity-PlayerLoopProfiles/archive/refs/heads/master.zip)  and copy the `Assets/Plugins` folder into your Unity's `Assets` directory


## Code examples

