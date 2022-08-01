using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.LowLevel;
using UnityEngine.UI;
using static Assets.Scripts.PlayerLoop.LowPowerInteraction;

namespace Assets.Scripts.PlayerLoop
{
    public static class LowPowerTimeout 
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
            type = typeof(LowPowerTimeout),
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


        public static void AddInteraction(ActionType pInteraction)
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