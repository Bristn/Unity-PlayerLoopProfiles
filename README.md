# Unity Player Loop Profiles

A collection of classes and methods to create, manage and activate different PlayerLoop profiles in order to reduce resource usage, mainly CPU usage, in specific states/stages of your application lifecycle.  

## Background

After looking into making crossplatform "Non-games" with unity, one of the most commonly stated issues/problems peoply point out are: 

- The UI building/designing with unity is not as good as with other crossplatform environments. 
- Unity uses a lot of CPU without actually doing anything. In other words even static UIs have a relatively high constant CPU usage.

The first point is pretty subjective and I think is lessened with the "new" UI-Toolkit. The other point is what this plugin is focused on tackeling to some extend.

### Inspiration

After further searching I stumbled across [Building Non-Game Software in Unity - Jon Manning](https://www.youtube.com/watch?v=C7CZyqHGKXw) and subsequently [Unity 2018 and PlayerLoop](https://medium.com/@thebeardphantom/unity-2018-and-playerloop-5c46a12a677). This plugin combines aspects of both resources allowing to add and remove parts of the PlayerLoop more easily, when they aren't needed, hopefully leading to better performance without affecting the UX.


## Resource usage in comparison

- // todo

## Project Requirements

- If you want to make use of the timeout mechanic, make sure `"Project Settings/Player/Other Settings/Active Input Handling"` is either set to `"Input System Packacge (new)"` or `"Both"`, as the plugin internally uses the new Input System.


## Installation

Before installing a quick disclaimer: // todo

- Simply download the repository and copy the *Plugins* folder into your Unity's *Assets* folder


## Example Project

