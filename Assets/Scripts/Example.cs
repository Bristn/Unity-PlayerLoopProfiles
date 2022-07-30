﻿using Assets.Scripts.LowPower;
using Assets.Scripts.LowPower.PlayerLoop;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.PlayerLoop;
using static Assets.Scripts.LowPower.LowPowerImplementation;
using static Assets.Scripts.LowPower.PlayerLoop.IPlayerLoopProfile;
using static UnityEngine.PlayerLoop.FixedUpdate;
using static UnityEngine.PlayerLoop.Initialization;
using static UnityEngine.PlayerLoop.PostLateUpdate;
using static UnityEngine.PlayerLoop.PreUpdate;

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

                // input Test
                typeof(SynchronizeInputs),
                typeof(EarlyUpdate.UpdateInputManager),
                typeof(EarlyUpdate.ProcessRemoteInput),
                typeof(NewInputFixedUpdate),
                typeof(CheckTexFieldInput),
                typeof(NewInputUpdate),
                typeof(InputEndFrame),
                typeof(ResetInputAxis),
            });

           
            Dictionary<Type, Test> ui = new Dictionary<Type, Test>();
            ui.Add(typeof(TMP_InputField), aa);

            IPlayerLoopProfile idle = new PlayerLoopProfileBuilder()
                .FilterSystems(idleFilter)
                .FilterType(FilterType.KEEP)
                .InteractionCallback(InteractionActionIdle)
                .IgnoreInteraction(InteractionType.DSEKTOP_MOUSE_MOVE)
                .Build();

            IPlayerLoopProfile normal = new PlayerLoopProfileBuilder()
               .TimeoutCallback(TimeoutActionActive)
                //.UI(ui)
               .Build();

            LowPowerManager.Instance.playerLoopManager.AddProfile(Profile.IDLE, idle);
            LowPowerManager.Instance.playerLoopManager.AddProfile(Profile.NORMAL, normal);

            LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.IDLE);
        }

        private bool aa(Component pComp)
        {
            Debug.Log("Custom");
            TMP_InputField tmpInputField = (TMP_InputField)pComp;
                return tmpInputField.isFocused;

        }

        private void InteractionActionIdle(InteractionType pType)
        {
            Debug.Log("Interaction: " + pType);
            if (pType == InteractionType.DSEKTOP_MOUSE_SCROLL)
            {
                return;
            }

            LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.NORMAL);
        }

        private void TimeoutActionActive()
        {
            Debug.Log("Timeout");
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