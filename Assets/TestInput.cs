using Assets.Scripts.LowPower;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Assets.Scripts.Example;
using static UnityEngine.InputSystem.InputAction;

public class TestInput : MonoBehaviour
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
        MOVE,
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
        "Move", 
        "Submit", 
        "Cancel", 
        "TrackedDevicePosition",
        "TrackedDeviceOrientation"
    }.ToList();

    private Dictionary<string, UnityAction<CallbackContext>> actions = new Dictionary<string, UnityAction<CallbackContext>>();


    private void Start()
    {
        virtualMouse = InputSystem.AddDevice<Mouse>();

        actions.Add(actionNames[(int)ActionType.NAVIGATE], Navigate);
        actions.Add(actionNames[(int)ActionType.POINT], Point);
        actions.Add(actionNames[(int)ActionType.RIGHT_CLICK], RightClick);
        actions.Add(actionNames[(int)ActionType.MIDDLE_CLICK], MiddleClick);
        actions.Add(actionNames[(int)ActionType.CLICK], Click);
        actions.Add(actionNames[(int)ActionType.SCROLL_WHEEL], ScrollWheel);
        actions.Add(actionNames[(int)ActionType.MOVE], Move);
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


    public void Navigate(CallbackContext pContext)
    {
        Debug.Log("Navigate");
        LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.NORMAL);
    }

    public void Point(CallbackContext pContext)
    {
        Debug.Log("Point");
        LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.NORMAL);
    }

    public void Click(CallbackContext pContext)
    {
        Debug.Log("Click");
        LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.NORMAL);
    }

    public void MiddleClick(CallbackContext pContext)
    {
        Debug.Log("MiddleClick");
        LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.NORMAL);
    }

    public void RightClick(CallbackContext pContext)
    {
        Debug.Log("RightClick");
        LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.NORMAL);
    }

    public void ScrollWheel(CallbackContext pContext)
    {
        Debug.Log("ScrollWheel");
        LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.NORMAL);
    }

    public void Move(CallbackContext pContext)
    {
        Debug.Log("Move");
        LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.NORMAL);
    }

    public void Submit(CallbackContext pContext)
    {
        Debug.Log("Submit");
        LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.NORMAL);
    }

    public void Cancel(CallbackContext pContext)
    {
        Debug.Log("Cancel");
        LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.NORMAL);
    }

    public void TrackedDevicePosition(CallbackContext pContext)
    {
        Debug.Log("TrackedDevicePosition");
        LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.NORMAL);
    }

    public void TrackedDeviceOrientation(CallbackContext pContext)
    {
        Debug.Log("TrackedDeviceOrientation");
        LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.NORMAL);
    }
}
