using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

namespace Assets.Scripts.LowPower
{
    public class LowPowerDesktop : LowPowerImplementation
    {
        public LowPowerDesktop(LowPowerTimeout pTimeout) : base(pTimeout)
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



        private bool keyDown;
        private Vector3 mousePos;


        public void UpdateInput()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
               return;
#endif

            if (!keyDown && Input.anyKey)
            {
                keyDown = true;
                AddInteraction(InteractionType.DSEKTOP_KEYBOARD_BUTTON);
            } 
            else if (keyDown && !Input.anyKey)
            {
                keyDown = false;
                RemoveInteraction(InteractionType.DSEKTOP_KEYBOARD_BUTTON);
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                AddInteraction(InteractionType.DSEKTOP_MOUSE_SCROLL);
            }

            if (!mousePos.Equals(Input.mousePosition))
            {
                AddInteraction(InteractionType.DSEKTOP_MOUSE_MOVE);
            }
            mousePos = Input.mousePosition;

        }
    }
}