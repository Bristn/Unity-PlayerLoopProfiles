using System.Collections;
using UnityEngine;

namespace Assets.Scripts.LowPower
{
    public abstract class LowPowerImplementation
    {
        private bool hasInteraction;
        private bool tempInteraction;

        private LowPowerTimeout timeout;

        public LowPowerImplementation(LowPowerTimeout pTimeout)
        {
            timeout = pTimeout;
        }

        public bool HasInteraction
        {
            get => hasInteraction;
            protected set
            {
                hasInteraction = value;
                if (hasInteraction)
                {
                    timeout.Interaction++;
                }
                else
                {
                    timeout.Interaction--;
                }
            }
        }

        public bool TempInteraction
        {
            get => tempInteraction;
            protected set
            {
                tempInteraction = value;
                if (tempInteraction)
                {
                    timeout.TempInteraction = true;
                }
            }
        }

        public abstract void Pause();

        public abstract void Resume();

        public abstract void Clean();
    }
}