using System.Collections;
using UnityEngine;

namespace Assets.Scripts.LowPower
{
    public class LowPowerMobile : LowPowerImplementation
    {
        public LowPowerMobile(LowPowerTimeout pTimeout) : base(pTimeout)
        {

        }

        public override void Clean()
        {
        }

        public override void Pause()
        {
        }

        public override void Resume()
        {
        }


        private bool touch;

        public void UpdateInput()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            return;
#endif
            /*
            if (!touch && Input.touchCount > 0)
            {
                touch = true;
                AddInteraction(InteractionType.MOBILE_TOUCH);
            }
            else if (touch && Input.touchCount == 0)
            {
                touch = false;
                RemoveInteraction(InteractionType.MOBILE_TOUCH);
            }
            */
        }
    }
}