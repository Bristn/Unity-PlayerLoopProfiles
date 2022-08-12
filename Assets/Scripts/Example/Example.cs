using Assets.Scripts.PlayerLoop;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
using static Assets.Scripts.PlayerLoop.PlayerLoopInteraction;
using static Assets.Scripts.PlayerLoop.PlayerLoopProfile;
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

           

            PlayerLoopProfile idle = new PlayerLoopProfileBuilder()
                .FilterSystems(idleFilter)
                .FilterType(FilterType.KEEP)
                .InteractionCallback(InteractionActionIdle)
                .IgnoreInteraction(InteractionType.POINT)
                .Build();

            PlayerLoopProfile normal = new PlayerLoopProfileBuilder()
               .TimeoutCallback(TimeoutActionActive)
               .TimeoutDuration(0.1f) 
               .UI(typeof(TMP_InputField), CallbackTextMeshPro)
               .UI(typeof(TextField), CallbackUiToolkit)
               .Build();

            PlayerLoopManager.AddProfile(Profile.IDLE, idle);
            PlayerLoopManager.AddProfile(Profile.NORMAL, normal);

            PlayerLoopManager.SetActiveProfile(Profile.IDLE);
        }

        private bool CallbackTextMeshPro(Component pTextField)
        {
            return ((TMP_InputField)pTextField).isFocused;
        }

        private bool CallbackUiToolkit(Focusable pTextField)
        {
            return true;
        }

        private void InteractionActionIdle(InteractionType pType)
        {
            Debug.Log("Interaction: " + pType);
            if (pType == InteractionType.SCROLL_WHEEL)
            {
                return;
            }

            PlayerLoopManager.SetActiveProfile(Profile.NORMAL);
        }

        private void TimeoutActionActive()
        {
            Debug.Log("Timeout");
            PlayerLoopManager.SetActiveProfile(Profile.IDLE);
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