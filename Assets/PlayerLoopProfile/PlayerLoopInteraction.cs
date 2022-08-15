using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace PlayerLoopProfiles
{
    public class PlayerLoopInteraction : MonoBehaviour
    {
        [SerializeField] private PlayerInput input;

        public enum InteractionType
        {
            NAVIGATE,
            POINT,
            RIGHT_CLICK,
            MIDDLE_CLICK,
            CLICK,
            SCROLL_WHEEL,
            SUBMIT,
            CANCEL,
            TRACKED_DEVICE_POSITION,
            TRACKED_DEVICE_ORIENTATION,
        }

        private List<string> actionNames = new string[]
        {
            "Navigate",
            "Point",
            "RightClick",
            "MiddleClick",
            "Click",
            "ScrollWheel",
            "Submit",
            "Cancel",
            "TrackedDevicePosition",
            "TrackedDeviceOrientation"
        }.ToList();

        private void Start()
        {
            string prefix = "UI/";
            foreach (PlayerInput.ActionEvent element in input.actionEvents)
            {
                if (!element.actionName.StartsWith("UI/"))
                {
                    continue;
                }

                string name = element.actionName;
                name = name.Substring(name.IndexOf(prefix) + prefix.Length);
                if (name.Contains("["))
                {
                    name = name.Substring(0, name.IndexOf("["));
                }

                InteractionType type = (InteractionType) actionNames.IndexOf(name);
                element.AddListener((context) => Interaction(context, type));
            }
        }

        private void Interaction(CallbackContext pContext, InteractionType pType)
        {
            PlayerLoopTimeout.AddInteraction(pType);
        }
    }
}
