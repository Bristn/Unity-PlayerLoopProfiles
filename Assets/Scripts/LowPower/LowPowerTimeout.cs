using Assets.Scripts.LowPower.PlayerLoop;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static LowPowerInteraction;

namespace Assets.Scripts.LowPower
{
    public class LowPowerTimeout 
    {
        private float timePassed;
        private bool timeoutHappened;
        private bool tempInteraction;
        private IPlayerLoopProfile profile;

        public IPlayerLoopProfile Profile 
        {
            private get => profile;
            set
            {
                timePassed = 0;
                timeoutHappened = false;
                profile = value;
            }
        }

        public void AddInteraction(ActionType pInteraction)
        {
            // If the interaction is ignored simply return
            if (Profile.IgnoredInteraction.Contains(pInteraction))
            {
                return;
            }

            // Check if this is a temporary interaction (Temporary = Only one state: Scrolled, Moved)
            tempInteraction = true;
            if (Profile.InteractionAction != null)
            {
                Profile.InteractionAction.Invoke(pInteraction);
            }
        }

        public void UpdateTimeout()
        {
            if (LowPowerManager.Instance.playerLoopManager.PreventProfileChange > 0 || Profile.TimeoutAction == null || timeoutHappened || tempInteraction || SelectedUIElement())
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

        private bool SelectedUIElement()
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


        private void ResetTimer()
        {
            timePassed = 0;
            tempInteraction = false;
        }
    }
}