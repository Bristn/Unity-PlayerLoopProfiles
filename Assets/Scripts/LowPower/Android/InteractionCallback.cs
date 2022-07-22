#if UNITY_ANDROID && !UNITY_EDITOR

using Assets.Scripts.LowPower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LowPower.Android
{ 
    public class InteractionCallback : AndroidJavaProxy
    {
        private LowPowerAndroid implementation;

        public InteractionCallback(LowPowerAndroid pImplementation) : base("com.bristn.lowpowerplugin.InteractionCallback")
        {
            implementation = pImplementation;
        }

        public void onTouchEvent(bool pPressed) {
            implementation.TouchInteraction(pPressed);
        }
    }
}

#endif
