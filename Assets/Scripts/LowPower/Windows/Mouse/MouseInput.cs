#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using Assets.Scripts.LowPower.Windows;
using InputTrackingExample;
using System;

namespace Assets.Scripts.LowPower.Windows.Mouse
{
    public class MouseInput : IDisposable
    {
        private LowPowerWindows implementation;
        private WindowsHookHelper.HookDelegate mouseDelegate;
        private IntPtr mouseHandle = IntPtr.Zero;
        private bool disposed;

        public MouseInput(LowPowerWindows pImplementation)
        {
            implementation = pImplementation;
            mouseDelegate = MouseHookDelegate;
            Hook();
        }

        private IntPtr MouseHookDelegate(Int32 Code, IntPtr wParam, IntPtr lParam)
        {
            if (Code >= 0) 
            {
                implementation.MouseInteraction(this, new MouseArgs(Code, wParam, lParam));
            }
            
            return WindowsHookHelper.CallNextHookEx(mouseHandle, Code, wParam, lParam);
        }

        public void Hook()
        {
            if (mouseHandle == IntPtr.Zero)
            {
                mouseHandle = WindowsHookHelper.SetWindowsHookEx(MouseArgs.WH_MOUSE_LL, mouseDelegate, IntPtr.Zero, 0);
            }
        }

        public void UnHook()
        {
            if (mouseHandle != IntPtr.Zero)
            {
                WindowsHookHelper.UnhookWindowsHookEx(mouseHandle);
                mouseHandle = IntPtr.Zero;
            }
        }

        ~MouseInput() => Dispose();

        public void Dispose()
        {
            if (!disposed)
            {
                UnHook();
                disposed = true;
            }
        }

    }
}

#endif
