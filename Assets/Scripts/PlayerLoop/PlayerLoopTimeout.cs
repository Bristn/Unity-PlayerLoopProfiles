using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.LowLevel;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Assets.Scripts.PlayerLoop.PlayerLoopInteraction;

namespace Assets.Scripts.PlayerLoop
{
    public static class PlayerLoopTimeout 
    {
        private static float timePassed;
        private static bool timeoutHappened;
        private static bool tempInteraction;
        private static PlayerLoopProfile profile;

        public static PlayerLoopProfile Profile 
        {
            private get => profile;
            set
            {
                timePassed = 0;
                timeoutHappened = false;
                profile = value;
            }
        }

        public static PlayerLoopSystem UpdateSystem { get; private set; } = new PlayerLoopSystem()
        {
            type = typeof(PlayerLoopTimeout),
            updateDelegate = Update,
        };


        public static void Update()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            UpdateTimeout();
        }


        public static void AddInteraction(InteractionType pInteraction)
        {
            if (Profile.IgnoredInteraction.Contains(pInteraction))
            {
                return;
            }

            tempInteraction = true;
            if (Profile.InteractionAction != null)
            {
                Profile.InteractionAction.Invoke(pInteraction);
            }
        }

        public static void UpdateTimeout()
        {
            if (PlayerLoopManager.PreventProfileChange > 0 || Profile.TimeoutAction == null || timeoutHappened || tempInteraction || SelectedUIElement())
            {
                ResetTimer();
                return;
            }

            if (timePassed >= Profile.TimeoutDuration)
            {
                timeoutHappened = true;
                Profile.TimeoutAction.Invoke();
            }

            timePassed += Time.deltaTime;
        }

        private static bool SelectedUIElement()
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;
            if (selected == null)
            {
                return false;
            }


            Component[] comps = selected.GetComponents(typeof(Component));
            foreach (Component c in comps)
            {
                // Debug.Log(c + "  " + (c is PanelEventHandler));
                if (c is PanelEventHandler)
                {
                    Focusable focused = ((PanelEventHandler)c).panel.focusController.focusedElement;
                    Debug.Log("Focused: " + focused);
                    if (focused == null)
                    {
                        break;
                    }

                    foreach (var element in Profile.UiToolkitTest)
                    {
                        if (focused.GetType() == element.Key)
                        {
                            if (element.Value.Invoke(focused))
                            {
                                return true;
                            }
                        }
                    }
                    break;
                }
            }

            foreach (var element in Profile.UITest)
            {
                if (selected.TryGetComponent(element.Key, out Component component))
                {
                    if (element.Value.Invoke(component))
                    {
                        return true;
                    }
                }
            }
      
            return false;
        }


        private static void ResetTimer()
        {
            timePassed = 0;
            tempInteraction = false;
        }
    }
}