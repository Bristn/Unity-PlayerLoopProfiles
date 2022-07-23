#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    using Assets.Scripts.LowPower.Windows;
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
    using Assets.Scripts.LowPower.Android;
#endif

using Assets.Scripts.LowPower.PlayerLoop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LowPower
{
    public class LowPowerManager
    {
        private static LowPowerManager instance;

        public static LowPowerManager Instance
        {
            get
            {
                if (instance == null || instance.Equals(null))
                {
                    instance = new LowPowerManager();
                }
                return instance;
            }
        }


        private List<LowPowerImplementation> implementations = new List<LowPowerImplementation>();

        public LowPowerMono mono { get; private set; }


        public LowPowerTimeout timeout { get; private set; }

        public LowPowerDispatcher dispatcher { get; private set; }


        public PlayerLoopManager playerLoopManager { get; private set; }

        public LowPowerManager()
        {
            GameObject gameObject = new GameObject("LowPower");

            // Create sub systems
            timeout = new LowPowerTimeout();
            dispatcher = new LowPowerDispatcher(timeout);

            playerLoopManager = new PlayerLoopManager(dispatcher, timeout);


            // Add all implementations
            if (Application.isPlaying)
            {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                implementations.Add(new LowPowerWindows(timeout));
#endif
#if UNITY_ANDROID && !UNITY_EDITOR 
                implementations.Add(new LowPowerAndroid(timeout));
#endif
            }

            // Add new component which reacts to pause, focus etc. events
            gameObject.AddComponent<LowPowerMono>();
            gameObject.GetComponent<LowPowerMono>().Implementations = implementations;
        }
    }
}