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
                activeButtonCount--;
            }
            else if (!pressedKeyboardButton.Contains(pArguments.ButtonCode))
            {
                pressedKeyboardButton.Add(pArguments.ButtonCode);
                activeButtonCount++;
            }
        }

        public void MouseInteraction(object pSender, MouseArgs pArguments)
        {
            if (pArguments.MouseMoved)
            {
                TempInteraction = true; // TODO: ??
                return;
            }

            if (pArguments.ButtonPressed)
            {
                activeButtonCount++;
            }
            else if (pArguments.ButtonReleased)
            {
                activeButtonCount--;
            }
        }


        private int _activeButtonCount = 0;

        private int activeButtonCount
        {
            get
            {
                return _activeButtonCount;
            }
            set
            {
                int prevValue = _activeButtonCount;
                _activeButtonCount = value;

                if (prevValue == 0 && _activeButtonCount > 0)
                {
                    HasInteraction = true;
                }
                else if (prevValue > 0 && _activeButtonCount == 0)
                {
                    HasInteraction = false;
                }
            }
        }
    }
}
#endif