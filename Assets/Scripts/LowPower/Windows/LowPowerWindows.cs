#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using Assets.Scripts.LowPower.Windows.Keyboard;
using Assets.Scripts.LowPower.Windows.Mouse;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LowPower.Windows
{
    public class LowPowerWindows : LowPowerImplementation
    {
        private KeyboardInput keyboard;
        private MouseInput mouse;

        public LowPowerWindows(LowPowerTimeout pTimeout) : base(pTimeout)
        {
            keyboard = new KeyboardInput(this);
            mouse = new MouseInput(this);
        }

        public override void Pause()
        {
            keyboard.UnHook();
            mouse.UnHook();
        }

        public override void Resume()
        {
            keyboard.Hook();
            mouse.Hook();
        }

        public override void Clean()
        {
            keyboard.Dispose();
            mouse.Dispose();

            keyboard = null;
            mouse = null;
        }

        private HashSet<byte> pressedKeyboardButton = new HashSet<byte>();

        public void KeyboardInteraction(object pSender, KeyboardArgs pArguments)
        {
            if (!pArguments.ButtonPressed)
            {
                pressedKeyboardButton.Remove(pArguments.ButtonCode);
                if (pressedKeyboardButton.Count == 0)
                {
                    RemoveInteraction(InteractionType.DSEKTOP_KEYBOARD_BUTTON);
                }
            }
            else if (!pressedKeyboardButton.Contains(pArguments.ButtonCode))
            {
                if (pressedKeyboardButton.Count == 0)
                {
                    AddInteraction(InteractionType.DSEKTOP_KEYBOARD_BUTTON);
                }
                pressedKeyboardButton.Add(pArguments.ButtonCode);
            }
        }

        private byte pressedMouseButton = 0;

        public void MouseInteraction(object pSender, MouseArgs pArguments)
        {
            if (pArguments.MouseMoved)
            {
                AddInteraction(InteractionType.DSEKTOP_MOUSE_MOVE);
                return;
            }

            if (pArguments.MouseWheel)
            {
                AddInteraction(InteractionType.DSEKTOP_MOUSE_SCROLL);
                return;
            }

            if (pArguments.ButtonPressed)
            {
                if (pressedMouseButton == 0)
                {
                    AddInteraction(InteractionType.DSEKTOP_MOUSE_BUTTON);
                }
                pressedMouseButton++;
            }
            else if (pArguments.ButtonReleased)
            {
                pressedMouseButton--;
                if (pressedMouseButton == 0)
                {
                    RemoveInteraction(InteractionType.DSEKTOP_MOUSE_BUTTON);
                }
            }
        }
    }
}
#endif