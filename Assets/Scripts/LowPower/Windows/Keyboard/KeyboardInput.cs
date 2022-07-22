#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using Assets.Scripts.LowPower.Windows.Keyboard;
using InputTrackingExample;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Scripts.LowPower.Windows.Keyboard
{
    public class KeyboardInput : IDisposable
    {
        private LowPowerWindows implementation;
        private WindowsHookHelper.HookDelegate keyBoardDelegate;
        private IntPtr keyBoardHandle;
        private bool disposed;

        public KeyboardInput(LowPowerWindows pImplementation)
        {
            implementation = pImplementation;
            keyBoardDelegate = KeyboardHookDelegate;
            Hook();
        }

        private IntPtr KeyboardHookDelegate(Int32 Code, IntPtr wParam, IntPtr lParam)
        {
            if (Code >= 0)
            {
                implementation.KeyboardInteraction(this, new KeyboardArgs(Code, wParam, lParam));
            }

            return WindowsHookHelper.CallNextHookEx(keyBoardHandle, Code, wParam, lParam);
        }

        public void Hook()
        {
            if (keyBoardHandle == IntPtr.Zero)
            {
                keyBoardHandle = WindowsHookHelper.SetWindowsHookEx(KeyboardArgs.WH_KEYBOARD_LL, keyBoardDelegate, IntPtr.Zero, 0);
            }
        }

        public void UnHook()
        {
            if (keyBoardHandle != IntPtr.Zero)
            {
                WindowsHookHelper.UnhookWindowsHookEx(keyBoardHandle);
                keyBoardHandle = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                UnHook();
                disposed = true;
            }
        }

        ~KeyboardInput() => Dispose();
    }
}

#endif