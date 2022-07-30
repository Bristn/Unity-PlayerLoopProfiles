using System.Collections;
using UnityEngine;

namespace Assets.Scripts.LowPower
{
    public abstract class LowPowerImplementation
    {
        public enum InteractionType
        {
            // Desktop
            DSEKTOP_MOUSE_BUTTON,
            DSEKTOP_MOUSE_MOVE,
            DSEKTOP_MOUSE_SCROLL,
            DSEKTOP_KEYBOARD_BUTTON,

            // Mobile
            MOBILE_TOUCH,
        }


        private LowPowerTimeout timeout;

        public LowPowerImplementation(LowPowerTimeout pTimeout)
        {
            timeout = pTimeout;
        }

        protected void AddInteraction(InteractionType pType) => timeout.AddInteraction(pType);

        protected void RemoveInteraction(InteractionType pType) => timeout.RemoveInteraction(pType);

        public abstract void Pause();

        public abstract void Resume();

        public abstract void Clean();
    }
}