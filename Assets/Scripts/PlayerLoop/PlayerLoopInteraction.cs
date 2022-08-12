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

        private Dictionary<string, UnityAction<CallbackContext>> actions = new Dictionary<string, UnityAction<CallbackContext>>();

        private void Start()
        {
            actions.Add(actionNames[(int)InteractionType.NAVIGATE], Navigate);
            actions.Add(actionNames[(int)InteractionType.POINT], Point);
            actions.Add(actionNames[(int)InteractionType.RIGHT_CLICK], RightClick);
            actions.Add(actionNames[(int)InteractionType.MIDDLE_CLICK], MiddleClick);
            actions.Add(actionNames[(int)InteractionType.CLICK], Click);
            actions.Add(actionNames[(int)InteractionType.SCROLL_WHEEL], ScrollWheel);
            actions.Add(actionNames[(int)InteractionType.SUBMIT], Submit);
            actions.Add(actionNames[(int)InteractionType.CANCEL], Cancel);
            actions.Add(actionNames[(int)InteractionType.TRACKED_DEVICE_POSITION], TrackedDevicePosition);
            actions.Add(actionNames[(int)InteractionType.TRACKED_DEVICE_ORIENTATION], TrackedDeviceOrientation);

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

                if (actions.TryGetValue(name, out UnityAction<CallbackContext> result))
                {
                    element.AddListener(result);
                }
            }
        }

        public void Navigate(CallbackContext pContext) => Interaction(pContext, InteractionType.NAVIGATE);

        public void Point(CallbackContext pContext) => Interaction(pContext, InteractionType.POINT);

        public void Click(CallbackContext pContext) => Interaction(pContext, InteractionType.CLICK);

        public void MiddleClick(CallbackContext pContext) => Interaction(pContext, InteractionType.MIDDLE_CLICK);

        public void RightClick(CallbackContext pContext) => Interaction(pContext, InteractionType.RIGHT_CLICK);

        public void ScrollWheel(CallbackContext pContext) => Interaction(pContext, InteractionType.SCROLL_WHEEL);

        public void Submit(CallbackContext pContext) => Interaction(pContext, InteractionType.SUBMIT);

        public void Cancel(CallbackContext pContext) => Interaction(pContext, InteractionType.CANCEL);

        public void TrackedDevicePosition(CallbackContext pContext) => Interaction(pContext, InteractionType.TRACKED_DEVICE_POSITION);

        public void TrackedDeviceOrientation(CallbackContext pContext) => Interaction(pContext, InteractionType.TRACKED_DEVICE_ORIENTATION);

        private void Interaction(CallbackContext pContext, InteractionType pType)
        {
            PlayerLoopTimeout.AddInteraction(pType);
        }
    }
}
