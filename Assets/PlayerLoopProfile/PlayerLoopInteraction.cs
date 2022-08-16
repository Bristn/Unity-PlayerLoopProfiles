using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace PlayerLoopProfiles
{
    public static class PlayerLoopInteraction
    {
        private static List<InputActionMap> actionsMaps = new List<InputActionMap>();
        private static Dictionary<string, string> actionLookup = new Dictionary<string, string>();

        public static void AddActionMap(InputActionMap pMap)
        {
            actionsMaps.Add(pMap);
            UpdateActionLookup();
        }

        public static void RemoveActionMap(InputActionMap pMap)
        {
            actionsMaps.Remove(pMap);
            UpdateActionLookup();
        }

        public static void ClearActionMaps()
        {
            actionsMaps.Clear();
            UpdateActionLookup();
        }
        
        private static void UpdateActionLookup()
        {
            actionLookup.Clear();
            InputSystem.onEvent -= EventListener;
            foreach (InputActionMap map in actionsMaps)
            {
                foreach (InputAction action in map.actions)
                {
                    foreach (InputControl control in action.controls)
                    {
                        actionLookup.TryAdd(control.path, action.name);
                    }
                }
            }
            InputSystem.onEvent += EventListener;
        }

        private static void EventListener(InputEventPtr eventPtr, InputDevice device)
        {
            if (!Application.isPlaying)
            {
                InputSystem.onEvent -= EventListener;
                return;
            }

            if (actionLookup.Count == 0 || !eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
            {
                return;
            }

            foreach (InputControl control in eventPtr.EnumerateChangedControls(device))
            {
                string info = null;
                string name = control.path;
                while (name.Count(x => x == '/') > 1)
                {
                    info = actionLookup.GetValueOrDefault(name, string.Empty);
                    name = name.Substring(0, name.LastIndexOf("/"));
                    OnInteraction(info);
                }
            }
        }

        private static void OnInteraction(string pType)
        {
            if (pType == string.Empty)
            {
                return;
            }

            PlayerLoopTimeout.AddInteraction(pType);
        }
    }
}
