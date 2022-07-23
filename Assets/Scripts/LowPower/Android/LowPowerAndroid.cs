#if UNITY_ANDROID && !UNITY_EDITOR

using System.Collections;
using UnityEngine;

namespace Assets.Scripts.LowPower.Android
{
    public class LowPowerAndroid : LowPowerImplementation
    {
        private AndroidJavaClass unityClass;
        private AndroidJavaObject unityActivity;
        private AndroidJavaObject pluginInstance;
        private byte activeTouchCount = 0;

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

        public override void Pause() // TODO: When app times out: Handler sending message to a Handler on a dead thread 
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
                if (activeTouchCount == 0)
                {
                    AddInteraction(InteractionType.ANDROID_TOUCH);
                }
                activeTouchCount++;
            }
            else
            {
                activeTouchCount--;
                if (activeTouchCount == 0)
                {
                    RemoveInteraction(InteractionType.ANDROID_TOUCH);
                }
            }
        }
    }
}

#endif