﻿using Assets.Scripts.LowPower;
using Assets.Scripts.LowPower.PlayerLoop;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static Assets.Scripts.LowPower.PlayerLoop.IPlayerLoopProfile;
using static UnityEngine.PlayerLoop.Initialization;
using static UnityEngine.PlayerLoop.PostLateUpdate;

namespace Assets.Scripts
{
    public class Example : MonoBehaviour
    {
        [SerializeField] private GameObject Cube;
        [SerializeField] private TextMeshProUGUI Frames;

        public enum Profile
        {
            IDLE,
            NORMAL,
        }

        // Use this for initialization
        void Start()
        {
            // Limit framerate
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;

            // Initialize profiles
            List<Type> idleFilter = new List<Type>(new Type[]
            {
                // Keep, as causes higher CPU usage when removed
                typeof(TimeUpdate),

#if !UNITY_ANDROID || UNITY_EDITOR
                // Causese: GfxDeviceD3D11Base::WaitForLastPresentationAndGetTimestamp() was called multiple times in a row without calling GfxDeviceD3D11Base::PresentFrame(). This may result in a deadlock.
                typeof(PresentAfterDraw),
#endif

                // Keep Profiler for debugging
                typeof(ProfilerStartFrame),
                typeof(ProfilerSynchronizeStats),
                typeof(ProfilerEndFrame),
            });


            IPlayerLoopProfile idle = new PlayerLoopProfileBuilder()
                .Filter(idleFilter, FilterType.KEEP)
                .OnInteraction(InteractionActionIdle)
                .build();

            IPlayerLoopProfile normal = new PlayerLoopProfileBuilder()
               .OnTimeout(TimeoutActionActive)
               .build();

            LowPowerManager.Instance.playerLoopManager.AddProfile(Profile.IDLE, idle);
            LowPowerManager.Instance.playerLoopManager.AddProfile(Profile.NORMAL, normal);

            LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.NORMAL);
        }

        private void InteractionActionIdle()
        {
            Debug.Log("Idle Interaction Happened!");
            LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.NORMAL);
        }

        private void TimeoutActionActive()
        {
            Debug.Log("Active Timeout Happened!");
            LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.IDLE);
        }

        // Update is called once per frame
        private int frameCount;
        void Update()
        {
            frameCount++;
            Cube.transform.RotateAround(Vector3.zero, Vector3.forward, 1);
            Frames.SetText("Frame count: " + frameCount);
        }
    }
}