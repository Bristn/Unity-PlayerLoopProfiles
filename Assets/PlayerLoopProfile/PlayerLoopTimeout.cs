using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.LowLevel;
using UnityEngine.UIElements;

namespace PlayerLoopProfiles
{
    public static class PlayerLoopTimeout 
    {
        private static float timePassed;
        private static bool timeoutHappened;
        private static bool tempInteraction;
        private static PlayerLoopProfile profile;

        /// <summary>
        /// The active profile which is needed to invoke it's callbacks and get various other variables.
        /// </summary>
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

        /// <summary>
        /// Custom PlayerLoopSystem which gets added to the PlayerLoop with all profiles.
        /// </summary>
        public static PlayerLoopSystem UpdateSystem { get; private set; } = new PlayerLoopSystem()
        {
            type = typeof(PlayerLoopTimeout),
            updateDelegate = Update,
        };

        /// <summary>
        /// "Adds" a new interaction. 
        /// This will invoke the current profiles 'InteractionCallback' with the passed string parameter as interaction.
        /// Aside from that the current timer gets reset.
        /// </summary>
        public static void AddInteraction(string pInteraction)
        {
            if (!Profile.IgnoredInteraction.Contains(pInteraction))
            {
                tempInteraction = true;
            }
            
            if (Profile.InteractionAction != null)
            {
                Profile.InteractionAction.Invoke(pInteraction);
            }
        }

        /// <summary>
        /// A custom update method which gets added to the playerloop on any profile. 
        /// Used to ensure it gets called even if all other systems get removed.
        /// </summary>
        public static void Update()
        {
            if (!Application.isPlaying)
            {
                return;
            }

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

            if (CheckToolkitElement(selected)) {
                return true;
            }

            foreach (var element in Profile.UiEvaluation)
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

        private static bool CheckToolkitElement(GameObject pSelected)
        {
            foreach (Component compoenent in pSelected.GetComponents(typeof(Component)))
            {
                if (compoenent is not PanelEventHandler)
                {
                    continue;
                }

                Focusable focused = ((PanelEventHandler)compoenent).panel.focusController.focusedElement;
                if (focused == null)
                {
                    continue;
                }

                foreach (var element in Profile.UiToolkitEvaluation)
                {
                    if (focused.GetType() == element.Key)
                    {
                        if (element.Value.Invoke(focused))
                        {
                            return true;
                        }
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