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

        /// <summary>
        /// All actions of the registered maps are processed.
        /// There can be multiple registered maps. 
        /// This method registers a new action map.
        /// </summary>
        public static void AddActionMap(InputActionMap pMap)
        {
            actionsMaps.Add(pMap);
            UpdateActionLookup();
        }

        /// <summary>
        /// All actions of the registered maps are processed.
        /// There can be multiple registered maps. 
        /// This method removes a action map.
        /// </summary>
        public static void RemoveActionMap(InputActionMap pMap)
        {
            actionsMaps.Remove(pMap);
            UpdateActionLookup();
        }

        /// <summary>
        /// All actions of every registered map is processed.
        /// There can be multiple registered maps. 
        /// This method removes all action maps.
        /// </summary>
        public static void ClearActionMaps()
        {
            actionsMaps.Clear();
            UpdateActionLookup();
        }

        /// <summary>
        /// Update the internal action lookup to reflect the registered maps.
        /// </summary>
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
