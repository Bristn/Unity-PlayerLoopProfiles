#if UNITY_ANDROID && !UNITY_EDITOR

using System.Collections;
using UnityEngine;

namespace Assets.Scripts.LowPower.Android
{
    public class LowPowerAndroid : ILowPowerImplementation
    {
        private AndroidJavaClass unityClass;
        private AndroidJavaObject unityActivity;
        private AndroidJavaObject pluginInstance;

        public LowPowerAndroid(LowPowerTimeout pTimeout) : base(pTimeout)
        {
            unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            pluginInstance = new AndroidJavaObject("com.bristn.lowpowerplugin.PluginInstance");

            if (pluginInstance == null)
            {
                Debug.Log("Plguin instance error");
            }

            pluginInstance.CallStatic("setup", new InteractionCallback(this));
        }

        public override void Pause()
        {
            pluginInstance.CallStatic("pause");
        }

        public override void Resume()
        {
            pluginInstance.CallStatic("resume");
        }

        public override void Clean()
        {
            pluginInstance.CallStatic("clean");
        }

        public void TouchInteraction(bool pPressed)
        {
            if (pPressed)
            {
                activeButtonCount++;
            }
            else
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