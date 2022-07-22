#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Scripts.LowPower.Windows.Keyboard
{
    public class KeyboardArgs 
    {
        // https://docs.microsoft.com/en-us/previous-versions/windows/desktop/legacy/ms644985(v=vs.85)
        // https://docs.microsoft.com/de-de/windows/win32/api/winuser/ns-winuser-kbdllhookstruct?redirectedfrom=MSDN

        public static readonly int WH_KEYBOARD_LL = 13;

        public bool ButtonPressed { get; private set; }

        public byte ButtonCode { get; private set; }


        private struct KeyInfo
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        public KeyboardArgs(Int32 code, IntPtr wParam, IntPtr lParam)
        {
            KeyInfo keyInfo = (KeyInfo)Marshal.PtrToStructure(lParam, typeof(KeyInfo));
            ButtonCode = (byte)keyInfo.vkCode;
            ButtonPressed = (keyInfo.flags & 128) == 0;
        }

        public bool MouseMoved { get; internal set; }
    }
}

#endif