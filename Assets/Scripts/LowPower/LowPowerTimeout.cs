using Assets.Scripts.LowPower.PlayerLoop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.LowPower.LowPowerImplementation;

namespace Assets.Scripts.LowPower
{
    public class LowPowerTimeout 
    {
        private float timePassed;
        private bool timeoutHappened;
        private bool tempInteraction;
        private int interactionCount;
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

        public void AddInteraction(InteractionType pInteraction)
        {
            int prevCount = interactionCount;
            if (pInteraction.IsTemporary())
            {
                if (prevCount == 0)
                {
                    tempInteraction = true;
                    if (Profile.InteractionAction != null)
                    {
                        Profile.InteractionAction.Invoke(pInteraction);
                    }
                }
                return;
            }

            interactionCount++;

            int curCount = interactionCount;
            if (prevCount == 0 && curCount > 0)
            {
                if (Profile.InteractionAction != null)
                {
                    Profile.InteractionAction.Invoke(pInteraction);
                }
            }
        }

        public void RemoveInteraction(InteractionType pInteraction) => interactionCount--;

        public void UpdateTimeout()
        {
            if (Profile.TimeoutAction == null || timeoutHappened)
            {
                return;
            }

            if (interactionCount > 0 || tempInteraction)
            {
                timePassed = 0;
                tempInteraction = false;
            }
            else
            {
                if (timePassed >= Profile.TimeoutDuration)
                {
                    timeoutHappened = true;
                    Profile.TimeoutAction.Invoke();
                }
                timePassed += Time.deltaTime;
            }
        }
    }
}