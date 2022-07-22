using Assets.Scripts.LowPower.PlayerLoop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LowPower
{
    public class LowPowerTimeout 
    {
        private IPlayerLoopProfile profile;
        private float timePassed;
        private bool timeoutHappened;
        private int interaction;
        private bool tempInteraction;

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

        public int Interaction 
        {
            get => interaction;
            set
            {
                int prevValue = interaction;
                interaction = value;
                if (prevValue == 0 && interaction > 0)
                {
                    if (Profile.InteractionAction != null)
                    {
                        Profile.InteractionAction.Invoke();
                    }
                }
            }
        }

        public bool TempInteraction
        {
            get => tempInteraction;
            set
            {
                if (Interaction == 0)
                {
                    tempInteraction = value;
                    if (tempInteraction && Profile.InteractionAction != null)
                    {
                        Profile.InteractionAction.Invoke();
                    }
                }
            }
        }


        public void UpdateTimeout()
        {
            if (Profile.TimeoutAction == null || timeoutHappened)
            {
                return;
            }

            if (Interaction > 0 || TempInteraction)
            {
                timePassed = 0;
                TempInteraction = false;
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