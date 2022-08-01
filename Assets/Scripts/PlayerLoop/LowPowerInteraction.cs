using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.PlayerLoop
{
    public class LowPowerInteraction : MonoBehaviour
    {
        [SerializeField] private PlayerInput input;

        public enum ActionType
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
            actions.Add(actionNames[(int)ActionType.NAVIGATE], Navigate);
            actions.Add(actionNames[(int)ActionType.POINT], Point);
            actions.Add(actionNames[(int)ActionType.RIGHT_CLICK], RightClick);
            actions.Add(actionNames[(int)ActionType.MIDDLE_CLICK], MiddleClick);
            actions.Add(actionNames[(int)ActionType.CLICK], Click);
            actions.Add(actionNames[(int)ActionType.SCROLL_WHEEL], ScrollWheel);
            actions.Add(actionNames[(int)ActionType.SUBMIT], Submit);
            actions.Add(actionNames[(int)ActionType.CANCEL], Cancel);
            actions.Add(actionNames[(int)ActionType.TRACKED_DEVICE_POSITION], TrackedDevicePosition);
            actions.Add(actionNames[(int)ActionType.TRACKED_DEVICE_ORIENTATION], TrackedDeviceOrientation);

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

        public void Navigate(CallbackContext pContext) => Interaction(pContext, ActionType.NAVIGATE);

        public void Point(CallbackContext pContext) => Interaction(pContext, ActionType.POINT);

        public void Click(CallbackContext pContext) => Interaction(pContext, ActionType.CLICK);

        public void MiddleClick(CallbackContext pContext) => Interaction(pContext, ActionType.MIDDLE_CLICK);

        public void RightClick(CallbackContext pContext) => Interaction(pContext, ActionType.RIGHT_CLICK);

        public void ScrollWheel(CallbackContext pContext) => Interaction(pContext, ActionType.SCROLL_WHEEL);

        public void Submit(CallbackContext pContext) => Interaction(pContext, ActionType.SUBMIT);

        public void Cancel(CallbackContext pContext) => Interaction(pContext, ActionType.CANCEL);

        public void TrackedDevicePosition(CallbackContext pContext) => Interaction(pContext, ActionType.TRACKED_DEVICE_POSITION);

        public void TrackedDeviceOrientation(CallbackContext pContext) => Interaction(pContext, ActionType.TRACKED_DEVICE_ORIENTATION);

        private void Interaction(CallbackContext pContext, ActionType pType)
        {
            LowPowerTimeout.AddInteraction(pType);
        }
    }
}
