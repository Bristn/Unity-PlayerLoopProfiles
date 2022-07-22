#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.LowPower.Windows.Mouse
{
    public class MouseArgs
    {
        // https://docs.microsoft.com/en-us/previous-versions/windows/desktop/legacy/ms644986(v=vs.85)

        public static readonly int WH_MOUSE_LL = 14;

        public bool ButtonPressed { get; private set; }

        public bool ButtonReleased { get; private set; }

        public bool MouseWheel { get; private set; }

        public bool MouseMoved { get; private set; }

        public MouseArgs(Int32 code, IntPtr wParam, IntPtr lParam)
        {
            Interaction type = (Interaction)((int)wParam);

            if (type == Interaction.WM_LBUTTONDOWN || type == Interaction.WM_RBUTTONDOWN)
            {
                ButtonPressed = true;
            }
            else if (type == Interaction.WM_LBUTTONUP || type == Interaction.WM_RBUTTONUP)
            {
                ButtonReleased = true;
            }
            else if (type == Interaction.WM_MOUSEWHEEL || type == Interaction.WM_MOUSEHWHEEL)
            {
                MouseWheel = true;
            } 
            else if (type == Interaction.WM_MOUSEMOVE)
            {
                MouseMoved = true;
            }
        }

        private enum Interaction
        {
            WM_MOUSEMOVE = 0x0200,
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_MOUSEWHEEL = 0x020A,
            WM_MOUSEHWHEEL = 0x020E,
        }
    }
}

#endif